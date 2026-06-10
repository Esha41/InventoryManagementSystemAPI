using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.AssetHistory.Dtos
{
    public class AssetHistoryDto
    {
        public long Id { get; set; }
        public long AssetId { get; set; }
        public string? AssetSerialNumber { get; set; }
        public string? BatchNumber { get; set; }

        public AssetHistoryActionType ActionType { get; set; }
        public string ActionTypeName => ActionType.ToString();
        public DateTime ActionDate { get; set; }

        public AssetStatus? PreviousStatus { get; set; }
        public AssetStatus? NewStatus { get; set; }

        public long? PreviousDepartmentId { get; set; }
        public string? PreviousDepartmentName { get; set; }
        public string? PreviousDepartmentNameAr { get; set; }
        public long? NewDepartmentId { get; set; }
        public string? NewDepartmentName { get; set; }
        public string? NewDepartmentNameAr { get; set; }

        public long? PreviousCustodianId { get; set; }
        public string? PreviousCustodianName { get; set; }
        public string? PreviousCustodianNameAr { get; set; }
        public long? NewCustodianId { get; set; }
        public string? NewCustodianName { get; set; }
        public string? NewCustodianNameAr { get; set; }

        public string? PreviousLocation { get; set; }
        public string? NewLocation { get; set; }

        public long? OrderId { get; set; }
        public string? OrderRequestNo { get; set; }
        public long? AssetSupplyId { get; set; }
        public long? AssetAssignmentId { get; set; }

        public string? PerformedByUserId { get; set; }
        public string? PerformedByUserName { get; set; }
        public string? Notes { get; set; }
        public string? Metadata { get; set; }

        public DateTime CreationDate { get; set; }
    }

    public class AssetHistoryContext
    {
        public AssetStatus? PreviousStatus { get; set; }
        public AssetStatus? NewStatus { get; set; }
        public long? PreviousDepartmentId { get; set; }
        public long? NewDepartmentId { get; set; }
        public long? PreviousCustodianId { get; set; }
        public long? NewCustodianId { get; set; }
        public string? PreviousLocation { get; set; }
        public string? NewLocation { get; set; }
        public long? OrderId { get; set; }
        public long? AssetSupplyId { get; set; }
        public long? AssetAssignmentId { get; set; }
        public string? Notes { get; set; }
        public string? Metadata { get; set; }
    }
}
