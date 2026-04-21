using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;
using System;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Entity for storing DevExpress report definitions in the database
    /// </summary>
    public class ReportEntity : FullAuditEntity<Guid>
    {
        /// <summary>
        /// Report name
        /// </summary>
        public string ReportName { get; set; } = string.Empty;

        /// <summary>
        /// Report status enum (stored as int, no FK).
        /// </summary>
        public ReportStatuses ReportStatusId { get; set; }

        /// <summary>
        /// Unique URL identifier for the report (used by DevExpress ReportStorage)
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Report description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Serialized report layout data (XML format from XtraReport.SaveLayoutToXml)
        /// This is the main report definition that can be loaded into DevExpress Report Designer
        /// </summary>
        public byte[] LayoutData { get; set; }

        /// <summary>
        /// Report parameters configuration in JSON format
        /// Contains parameter definitions, default values, and validation rules
        /// </summary>
        public string ReportParameters { get; set; }

    }
}
