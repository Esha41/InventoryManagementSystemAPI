using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Batches.Dtos
{
    public class BulkUpdateBatchAssetsDto
    {
        public List<BatchAssetUpdateItem> Items { get; set; } = new();
    }

    public class BatchAssetUpdateItem
    {
        public long AssetId { get; set; }

        public long ItemId { get; set; }

        public string? SerialNumber { get; set; }

        public string? RFID { get; set; }

        public AssetStatus? Status { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiryDate { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string? DeliveryReceipt { get; set; }

        public string? Notes { get; set; }

        /// <summary>
        /// When true, applies <see cref="AssignToDepartmentId"/>, <see cref="AssignToEmployeeId"/>, and clears assignment if both are null.
        /// When false, existing assignment is left unchanged.
        /// </summary>
        public bool UpdateAssignment { get; set; }

        /// <summary>Department-only assignment. Omit when <see cref="AssignToEmployeeId"/> is set.</summary>
        public long? AssignToDepartmentId { get; set; }

        /// <summary>Employee assignment; uses the employee's department. Omit when using department-only assignment.</summary>
        public long? AssignToEmployeeId { get; set; }

        public string? AssignmentNotes { get; set; }
    }
}
