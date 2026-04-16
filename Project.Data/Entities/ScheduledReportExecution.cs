using Ettad.CrossCutting.Comman.Base;
using System;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Entity for storing scheduled report execution history
    /// </summary>
    public class ScheduledReportExecution : FullAuditEntity<Guid>
    {
        /// <summary>
        /// Scheduled Report ID (foreign key)
        /// </summary>
        public Guid ScheduledReportId { get; set; }

        /// <summary>
        /// Navigation property for scheduled report
        /// </summary>
        public virtual ScheduledReport ScheduledReport { get; set; }

        /// <summary>
        /// Execution start date/time
        /// </summary>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Execution status: Success, Failed
        /// </summary>
        public string Status { get; set; } = "Success";

        /// <summary>
        /// Error message if execution failed
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Number of recipients the report was sent to
        /// </summary>
        public int RecipientCount { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long? FileSizeBytes { get; set; }
    }
}
