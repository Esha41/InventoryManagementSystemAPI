namespace Ettad.Inventory.Service.AdvancedAnalytics.Dtos
{
    /// <summary>
    /// Consumption Rate vs Forecast KPI DTO
    /// </summary>
    public class ConsumptionForecastDto
    {
        /// <summary>
        /// Overall consumption ratio (Actual / Forecast)
        /// </summary>
        public decimal ConsumptionRatio { get; set; }

        /// <summary>
        /// Overall variance percentage
        /// </summary>
        public decimal VariancePercentage { get; set; }

        /// <summary>
        /// Trend data comparing actual vs forecast over time
        /// </summary>
        public List<ConsumptionForecastTrendDto> TrendData { get; set; } = new();

        /// <summary>
        /// Variance breakdown by item category
        /// </summary>
        public List<ConsumptionForecastByCategoryDto> ByCategory { get; set; } = new();

        /// <summary>
        /// Items with significant variance (>10% or <-10%)
        /// </summary>
        public List<ConsumptionForecastAlertDto> Alerts { get; set; } = new();
    }

    /// <summary>
    /// Consumption Forecast Trend Data Point
    /// </summary>
    public class ConsumptionForecastTrendDto
    {
        public DateTime Date { get; set; }
        public decimal ActualConsumption { get; set; }
        public decimal ForecastedConsumption { get; set; }
        public decimal Variance { get; set; }
        public decimal VariancePercentage { get; set; }
    }

    /// <summary>
    /// Consumption Forecast by Category
    /// </summary>
    public class ConsumptionForecastByCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal ActualConsumption { get; set; }
        public decimal ForecastedConsumption { get; set; }
        public decimal Variance { get; set; }
        public decimal VariancePercentage { get; set; }
    }

    /// <summary>
    /// Consumption Forecast Alert (Over/Under consumption)
    /// </summary>
    public class ConsumptionForecastAlertDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal ActualConsumption { get; set; }
        public decimal ForecastedConsumption { get; set; }
        public decimal VariancePercentage { get; set; }
        public string AlertType { get; set; } = string.Empty; // "OverConsumption" or "UnderUtilization"
    }
}
