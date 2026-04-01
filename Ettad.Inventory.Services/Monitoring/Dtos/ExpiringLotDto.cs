using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    public class ExpiringLotDto
    {
        public long InventoryDetailId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string? ItemNo { get; set; }
        public string Lot { get; set; } = string.Empty;
        public string? BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? DaysUntilExpiry { get; set; }
        public long RemainingQuantity { get; set; }
        public DepotDto? Depot { get; set; }
        public SupplierDto? Supplier { get; set; }
        public ManufacturerDto? Manufacturer { get; set; }
    }
}
