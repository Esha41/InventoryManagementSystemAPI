namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class UpdateSupplyDetailDto
    {
        public long ItemId { get; set; }
        public int Lot { get; set; }
        public long Quantity { get; set; }
        public string? Notes { get; set; }
    }
}

