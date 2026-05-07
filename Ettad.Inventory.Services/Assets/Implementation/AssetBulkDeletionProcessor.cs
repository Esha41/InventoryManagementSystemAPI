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
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Service.Assets.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Inventory.Service.Assets.Implementation
{
    public class AssetBulkDeletionProcessor : IAssetBulkDeletionProcessor
    {
        private readonly IAssetBulkDeletionPersistence _persistence;
        private readonly ICrossCuttingRepository<Batch> _batchRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<AssetBulkDeletionProcessor> _logger;

        public AssetBulkDeletionProcessor(
            IAssetBulkDeletionPersistence persistence,
            ICrossCuttingRepository<Batch> batchRepository,
            IFileUploadService fileUploadService,
            IDateTimeProvider dateTimeProvider,
            ITransactionManager transactionManager,
            ILogger<AssetBulkDeletionProcessor> logger)
        {
            _persistence = persistence;
            _batchRepository = batchRepository;
            _fileUploadService = fileUploadService;
            _dateTimeProvider = dateTimeProvider;
            _transactionManager = transactionManager;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid jobId, CancellationToken cancellationToken = default)
        {
            _persistence.SetBulkCommandTimeout(TimeSpan.FromMinutes(30));

            var job = await _persistence.GetTrackedJobAsync(jobId, cancellationToken).ConfigureAwait(false);

            if (job == null)
            {
                _logger.LogWarning("AssetBulkDeletionJob not found in database. JobId={JobId}", jobId);
                return;
            }

            if (job.JobStatus == BulkDeleteAssetsDtos.JobStatus.Completed)
                return;

            try
            {
                if (job.JobStatus == BulkDeleteAssetsDtos.JobStatus.Pending)
                {
                    job.JobStatus = BulkDeleteAssetsDtos.JobStatus.Running;
                    job.StartedUtc = _dateTimeProvider.Now;
                    await _persistence.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }

                await RunDeletionAsync(job, cancellationToken).ConfigureAwait(false);

                job.JobStatus = BulkDeleteAssetsDtos.JobStatus.Completed;
                job.CompletedUtc = _dateTimeProvider.Now;
                await _persistence.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bulk asset deletion failed. JobId={JobId}", jobId);
                job.JobStatus = BulkDeleteAssetsDtos.JobStatus.Failed;
                job.Message = $"{ex.GetType().Name}: {ex.Message}";
                job.CompletedUtc = _dateTimeProvider.Now;
                await _persistence.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task RunDeletionAsync(AssetBulkDeletionJob job, CancellationToken cancellationToken)
        {
            string deletedBy = string.IsNullOrWhiteSpace(job.InitiatedByUserId) ? "system" : job.InitiatedByUserId;

            switch (job.Scope)
            {
                case BulkDeleteAssetsDtos.Scope.Batch:
                    await CleanupBatchUploadedFiles(job.BatchId!.Value).ConfigureAwait(false);
                    await DeleteAssetsInRepeatedChunks(job, q => q.Where(a =>
                        !a.IsDeleted && a.BatchId == job.BatchId!.Value), deletedBy,
                        cancellationToken).ConfigureAwait(false);
                    await SoftDeleteBatch(job.BatchId.Value, deletedBy).ConfigureAwait(false);
                    break;

                case BulkDeleteAssetsDtos.Scope.Depot:
                    await DeleteAssetsInRepeatedChunks(job, q =>
                        q.Where(a => !a.IsDeleted && a.DepotId == job.DepotId!.Value),
                        deletedBy, cancellationToken).ConfigureAwait(false);
                    break;

                case BulkDeleteAssetsDtos.Scope.ExplicitIds:
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
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var chunkQuery = filterAssets(_persistence.Assets)
                    .OrderBy(a => a.Id)
                    .Take(BulkDeleteAssetsDtos.UpdateChunkSize);

                var updated = await ApplySoftDeleteChunkFromQuery(chunkQuery, deletedBy, job, cancellationToken)
                    .ConfigureAwait(false);

                if (updated == 0)
                    break;
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

            for (var i = 0; i < orderedDistinct.Count; i += BulkDeleteAssetsDtos.ExplicitSliceSize)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var slice = orderedDistinct
                    .Skip(i)
                    .Take(BulkDeleteAssetsDtos.ExplicitSliceSize)
                    .ToList();

                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var chunkQuery = _persistence.Assets
                        .Where(a => slice.Contains(a.Id) && !a.IsDeleted)
                        .OrderBy(a => a.Id)
                        .Take(BulkDeleteAssetsDtos.UpdateChunkSize);

                    var updated = await ApplySoftDeleteChunkFromQuery(chunkQuery, deletedBy, job, cancellationToken)
                        .ConfigureAwait(false);

                    if (updated == 0)
                        break;
                }
            }
        }

        /// <summary>
        /// Runs one server-side UPDATE for the filtered ordered chunk (TOP N) and persists job counters
        /// in the same transaction. Avoids SELECT ids + batched IN lists; batch/depot scope uses BatchId/DepotId predicates only.
        /// </summary>
        private async Task<int> ApplySoftDeleteChunkFromQuery(
            IQueryable<Asset> chunkQuery,
            string deletedBy,
            AssetBulkDeletionJob job,
            CancellationToken cancellationToken)
        {
            await using var _ = await _transactionManager.BeginAsync(cancellationToken).ConfigureAwait(false);

            var delta = 0;
            var countersApplied = false;

            try
            {
                var now = _dateTimeProvider.Now;

                delta = await chunkQuery
                    .ExecuteUpdateAsync(s => s
                            .SetProperty(a => a.IsDeleted, true)
                            .SetProperty(a => a.DeletionDate, now)
                            .SetProperty(a => a.DeletedBy, deletedBy)
                            .SetProperty(a => a.ModificationDate, now)
                            .SetProperty(a => a.ModifiedBy, deletedBy),
                        cancellationToken)
                    .ConfigureAwait(false);

                job.ProcessedCount += delta;
                job.DeletedCount += delta;
                countersApplied = true;

                await PersistJobProgress(job, cancellationToken).ConfigureAwait(false);
                await _transactionManager.CommitAsync(cancellationToken).ConfigureAwait(false);
                return delta;
            }
            catch
            {
                await _transactionManager.RollbackAsync(cancellationToken).ConfigureAwait(false);
                if (countersApplied)
                {
                    job.ProcessedCount -= delta;
                    job.DeletedCount -= delta;
                }

                throw;
            }
        }

        private Task PersistJobProgress(AssetBulkDeletionJob job, CancellationToken ct)
            => _persistence.SaveChangesAsync(ct);
    }
}
