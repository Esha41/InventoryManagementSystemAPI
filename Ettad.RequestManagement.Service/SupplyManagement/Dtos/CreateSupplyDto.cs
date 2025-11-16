namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class CreateSupplyDto
    {
        public long OrderId { get; set; }
        public List<CreateSupplyItemDto> SupplyItems { get; set; } = new List<CreateSupplyItemDto>();
    }

    public class CreateSupplyItemDto
    {
        public long RequestItemId { get; set; }
        public long InventoryDetailId { get; set; }
        public long Quantity { get; set; }
    }
}

