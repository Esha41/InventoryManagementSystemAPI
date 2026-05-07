using System;
using System.Collections.Generic;

namespace Ettad.Inventory.Service.Assets.Dtos
{
    /// <summary>Scopes for async bulk soft-delete of tracked assets.</summary>
    public enum AssetBulkDeletionScopeDto
    {
        Batch = 0,
        Depot = 1,
        ExplicitIds = 2
    }

    public class StartBulkDeleteAssetsDto
    {
        public AssetBulkDeletionScopeDto Scope { get; set; }

        public long? BatchId { get; set; }

        public long? DepotId { get; set; }

        /// <summary>Omit or null except when <see cref="AssetBulkDeletionScopeDto.ExplicitIds"/>.</summary>
        public List<long>? AssetIds { get; set; }
    }

    public class BulkDeleteAssetsEnqueueResultDto
    {
        public string JobId { get; set; }

        public string HangfireJobId { get; set; }

        public int TotalCandidates { get; set; }
    }

    public class BulkDeleteAssetsStatusDto
    {
        public string JobId { get; set; }

        public string Status { get; set; }

        public int TotalCandidates { get; set; }

        public int ProcessedCount { get; set; }

        public int DeletedCount { get; set; }

        public double ProgressPercent { get; set; }

        public string Message { get; set; }

        public DateTime? StartedUtc { get; set; }

        public DateTime? CompletedUtc { get; set; }
    }
}
