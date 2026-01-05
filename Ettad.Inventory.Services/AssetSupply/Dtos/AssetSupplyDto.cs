using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// Response DTO for asset supply operations
    /// </summary>
    public class AssetSupplyDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public DateTime? SupplyDate { get; set; }
        public AssetSupplyStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public long? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public long? CustodianId { get; set; }
        public string? CustodianName { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverMilitaryId { get; set; }
        public long? ReceiverRankId { get; set; }
        public string? ReceiverRankName { get; set; }
        public string? Location { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedByUserId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }

        public List<AssetSupplyDetailDto> SupplyDetails { get; set; } = new();
    }
}

