namespace Ettad.Inventory.Service.Inventories.Dtos
{
    /// <summary>
    /// Represents aggregated inventory quantities for a specific item across all lots
    /// </summary>
    public class ItemInventorySummaryDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public long TotalQuantity { get; set; }
        public long UsedQuantity { get; set; }
        public long ReservedQuantityByOrdersOnProcessing { get; set; }
        public long RemainingQuantity { get; set; }
        public int TotalLots { get; set; }
    }
}

