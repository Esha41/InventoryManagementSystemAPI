using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Batches.Dtos
{
    /// <summary>
    /// One row from batch asset Excel import/export (parsed via Excel import service).
    /// </summary>
    public class BatchAssetExcelImportRowDto
    {
        public int RowNumber { get; set; }

        /// <summary>Set during import validation when the row creates a new asset in the batch (no matching _AssetId or in-batch serial).</summary>
        public bool IsNewRow { get; set; }

        public long AssetId { get; set; }

        public long ItemId { get; set; }

        public string? ItemName { get; set; }

        public string? ItemNo { get; set; }

        public string? SerialNumber { get; set; }

        public string? RFID { get; set; }

        /// <summary>Parsed from Excel after resolution; may be set from <see cref="StatusLabel"/>.</summary>
        public AssetStatus? Status { get; set; }

        /// <summary>Operational status as shown in the Excel dropdown (mapped from Status column).</summary>
        public string? StatusLabel { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiryDate { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string? DeliveryReceipt { get; set; }

        public string? Notes { get; set; }

        /// <summary>Raw label from the "Assignment Mode" Excel dropdown (parsed during enrichment).</summary>
        public string? AssignmentModeLabel { get; set; }

        /// <summary>Resolved during enrichment from <see cref="AssignmentModeLabel"/>.</summary>
        public BatchAssignmentMode AssignmentMode { get; set; } = BatchAssignmentMode.NoChange;

        /// <summary>True when assignment fields should be processed (any mode except NoChange).</summary>
        public bool UpdateAssignment => AssignmentMode != BatchAssignmentMode.NoChange;

        /// <summary>True when the user explicitly wants to remove assignment.</summary>
        public bool RemoveAssignment => AssignmentMode == BatchAssignmentMode.RemoveAssignment;

        /// <summary>Department display from Excel dropdown (resolved to <see cref="AssignToDepartmentId"/>).</summary>
        public string? AssignmentDepartment { get; set; }

        /// <summary>Employee display from Excel dropdown (resolved to <see cref="AssignToEmployeeId"/>).</summary>
        public string? AssignmentEmployee { get; set; }

        public long? AssignToDepartmentId { get; set; }

        public long? AssignToEmployeeId { get; set; }

        public string? AssignmentNotes { get; set; }
    }
}
