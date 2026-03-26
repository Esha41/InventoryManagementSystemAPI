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
        public List<WorkflowSupplySummaryLineDto> Lines { get; set; } = new();
    }

    public class WorkflowSupplySummaryLineDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ItemNo { get; set; }
        public long RequestedQuantity { get; set; }
        public long SuppliedQuantity { get; set; }
        public string Lot { get; set; } = string.Empty;
        public long? DepotId { get; set; }
        public string? DepotName { get; set; }
        public string? DepotCode { get; set; }
        public string? Notes { get; set; }
    }
}
