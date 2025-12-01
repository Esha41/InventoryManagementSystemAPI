using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Inventories.Dtos
{
    /// <summary>
    /// Represents detailed information about a specific lot for an item
    /// </summary>
    public class LotDetailDto
    {
        public long InventoryDetailId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public int Lot { get; set; }
        public long OriginalQuantity { get; set; }
        public long UsedQuantity { get; set; }
        public long ReservedQuantityByOrdersOnProcessing { get; set; }
        public long RemainingQuantity { get; set; }
        public bool IsEmptyLot { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public long InventoryId { get; set; }
        public DepotDto? Depot { get; set; }
        public SupplierDto? Supplier { get; set; }
        public ManufacturerDto? Manufacturer { get; set; }
        public CountryDto? Country { get; set; }
    }
}
