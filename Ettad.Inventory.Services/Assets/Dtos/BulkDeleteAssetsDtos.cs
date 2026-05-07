using System;
using System.Collections.Generic;

namespace Ettad.Inventory.Service.Assets.Dtos
{
    /// <summary>Constants for async bulk-delete (API payloads and persisted job execution).</summary>
    public static class BulkDeleteAssetsDtos
    {
        public static class JobStatus
        {
            public const byte Pending = 0;
            public const byte Running = 1;
            public const byte Completed = 2;
            public const byte Failed = 3;
        }

        /// <summary>Matches <see cref="AssetBulkDeletionScopeDto"/> ordinal values stored on DB job rows.</summary>
        public static class Scope
        {
            public const byte Batch = 0;
            public const byte Depot = 1;
            public const byte ExplicitIds = 2;
        }

        /// <summary>LINQ Contains batch size staying under SQL Server parameter limits.</summary>
        public const int ExplicitSliceSize = 2000;

        /// <summary>Max rows soft-deleted per transaction (single server-side UPDATE per chunk).</summary>
        public const int UpdateChunkSize = 50_000;
    }

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
