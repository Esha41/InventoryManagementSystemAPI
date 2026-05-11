namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    /// <summary>
    /// Compact headline metrics for the inventory dashboard stat cards (totals, by-type, alerts).
    /// </summary>
    public class InventoryHeadlineMetricsDto
    {
        public long LowStockCount { get; set; }
        public long CriticalStockCount { get; set; }
        public long ExpiringSoonCount { get; set; }
        public long TotalDistinctItems { get; set; }
        public long TotalRemainingQuantity { get; set; }
        public long TotalLots { get; set; }
        public long AmmunitionItemCount { get; set; }
        public long ExplosiveItemCount { get; set; }
        public long AccessoryItemCount { get; set; }
        public long WeaponItemGroupsCount { get; set; }

        public long LotCount { get; set; }
        public long WeaponCount { get; set; }

        /// <summary>Weapon registry batches in scope (non-deleted <see cref="Batch"/> rows, depot-filtered like lots).</summary>
        public long TotalBatches { get; set; }
    }
}
