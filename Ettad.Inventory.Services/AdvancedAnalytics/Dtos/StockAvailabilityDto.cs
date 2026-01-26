namespace Ettad.Inventory.Service.AdvancedAnalytics.Dtos
{
    /// <summary>
    /// Stock Availability KPI DTO
    /// </summary>
    public class StockAvailabilityDto
    {
        /// <summary>
        /// Overall stock availability percentage
        /// </summary>
        public decimal AvailabilityRate { get; set; }

        /// <summary>
        /// Total required items
        /// </summary>
        public int TotalRequiredItems { get; set; }

        /// <summary>
        /// Total available items
        /// </summary>
        public int TotalAvailableItems { get; set; }

        /// <summary>
        /// Breakdown by warehouse
        /// </summary>
        public List<StockAvailabilityByWarehouseDto> ByWarehouse { get; set; } = new();

        /// <summary>
        /// Breakdown by item category
        /// </summary>
        public List<StockAvailabilityByCategoryDto> ByCategory { get; set; } = new();

        /// <summary>
        /// Low stock areas (for heat map)
        /// </summary>
        public List<LowStockAreaDto> LowStockAreas { get; set; } = new();
    }

    /// <summary>
    /// Stock Availability by Warehouse
    /// </summary>
    public class StockAvailabilityByWarehouseDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public int RequiredItems { get; set; }
        public int AvailableItems { get; set; }
        public decimal AvailabilityRate { get; set; }
    }

    /// <summary>
    /// Stock Availability by Category
    /// </summary>
    public class StockAvailabilityByCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public bool IsCritical { get; set; }
        public int RequiredItems { get; set; }
        public int AvailableItems { get; set; }
        public decimal AvailabilityRate { get; set; }
    }

    /// <summary>
    /// Low Stock Area for Heat Map
    /// </summary>
    public class LowStockAreaDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int LowStockItemCount { get; set; }
        public decimal Severity { get; set; } // 0-1 scale
    }
}
