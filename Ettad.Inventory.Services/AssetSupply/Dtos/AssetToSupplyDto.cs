using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// DTO representing assets available for supply grouped by item type
    /// </summary>
    public class OrderAssetsToSupplyDto
    {
        /// <summary>
        /// The order ID
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Order request number
        /// </summary>
        public string? RequestNo { get; set; }

        /// <summary>
        /// Department name from the order
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Items with their requested quantities and available assets
        /// </summary>
        public List<ItemAssetsToSupplyDto> Items { get; set; } = new();

        /// <summary>
        /// Whether all requested quantities can be fulfilled
        /// </summary>
        public bool CanFullyFulfill => Items.All(i => i.AvailableQuantity >= i.RequestedQuantity);
    }

    /// <summary>
    /// DTO for an item type and its available assets
    /// </summary>
    public class ItemAssetsToSupplyDto
    {
        /// <summary>
        /// The item type ID
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Item name
        /// </summary>
        public string? ItemName { get; set; }

        /// <summary>
        /// Quantity requested in the order
        /// </summary>
        public long RequestedQuantity { get; set; }

        /// <summary>
        /// Number of available assets for supply
        /// </summary>
        public int AvailableQuantity => AvailableAssets?.Count ?? 0;

        /// <summary>
        /// Whether this item can be fully fulfilled
        /// </summary>
        public bool CanFulfill => AvailableQuantity >= RequestedQuantity;

        /// <summary>
        /// Available assets for this item type (ordered by FIFO, serial number required)
        /// </summary>
        public List<AssetToSupplyDto> AvailableAssets { get; set; } = new();
    }

    /// <summary>
    /// DTO representing a single asset available for supply
    /// </summary>
    public class AssetToSupplyDto
    {
        /// <summary>
        /// Asset ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Serial number (required for supply)
        /// </summary>
        public string SerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// RFID tag if available
        /// </summary>
        public string? RFID { get; set; }

        /// <summary>
        /// Asset tag/barcode
        /// </summary>
        public string? AssetTag { get; set; }

        /// <summary>
        /// Current condition of the asset
        /// </summary>
        public string? Condition { get; set; }

        /// <summary>
        /// Current status of the asset
        /// </summary>
        public AssetStatus? Status { get; set; }

        /// <summary>
        /// Status name for display
        /// </summary>
        public string? StatusName => Status?.ToString();

        /// <summary>
        /// Date of purchase (used for FIFO ordering)
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Depot where asset is located
        /// </summary>
        public long DepotId { get; set; }

        /// <summary>
        /// Depot name
        /// </summary>
        public string? DepotName { get; set; }

        /// <summary>
        /// Priority order for supply (1 = highest priority based on FIFO)
        /// </summary>
        public int Priority { get; set; }
    }
}

