namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class CreateSupplyDto
    {
        public long OrderId { get; set; }
        public List<CreateSupplyDetailDto> SupplyDetails { get; set; } = new();
    }

    public class CreateSupplyDetailDto
    {
        public long ItemId { get; set; }
        public string Lot { get; set; } = string.Empty;
        public long Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
