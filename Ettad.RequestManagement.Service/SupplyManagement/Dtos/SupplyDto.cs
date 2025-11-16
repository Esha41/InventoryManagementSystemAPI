namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class SupplyDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime SupplyDate { get; set; }
        public string SuppliedBy { get; set; }
        public List<SupplyItemDto> SupplyItems { get; set; } = new List<SupplyItemDto>();
    }

    public class SupplyItemDto
    {
        public long Id { get; set; }
        public long SupplyId { get; set; }
        public long RequestItemId { get; set; }
        public long InventoryDetailId { get; set; }
        public long Quantity { get; set; }
        public string ItemName { get; set; }
        public int Lot { get; set; }
    }
}

