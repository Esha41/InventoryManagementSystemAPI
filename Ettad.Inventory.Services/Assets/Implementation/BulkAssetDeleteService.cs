using System;

using System.Collections.Generic;

using System.Linq;

using System.Text.Json;

using System.Threading.Tasks;

using Ettad.Application.Common.Interfaces;

using Ettad.Data.Entities;

using Ettad.Data.Interfaces.Repositories;

using Ettad.Inventory.Service.Assets.Dtos;

using Ettad.Inventory.Service.Assets.Interfaces;

using Ettad.Module.lookup.Interfaces;

using Ettad.ResponseHandler.Consts;

using Ettad.ResponseHandler.Models;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Logging;



namespace Ettad.Inventory.Service.Assets.Implementation

{

    public class BulkAssetDeleteService : IBulkAssetDeleteService

    {

        private const string FeatureSection = "Features:AssetBulkDelete:Enabled";



        /// <summary>Upper bound validated before enqueue (matches bulk template ceiling).</summary>

        private const int MaxAssetsPerJob = 1_000_000;



        private readonly ICrossCuttingRepository<Asset> _assetRepository;

        private readonly ICrossCuttingRepository<Batch> _batchRepository;

        private readonly ICrossCuttingRepository<AssetSupplyDetail> _assetSupplyDetailRepository;

        private readonly ICrossCuttingRepository<AssetBulkDeletionJob> _bulkDeletionJobRepository;



        private readonly ICurrentUserService _currentUserService;

        private readonly IDepotAccessService _depotAccessService;

        private readonly FluentValidation.IValidator<StartBulkDeleteAssetsDto> _dtoValidator;

        private readonly IAssetBulkDeleteJobScheduler _jobScheduler;

        private readonly IConfiguration _configuration;

        private readonly ILogger<BulkAssetDeleteService> _logger;



        public BulkAssetDeleteService(

            ICrossCuttingRepository<Asset> assetRepository,

            ICrossCuttingRepository<Batch> batchRepository,

            ICrossCuttingRepository<AssetSupplyDetail> assetSupplyDetailRepository,

            ICrossCuttingRepository<AssetBulkDeletionJob> bulkDeletionJobRepository,

            ICurrentUserService currentUserService,

            IDepotAccessService depotAccessService,

            FluentValidation.IValidator<StartBulkDeleteAssetsDto> dtoValidator,

            IAssetBulkDeleteJobScheduler jobScheduler,

            IConfiguration configuration,

            ILogger<BulkAssetDeleteService> logger)

        {

            _assetRepository = assetRepository;

            _batchRepository = batchRepository;

            _assetSupplyDetailRepository = assetSupplyDetailRepository;

            _bulkDeletionJobRepository = bulkDeletionJobRepository;

            _currentUserService = currentUserService;

            _depotAccessService = depotAccessService;

            _dtoValidator = dtoValidator;

            _jobScheduler = jobScheduler;

            _configuration = configuration;

            _logger = logger;

        }



        public async Task<APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>> StartBulkDeleteAsync(

            StartBulkDeleteAssetsDto dto)

        {

            if (!(_configuration.GetValue<bool?>(FeatureSection) ?? true))

            {

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(

                    ResponseType.BadRequest,

                    "Async bulk asset delete is disabled on this server.");

            }



            if (dto == null)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(

                    ResponseType.BadRequest, "Request body is required.");



            var fv = await _dtoValidator.ValidateAsync(dto).ConfigureAwait(false);

            if (!fv.IsValid)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                    string.Join("; ", fv.Errors.Select(e => e.ErrorMessage)));



            var userId = _currentUserService.UserId;

            try

            {

                return dto.Scope switch

                {

                    AssetBulkDeletionScopeDto.Batch =>

                        await StartForBatchScope(dto.BatchId!.Value, userId).ConfigureAwait(false),

                    AssetBulkDeletionScopeDto.Depot =>

                        await StartForDepotScope(dto.DepotId!.Value, userId).ConfigureAwait(false),

                    AssetBulkDeletionScopeDto.ExplicitIds =>

                        await StartForExplicitScope(dto.AssetIds!, userId).ConfigureAwait(false),

                    _ => APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(

                        ResponseType.BadRequest, "Invalid scope.")

                };

            }

            catch (Exception ex)

            {

                _logger.LogError(ex, "StartBulkDeleteAsync failed for user {UserId}", userId);

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(

                    ResponseType.InternalServerError, $"Could not enqueue bulk delete: {ex.Message}");

            }

        }



        public async Task<APIOperationResponse<BulkDeleteAssetsStatusDto>> GetBulkDeleteStatusAsync(Guid jobId)

