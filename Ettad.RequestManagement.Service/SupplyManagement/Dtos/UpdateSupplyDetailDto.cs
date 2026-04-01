namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class UpdateSupplyDetailDto
    {
        public long ItemId { get; set; }
        public string Lot { get; set; } = string.Empty;
        public long Quantity { get; set; }
        public string? Notes { get; set; }
    }
}

