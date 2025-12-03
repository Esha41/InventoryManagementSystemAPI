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
        public string ItemNo { get; set; }
        public ItemType ItemType { get; set; }
        public string Nsn { get; set; }
        public string PartNo { get; set; }
        public long TotalQuantity { get; set; }
        public long UsedQuantity { get; set; }
        public long ReservedQuantityByOrdersOnProcessing { get; set; }
        public long RemainingQuantity { get; set; }
        public int TotalLots { get; set; }
    }
}

