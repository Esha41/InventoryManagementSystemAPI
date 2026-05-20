using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Inventories.Dtos
{
    /// <summary>
    /// Represents aggregated inventory quantities for a specific item across all lots
    /// </summary>
    public class ItemInventorySummaryDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        /// <summary>Arabic catalog name when set; UI falls back to <see cref="ItemName"/>.</summary>
        public string? ItemNameAr { get; set; }
        public string ItemNo { get; set; }
        public ItemType ItemType { get; set; }
        public string Nsn { get; set; }
        public string PartNo { get; set; }
        /// <summary>Lookup id for ammunition / weapon caliber; aligns with dashboard caliber filter.</summary>
        public long? CaliberId { get; set; }

        /// <summary>From catalog (ammunition / weapon); null for other types or when not set.</summary>
        public string Caliber { get; set; }
        /// <summary>Display name for caliber unit (e.g. weapon), when available.</summary>
        public string CaliberUnitName { get; set; }
        public long TotalQuantity { get; set; }
        public long UsedQuantity { get; set; }
        public long ReservedQuantityByOrdersOnProcessing { get; set; }
        public long RemainingQuantity { get; set; }
        public int TotalLots { get; set; }
    }
}

