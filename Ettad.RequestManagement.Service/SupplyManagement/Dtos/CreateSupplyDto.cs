namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class CreateSupplyDto
    {
        public long OrderId { get; set; }
        public DateTime? SupplyDate { get; set; }
        public string? RecieverName { get; set; }
        public long? ReceiverRankId { get; set; }
        public string? RecieverMilitaryId { get; set; }
        public string? Notes { get; set; }
        public List<CreateSupplyDetailDto> SupplyDetails { get; set; } = new();
    }

    public class CreateSupplyDetailDto
    {
        public long ItemId { get; set; }
        public int Lot { get; set; }
        public long Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
