using Ettad.CrossCutting.Comman.Base;
using System;
using System.Collections.Generic;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Entity for storing scheduled report configurations
    /// </summary>
    public class ScheduledReport : FullAuditEntity<Guid>
    {
        /// <summary>
        /// Name of the scheduled report
        /// </summary>
        public string ScheduleName { get; set; } = string.Empty;

        /// <summary>
        /// Report ID (foreign key to ReportEntity)
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// Navigation property for report
        /// </summary>
        public virtual ReportEntity Report { get; set; }

        /// <summary>
        /// Output format: PDF, Excel, etc.
        /// </summary>
        public string OutputFormat { get; set; } = "PDF";

        /// <summary>
        /// Schedule frequency: Daily, Weekly, Monthly
        /// </summary>
        public string Frequency { get; set; } = "Daily";

        /// <summary>
        /// Time of day to run (HH:mm format, e.g., "08:00")
        /// </summary>
        public string TimeOfDay { get; set; } = "08:00";

        /// <summary>
        /// Day of week for weekly schedules (0=Sunday, 1=Monday, ..., 6=Saturday)
        /// </summary>
        public int? DayOfWeek { get; set; }

        /// <summary>
        /// Day of month for monthly schedules (1-31)
        /// </summary>
        public int? DayOfMonth { get; set; }

        /// <summary>
        /// Next scheduled run date/time
        /// </summary>
        public DateTime? NextRunDate { get; set; }

        /// <summary>
        /// Last run date/time
        /// </summary>
        public DateTime? LastRunDate { get; set; }

        /// <summary>
        /// Status: Active, Inactive
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Email subject template
        /// </summary>
        public string? EmailSubject { get; set; }

        /// <summary>
        /// Email body template
        /// </summary>
        public string? EmailBody { get; set; }

        /// <summary>
        /// Navigation property for scheduled report recipients
        /// </summary>
        public virtual ICollection<ScheduledReportRecipient> Recipients { get; set; } = new List<ScheduledReportRecipient>();

        /// <summary>
        /// Navigation property for scheduled report execution history
        /// </summary>
        public virtual ICollection<ScheduledReportExecution> Executions { get; set; } = new List<ScheduledReportExecution>();
    }
}
