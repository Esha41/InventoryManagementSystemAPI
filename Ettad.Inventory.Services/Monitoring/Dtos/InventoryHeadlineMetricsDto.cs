namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    /// <summary>
    /// Compact headline metrics for the inventory dashboard stat cards (totals, by-type, alerts).
    /// </summary>
    public class InventoryHeadlineMetricsDto
    {
        public int LowStockCount { get; set; }
        public int ExpiringSoonCount { get; set; }
        public int TotalDistinctItems { get; set; }
        public long TotalRemainingQuantity { get; set; }
        public long TotalLots { get; set; }
        public int AmmunitionItemCount { get; set; }
        public int ExplosiveItemCount { get; set; }
        public int AccessoryItemCount { get; set; }
        public int WeaponItemGroupsCount { get; set; }
    }
}
