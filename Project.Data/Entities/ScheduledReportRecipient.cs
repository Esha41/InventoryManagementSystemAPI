using Ettad.CrossCutting.Comman.Base;
using System;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Entity for storing scheduled report recipients (users or email addresses)
    /// </summary>
    public class ScheduledReportRecipient : FullAuditEntity<Guid>
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
        /// User ID if recipient is a system user (nullable)
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Email address (only used if UserId is null for manual email entries)
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Recipient type: To, CC, BCC
        /// </summary>
        public string RecipientType { get; set; } = "To"; // To, CC, BCC
    }
}
