using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Inventories.Dtos
{
    /// <summary>Lightweight SQL aggregates for dashboard headline metrics (no per-item DTO materialization).</summary>
    public class InventoryHeadlineInventoryAggregatesDto
    {
        public int AmmunitionItemCount { get; set; }
        public int ExplosiveItemCount { get; set; }
        public int AccessoryItemCount { get; set; }
        public int TotalNonWeaponDistinctItems { get; set; }
        public long TotalRemainingQuantity { get; set; }
        public long TotalLots { get; set; }
    }
}
