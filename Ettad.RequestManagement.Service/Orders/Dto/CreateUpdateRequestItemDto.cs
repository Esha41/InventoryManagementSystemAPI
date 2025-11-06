namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class CreateUpdateRequestItemDto
    {
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public string Notes { get; set; }
    }
}