        {

            var job = await _bulkDeletionJobRepository

                .FindOneAsync(j => j.Id == jobId)

                .ConfigureAwait(false);



            if (job == null)

                return APIOperationResponse<BulkDeleteAssetsStatusDto>.Fail(ResponseType.NotFound, "Job not found.");



            var userId = _currentUserService.UserId;

            if (!string.Equals(job.InitiatedByUserId, userId, StringComparison.Ordinal))

                return APIOperationResponse<BulkDeleteAssetsStatusDto>.Fail(

                    ResponseType.Forbidden, "You cannot view this job.");



            var statusLabel = MapStatus(job.JobStatus);

            var percent = job.TotalCandidates > 0

                ? Math.Min(100d, Math.Round(job.ProcessedCount * 100.0 / job.TotalCandidates, 2))

                : (job.JobStatus == BulkDeleteAssetsDtos.JobStatus.Completed ? 100 : 0);



            var dto = new BulkDeleteAssetsStatusDto

            {

                JobId = job.Id.ToString(),

                Status = statusLabel,

                TotalCandidates = job.TotalCandidates,

                ProcessedCount = job.ProcessedCount,

                DeletedCount = job.DeletedCount,

                ProgressPercent = percent,

                Message = job.Message,

                StartedUtc = job.StartedUtc,

                CompletedUtc = job.CompletedUtc

            };



            return APIOperationResponse<BulkDeleteAssetsStatusDto>.Success(dto);

        }



        private static string MapStatus(byte status) => status switch

        {

            BulkDeleteAssetsDtos.JobStatus.Pending => "Pending",

            BulkDeleteAssetsDtos.JobStatus.Running => "Running",

            BulkDeleteAssetsDtos.JobStatus.Completed => "Completed",

            BulkDeleteAssetsDtos.JobStatus.Failed => "Failed",

            _ => "Unknown"

        };



        private async Task<APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>> StartForExplicitScope(

            List<long> assetIds, string userId)

        {

            var distinct = assetIds.Where(id => id > 0).Distinct().OrderBy(i => i).ToList();



            for (var i = 0; i < distinct.Count; i += BulkDeleteAssetsDtos.ExplicitSliceSize)

            {

                var slice = distinct.Skip(i).Take(BulkDeleteAssetsDtos.ExplicitSliceSize).ToList();



                if (!string.IsNullOrEmpty(userId))

                {

                    var depotRows = await _assetRepository

                        .Find(a => slice.Contains(a.Id) && !a.IsDeleted)

                        .Select(a => a.DepotId)

                        .Distinct()

                        .ToListAsync()

                        .ConfigureAwait(false);



                    foreach (var dId in depotRows)

                    {

                        if (!await _depotAccessService.HasDepotAccessAsync(userId, dId).ConfigureAwait(false))

                        {

                            return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.Forbidden,

                                $"You do not have access to depot {dId} for one or more listed assets.");

                        }

                    }

                }



                var hasAssigned = await _assetRepository

                    .Find(a => slice.Contains(a.Id) && !a.IsDeleted && a.IsAssigned)

                    .AnyAsync()

                    .ConfigureAwait(false);



                if (hasAssigned)

                    return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                        "Cannot delete assigned assets — unassign them first.");



                var inSupply = await _assetSupplyDetailRepository

                    .Find(sd => !sd.IsDeleted && slice.Contains(sd.AssetId))

                    .AnyAsync()

                    .ConfigureAwait(false);



                if (inSupply)

