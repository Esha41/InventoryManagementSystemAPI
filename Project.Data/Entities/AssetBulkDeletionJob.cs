namespace Ettad.Data.Entities
{
    /// <summary>Persisted bulk asset soft-delete job (async Hangfire).</summary>
    public class AssetBulkDeletionJob
    {
        public Guid Id { get; set; }

        public string HangfireJobId { get; set; }

        public string InitiatedByUserId { get; set; }

        /// <summary>0 Pending, 1 Running, 2 Completed, 3 Failed.</summary>
        public byte JobStatus { get; set; }

        /// <summary>0 Batch, 1 Depot, 2 Explicit asset ids.</summary>
        public byte Scope { get; set; }

        public long? BatchId { get; set; }

        public long? DepotId { get; set; }

        /// <summary>JSON array of longs when Scope = ExplicitIds.</summary>
        public string ExplicitAssetIdsJson { get; set; }

        public int TotalCandidates { get; set; }

        public int ProcessedCount { get; set; }

        public int DeletedCount { get; set; }

        public string Message { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime? StartedUtc { get; set; }

        public DateTime? CompletedUtc { get; set; }
    }
}
