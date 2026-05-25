using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class WorkflowSupplySummaryDto
    {
        public long OrderId { get; set; }
        public DateTime? OrderSupplyDate { get; set; }
        public DateTime? SupplyDate { get; set; }
        public SupplySubmissionStatus SubmissionStatus { get; set; }
        public SupplyFulfillmentStatus FulfillmentStatus { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverMilitaryId { get; set; }
        public string? ReceiverRankName { get; set; }
        public string? Notes { get; set; }
        public bool IsWeaponOrder { get; set; }
        /// <summary>True when the order is in final approved/completed status; false while workflow is still in progress.</summary>
        public bool IsOrderCompleted { get; set; }
        /// <summary>Current phase: Selection (depot/batch chosen), Supplied (assets submitted), or None.</summary>
        public string Phase { get; set; } = "None";
        public List<WorkflowSupplySummaryLineDto> Lines { get; set; } = new();
        public List<WeaponSelectionLineDto> SelectionLines { get; set; } = new();
        public List<WeaponSuppliedLineDto> WeaponLines { get; set; } = new();
        /// <summary>Supply submission files (receiver signature and supporting attachments).</summary>
        public List<FileUploadDto> Files { get; set; } = new();
    }

    public class WorkflowSupplySummaryLineDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ItemNo { get; set; }
        public long RequestedQuantity { get; set; }
        /// <summary>Quantity approved on the order (may be lower than originally requested).</summary>
        public long ApprovedQuantity { get; set; }
        public long SuppliedQuantity { get; set; }
        public string Lot { get; set; } = string.Empty;
        public long? DepotId { get; set; }
        public string? DepotName { get; set; }
        public string? DepotNameEn { get; set; }
        public string? DepotNameAr { get; set; }
        public string? DepotCode { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>Depot/batch selection phase for weapon orders (before asset supply is created).</summary>
    public class WeaponSelectionLineDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public long DepotId { get; set; }
        public string? DepotName { get; set; }
        public string? DepotNameEn { get; set; }
        public string? DepotNameAr { get; set; }
        public string? DepotCode { get; set; }
        public long BatchId { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public int SelectedQuantity { get; set; }
    }

    /// <summary>Weapon supply submitted: one row per individual asset.</summary>
    public class WeaponSuppliedLineDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public long AssetId { get; set; }
        public string? SerialNumber { get; set; }
        public long? DepotId { get; set; }
        public string? DepotName { get; set; }
        public string? DepotNameEn { get; set; }
        public string? DepotNameAr { get; set; }
        public string? DepotCode { get; set; }
        public string? BatchNumber { get; set; }
        public string? AssigneeName { get; set; }
        public string? Notes { get; set; }
    }
}
