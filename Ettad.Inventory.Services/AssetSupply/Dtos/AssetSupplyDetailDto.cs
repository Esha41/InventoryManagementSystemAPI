using Ettad.User.Services.DTO;

namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// Response DTO for asset supply detail
    /// </summary>
    public class AssetSupplyDetailDto
    {
        public long Id { get; set; }
        public long AssetSupplyId { get; set; }
        public long AssetId { get; set; }
        public string? AssetSerialNumber { get; set; }
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public int SequenceNo { get; set; }
        public string? ConditionOnSupply { get; set; }
        public string? Notes { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string? CustodianId { get; set; }
        public UserDto? Custodian { get; set; }
    }
}

