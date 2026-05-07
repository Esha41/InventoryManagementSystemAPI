using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Inventory.Service.Assets;
using Ettad.Inventory.Service.Assets.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Inventory.Service.Assets.Implementation
{
    public class AssetBulkDeletionProcessor : IAssetBulkDeletionProcessor
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrossCuttingRepository<Batch> _batchRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<AssetBulkDeletionProcessor> _logger;

        public AssetBulkDeletionProcessor(
            ApplicationDbContext context,
            ICrossCuttingRepository<Batch> batchRepository,
            IFileUploadService fileUploadService,
            IDateTimeProvider dateTimeProvider,
            ILogger<AssetBulkDeletionProcessor> logger)
        {
            _context = context;
            _batchRepository = batchRepository;
            _fileUploadService = fileUploadService;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid jobId, CancellationToken cancellationToken = default)
        {
            _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(30));

            var job = await _context.AssetBulkDeletionJobs
                .AsTracking()
                .FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken).ConfigureAwait(false);

            if (job == null)
            {
                _logger.LogWarning("AssetBulkDeletionJob not found in database. JobId={JobId}", jobId);
                return;
            }

            if (job.JobStatus == AssetBulkDeletionConstants.JobStatus.Completed)
                return;

            try
            {
                if (job.JobStatus == AssetBulkDeletionConstants.JobStatus.Pending)
                {
                    job.JobStatus = AssetBulkDeletionConstants.JobStatus.Running;
                    job.StartedUtc = _dateTimeProvider.Now.ToUniversalTime();
                    await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }

                await RunDeletionAsync(job, cancellationToken).ConfigureAwait(false);

                job.JobStatus = AssetBulkDeletionConstants.JobStatus.Completed;
                job.CompletedUtc = _dateTimeProvider.Now.ToUniversalTime();
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bulk asset deletion failed. JobId={JobId}", jobId);
                job.JobStatus = AssetBulkDeletionConstants.JobStatus.Failed;
                job.Message = $"{ex.GetType().Name}: {ex.Message}";
                job.CompletedUtc = _dateTimeProvider.Now.ToUniversalTime();
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task RunDeletionAsync(AssetBulkDeletionJob job, CancellationToken cancellationToken)
        {
            string deletedBy = string.IsNullOrWhiteSpace(job.InitiatedByUserId) ? "system" : job.InitiatedByUserId;

            switch (job.Scope)
            {
                case AssetBulkDeletionConstants.Scope.Batch:
                    await CleanupBatchUploadedFiles(job.BatchId!.Value).ConfigureAwait(false);
                    await DeleteAssetsInRepeatedChunks(job, q => q.Where(a =>
                        !a.IsDeleted && a.BatchId == job.BatchId!.Value), deletedBy,
                        cancellationToken).ConfigureAwait(false);
                    await SoftDeleteBatch(job.BatchId.Value, deletedBy).ConfigureAwait(false);
                    break;

                case AssetBulkDeletionConstants.Scope.Depot:
                    await DeleteAssetsInRepeatedChunks(job, q =>
                        q.Where(a => !a.IsDeleted && a.DepotId == job.DepotId!.Value),
                        deletedBy, cancellationToken).ConfigureAwait(false);
                    break;

                case AssetBulkDeletionConstants.Scope.ExplicitIds:
                    await DeleteExplicitListedAssetsAsync(job, deletedBy, cancellationToken).ConfigureAwait(false);
                    break;

                default:
                    throw new InvalidOperationException($"Unknown bulk delete scope {(int)job.Scope}");
            }

            job.Message ??= $"Successfully soft-deleted {job.DeletedCount} asset(s).";
        }

        private async Task CleanupBatchUploadedFiles(long batchId)
        {
            try
            {
                var filesResp = await _fileUploadService
                    .GetByEntitiesAsync(FileEntityType.Weapon, new List<long> { batchId })
                    .ConfigureAwait(false);

                if (!filesResp.Succeeded || filesResp.Data == null || !filesResp.Data.Any())
                    return;

                var fileIds = filesResp.Data.Values
                    .Where(v => v != null)
                    .SelectMany(v => v)
                    .Select(f => f.Id)
                    .Distinct()
                    .ToList();

                foreach (var fileId in fileIds)
                {
                    try
                    {
                        await _fileUploadService.DeleteAsync(fileId).ConfigureAwait(false);
                    }
                    catch (Exception exDel)
                    {
                        _logger.LogWarning(exDel, "Failed deleting file {FileId} during bulk Asset delete batch {BatchId}",
                            fileId, batchId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error while cleaning uploaded files during bulk Asset delete batch {BatchId}",
                    batchId);
            }
        }

        private async Task SoftDeleteBatch(long batchId, string deletedBy)
        {
            var batch = await _batchRepository.FindOneAsync(b => b.Id == batchId && !b.IsDeleted)
                .ConfigureAwait(false);

            if (batch == null)
            {
                _logger.LogWarning("Batch {BatchId} not found for soft-delete after assets removed", batchId);
                return;
            }

            await _batchRepository.DeleteAsync(batch).ConfigureAwait(false);
        }

        private async Task DeleteAssetsInRepeatedChunks(
            AssetBulkDeletionJob job,
            Func<IQueryable<Asset>, IQueryable<Asset>> filterAssets,
            string deletedBy,
            CancellationToken cancellationToken)
        {
            var baseQuery = _context.Assets.AsNoTracking();

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var ids = await filterAssets(baseQuery)
                    .OrderBy(a => a.Id)
                    .Take(AssetBulkDeletionConstants.UpdateChunkSize)
                    .Select(a => a.Id)
                    .ToListAsync(cancellationToken).ConfigureAwait(false);

                if (ids.Count == 0)
                    break;

                await ApplySoftDeleteChunk(ids, deletedBy, job, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private async Task DeleteExplicitListedAssetsAsync(
            AssetBulkDeletionJob job,
            string deletedBy,
            CancellationToken cancellationToken)
        {
            List<long>? allIds = null;
            try
            {
                allIds = JsonSerializer.Deserialize<List<long>>(job.ExplicitAssetIdsJson ?? "[]") ?? new List<long>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Invalid ExplicitAssetIdsJson on job.", ex);
            }

            var orderedDistinct = allIds.Where(id => id > 0).Distinct().OrderBy(id => id).ToList();

            for (var i = 0; i < orderedDistinct.Count; i += AssetBulkDeletionConstants.ExplicitSliceSize)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var slice = orderedDistinct
                    .Skip(i)
                    .Take(AssetBulkDeletionConstants.ExplicitSliceSize)
                    .ToList();

                while (true)
                {
                    var ids = await _context.Assets.AsNoTracking()
                        .Where(a => slice.Contains(a.Id) && !a.IsDeleted)
                        .OrderBy(a => a.Id)
                        .Take(AssetBulkDeletionConstants.UpdateChunkSize)
                        .Select(a => a.Id)
                        .ToListAsync(cancellationToken).ConfigureAwait(false);

                    if (ids.Count == 0)
                        break;

                    await ApplySoftDeleteChunk(ids, deletedBy, job, cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Soft-deletes one chunk of assets and persists job counters in a single transaction so either
        /// both commit or both roll back.
        /// </summary>
        private async Task ApplySoftDeleteChunk(
            List<long> ids,
            string deletedBy,
            AssetBulkDeletionJob job,
            CancellationToken cancellationToken)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            var processedDelta = 0;
            var deletedDelta = 0;
            var countersApplied = false;

            try
            {
                var utcNow = _dateTimeProvider.Now.ToUniversalTime();

                var updated = await _context.Assets
                    .Where(a => ids.Contains(a.Id) && !a.IsDeleted)
                    .ExecuteUpdateAsync(s => s
                            .SetProperty(a => a.IsDeleted, true)
                            .SetProperty(a => a.DeletionDate, utcNow)
                            .SetProperty(a => a.DeletedBy, deletedBy)
                            .SetProperty(a => a.ModificationDate, utcNow)
                            .SetProperty(a => a.ModifiedBy, deletedBy),
                        cancellationToken)
                    .ConfigureAwait(false);

                processedDelta = ids.Count;
                deletedDelta = updated;

                job.ProcessedCount += processedDelta;
                job.DeletedCount += deletedDelta;
                countersApplied = true;

                await PersistJobProgress(job, cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                if (countersApplied)
                {
                    job.ProcessedCount -= processedDelta;
                    job.DeletedCount -= deletedDelta;
                }

                throw;
            }
        }

        private Task PersistJobProgress(AssetBulkDeletionJob job, CancellationToken ct)
            => _context.SaveChangesAsync(ct);
    }
}
