namespace Ettad.Inventory.Service.AdvancedAnalytics.Dtos
{
    /// <summary>
    /// Complete Advanced Analytics Dashboard DTO
    /// </summary>
    public class AdvancedAnalyticsDashboardDto
    {
        /// <summary>
        /// Mission Readiness Rate KPI
        /// </summary>
        public MissionReadinessDto MissionReadiness { get; set; } = new();

        /// <summary>
        /// Stock Availability KPI
        /// </summary>
        public StockAvailabilityDto StockAvailability { get; set; } = new();

        /// <summary>
        /// Order Cycle Time KPI
        /// </summary>
        public OrderCycleTimeDto OrderCycleTime { get; set; } = new();

        /// <summary>
        /// Consumption Rate vs Forecast KPI
        /// </summary>
        public ConsumptionForecastDto ConsumptionForecast { get; set; } = new();

        /// <summary>
        /// Order Status Distribution Chart
        /// </summary>
        public OrderStatusDistributionDto OrderStatusDistribution { get; set; } = new();

        /// <summary>
        /// Request Trends Over Time
        /// </summary>
        public RequestTrendsDto RequestTrends { get; set; } = new();

        /// <summary>
        /// Inventory Value by Warehouse
        /// </summary>
        public InventoryValueDto InventoryValue { get; set; } = new();

        /// <summary>
        /// Asset Assignment Status
        /// </summary>
        public AssetAssignmentStatusDto AssetAssignmentStatus { get; set; } = new();

        /// <summary>
        /// Supply Fulfillment Status
        /// </summary>
        public SupplyFulfillmentStatusDto SupplyFulfillmentStatus { get; set; } = new();

        /// <summary>
        /// Department Request Volume
        /// </summary>
        public DepartmentRequestVolumeDto DepartmentRequestVolume { get; set; } = new();

        /// <summary>
        /// Notification Statistics
        /// </summary>
        public NotificationStatisticsDto NotificationStatistics { get; set; } = new();

        /// <summary>
        /// Import/Export Statistics
        /// </summary>
        public ImportExportStatisticsDto ImportExportStatistics { get; set; } = new();

        /// <summary>
        /// Last updated timestamp
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// Drill-down request parameters
    /// </summary>
    public class DrillDownRequestDto
    {
        public string KpiType { get; set; } = string.Empty; // "MissionReadiness", "StockAvailability", etc.
        public string Level { get; set; } = string.Empty; // "Category", "Item", "Transaction"
        public int? WarehouseId { get; set; }
        public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
