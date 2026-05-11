namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    public class CriticalStockItemDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ItemNo { get; set; }
        public string? Nsn { get; set; }
        public long? CriticalQuantity { get; set; }
        public long TotalStock { get; set; }
        public long HoldQuantity { get; set; }
        public long SuppliedQuantity { get; set; }
        public long Remaining { get; set; }
    }
}
