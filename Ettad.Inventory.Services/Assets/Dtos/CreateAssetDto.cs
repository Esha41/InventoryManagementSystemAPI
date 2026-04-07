using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Assets.Dtos
{
    public class CreateAssetDto
    {
        public long ItemId { get; set; }

        public string BatchNumber { get; set; } = string.Empty;

        /// <summary>Optional: set on the batch when the batch is first created (must be allowed for <see cref="ItemId"/>).</summary>
        public long? BatchPrimaryPurposId { get; set; }

        public string? SerialNumber { get; set; }

        public string? RFID { get; set; }

        public long DepotId { get; set; }

        public string? AssetTag { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiryDate { get; set; }

        public string? Condition { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string? DeliveryReceipt { get; set; }

        public string? Notes { get; set; }

        /// <summary>Optional: assign to this employee on intake (uses the employee's department only; do not send <see cref="AssignToDepartmentId"/>).</summary>
        public long? AssignToEmployeeId { get; set; }

        /// <summary>Optional: department-only intake assignment. Omit when <see cref="AssignToEmployeeId"/> is set.</summary>
        public long? AssignToDepartmentId { get; set; }

        /// <summary>Optional notes stored on the asset assignment when intake assignment is created.</summary>
        public string? AssignmentNotes { get; set; }
    }
}

