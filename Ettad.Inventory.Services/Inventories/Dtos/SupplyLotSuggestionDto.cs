using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Inventories.Dtos
{
    /// <summary>
    /// Represents a single lot suggestion for supplying an item
    /// </summary>
    public class SupplyLotSuggestionDto
    {
        public long InventoryDetailId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public int Lot { get; set; }
        public long AvailableQuantity { get; set; }
        public long SuggestedQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public long InventoryId { get; set; }
        public DepotDto? Depot { get; set; }
        public SupplierDto? Supplier { get; set; }
        public ManufacturerDto? Manufacturer { get; set; }
    }
}

