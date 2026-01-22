using Ettad.CrossCutting.Comman.Base;
using System;

namespace Ettad.Data.Entities.Reports
{
    /// <summary>
    /// Entity for storing DevExpress report definitions in the database
    /// </summary>
    public class ReportEntity : FullAuditEntity<Guid>
    {
        /// <summary>
        /// Unique URL identifier for the report (used by DevExpress ReportStorage)
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Display name of the report
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Report description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Serialized report layout data (XML format from XtraReport.SaveLayoutToXml)
        /// </summary>
        public byte[]? LayoutData { get; set; }

        /// <summary>
        /// Report status: Draft, Published, Archived
        /// </summary>
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Whether the report is publicly accessible
        /// </summary>
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// Date when the report was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when the report was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