                    return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                        "Cannot delete assets that appear on supply orders.");

            }



            var activeCount = await CountActiveExplicitAssets(distinct).ConfigureAwait(false);



            if (activeCount == 0)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                    "None of the listed assets exist as active rows.");



            if (activeCount > MaxAssetsPerJob)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                    $"At most {MaxAssetsPerJob:N0} assets per job.");



            var json = JsonSerializer.Serialize(distinct);

            var jobRow = BuildJobRow(userId ?? "anonymous", BulkDeleteAssetsDtos.Scope.ExplicitIds, null,

                null, explicitIdsJson: json, totalCandidates: activeCount);



            return await SaveAndEnqueue(jobRow).ConfigureAwait(false);

        }



        private async Task<int> CountActiveExplicitAssets(List<long> distinctSorted)

        {

            var totalTracked = 0;

            for (var i = 0; i < distinctSorted.Count; i += BulkDeleteAssetsDtos.ExplicitSliceSize)

            {

                var slice = distinctSorted.Skip(i).Take(BulkDeleteAssetsDtos.ExplicitSliceSize).ToList();

                totalTracked += await _assetRepository

                    .Find(a => slice.Contains(a.Id) && !a.IsDeleted)

                    .CountAsync()

                    .ConfigureAwait(false);

            }



            return totalTracked;

        }



        private async Task<APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>> StartForBatchScope(long batchId,

            string userId)

        {

            var batch = await _batchRepository

                .FindOneAsync(b => b.Id == batchId && !b.IsDeleted)

                .ConfigureAwait(false);



            if (batch == null)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(

                    ResponseType.NotFound, "Batch not found.");



            if (!string.IsNullOrEmpty(userId)

                && !await _depotAccessService.HasDepotAccessAsync(userId, batch.DepotId).ConfigureAwait(false))

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.Forbidden,

                    "You do not have access to this depot.");



            var precondition = await CheckBatchAssignablePreconditions(batchId).ConfigureAwait(false);

            if (precondition != null)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(

                    ResponseType.BadRequest, precondition);



            var total = await CountActiveAssets(batchIdFilter: batchId, depotFilter: null)

                .ConfigureAwait(false);



            if (total == 0)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                    "No active assets exist for this batch.");



            if (total > MaxAssetsPerJob)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                    $"At most {MaxAssetsPerJob:N0} assets can be processed in one job.");



            var jobRow = BuildJobRow(userId ?? "anonymous", BulkDeleteAssetsDtos.Scope.Batch, batchId,

                depotId: null, explicitIdsJson: null, totalCandidates: total);



            return await SaveAndEnqueue(jobRow).ConfigureAwait(false);

        }



        private async Task<APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>> StartForDepotScope(long depotId,

            string userId)

        {

            if (!string.IsNullOrEmpty(userId)

                && !await _depotAccessService.HasDepotAccessAsync(userId, depotId).ConfigureAwait(false))

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.Forbidden,

                    "You do not have access to this depot.");



            var precondition = await CheckDepotAssignablePreconditions(depotId).ConfigureAwait(false);

            if (precondition != null)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                    precondition);



            var total = await CountActiveAssets(batchIdFilter: null, depotFilter: depotId)

                .ConfigureAwait(false);



            if (total == 0)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                    "No active assets exist for this depot.");



            if (total > MaxAssetsPerJob)

                return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Fail(ResponseType.BadRequest,

                    $"At most {MaxAssetsPerJob:N0} assets can be processed in one job.");



            var jobRow =

                BuildJobRow(userId ?? "anonymous", BulkDeleteAssetsDtos.Scope.Depot, batchId: null, depotId,

                    null, total);



            return await SaveAndEnqueue(jobRow).ConfigureAwait(false);

        }



        private async Task<string?> CheckBatchAssignablePreconditions(long batchId)

        {

            var assignedExists = await _assetRepository

                .Find(a => !a.IsDeleted && a.BatchId == batchId && a.IsAssigned)

                .AnyAsync()

                .ConfigureAwait(false);



            var inSupplyExists = await (

                    from sd in _assetSupplyDetailRepository.Find(sd => !sd.IsDeleted)

                    join a in _assetRepository.Find(a => !a.IsDeleted && a.BatchId == batchId)

                        on sd.AssetId equals a.Id

                    select 1)

                .AnyAsync()

                .ConfigureAwait(false);



            if (assignedExists || inSupplyExists)

            {

                return "Cannot bulk delete assets: remove assignments and ensure no assets remain in supply orders (same rule as deleting a batch).";

            }



            return null;

        }



        private async Task<string?> CheckDepotAssignablePreconditions(long depotId)

        {

            var assignedExists = await _assetRepository

                .Find(a => !a.IsDeleted && a.DepotId == depotId && a.IsAssigned)

                .AnyAsync()

                .ConfigureAwait(false);



            var inSupplyExists = await (

                    from sd in _assetSupplyDetailRepository.Find(sd => !sd.IsDeleted)

                    join a in _assetRepository.Find(a => !a.IsDeleted && a.DepotId == depotId)

                        on sd.AssetId equals a.Id

                    select 1)

                .AnyAsync()

                .ConfigureAwait(false);



            if (assignedExists || inSupplyExists)

            {

                return "Cannot bulk delete assets in this depot while some are assigned or referenced on supply orders.";

            }



            return null;

        }



        private Task<int> CountActiveAssets(long? batchIdFilter, long? depotFilter)

        {

            var q = _assetRepository.Find(a => !a.IsDeleted);



            if (batchIdFilter.HasValue)

                q = q.Where(a => a.BatchId == batchIdFilter.Value);



            if (depotFilter.HasValue)

                q = q.Where(a => a.DepotId == depotFilter.Value);



            return q.CountAsync();

        }



        private static AssetBulkDeletionJob BuildJobRow(

            string userId,

            byte scope,

            long? batchId,

            long? depotId,

            string explicitIdsJson,

            int totalCandidates)

        {

            return new AssetBulkDeletionJob

            {

                Id = Guid.NewGuid(),

                InitiatedByUserId = userId ?? "anonymous",

                JobStatus = BulkDeleteAssetsDtos.JobStatus.Pending,

                Scope = scope,

                BatchId = batchId,

                DepotId = depotId,

                ExplicitAssetIdsJson = explicitIdsJson,

                TotalCandidates = totalCandidates,

                ProcessedCount = 0,

                DeletedCount = 0,

                CreatedUtc = DateTime.UtcNow,

                HangfireJobId = null,

                Message = null

            };

        }



        private async Task<APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>> SaveAndEnqueue(

            AssetBulkDeletionJob row)

        {

            await _bulkDeletionJobRepository.AddAsync(row).ConfigureAwait(false);



            var hangfireId = _jobScheduler.Enqueue(row.Id);

            row.HangfireJobId = hangfireId;

            row.Message =

                $"{row.TotalCandidates:N0} asset(s) queued for soft-delete.";

            await _bulkDeletionJobRepository.UpdateAsync(row).ConfigureAwait(false);



            var result = new BulkDeleteAssetsEnqueueResultDto

            {

                JobId = row.Id.ToString(),

                HangfireJobId = hangfireId,

                TotalCandidates = row.TotalCandidates

            };



            return APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>.Success(result);

        }

    }

}
