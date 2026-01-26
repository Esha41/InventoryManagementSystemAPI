namespace Ettad.Inventory.Service.AdvancedAnalytics.Dtos
{
    /// <summary>
    /// Order Cycle Time KPI DTO
    /// </summary>
    public class OrderCycleTimeDto
    {
        /// <summary>
        /// Average order cycle time in days
        /// </summary>
        public decimal AverageCycleTimeDays { get; set; }

        /// <summary>
        /// Median cycle time in days
        /// </summary>
        public decimal MedianCycleTimeDays { get; set; }

        /// <summary>
        /// Minimum cycle time in days
        /// </summary>
        public decimal MinCycleTimeDays { get; set; }

        /// <summary>
        /// Maximum cycle time in days
        /// </summary>
        public decimal MaxCycleTimeDays { get; set; }

        /// <summary>
        /// Trend data over time
        /// </summary>
        public List<OrderCycleTimeTrendDto> TrendData { get; set; } = new();

        /// <summary>
        /// Breakdown by component (approval, procurement, shipping, receiving)
        /// </summary>
        public OrderCycleTimeBreakdownDto Breakdown { get; set; } = new();

        /// <summary>
        /// Delays by supplier
        /// </summary>
        public List<OrderCycleTimeBySupplierDto> BySupplier { get; set; } = new();

        /// <summary>
        /// Delays by item type
        /// </summary>
        public List<OrderCycleTimeByItemTypeDto> ByItemType { get; set; } = new();

        /// <summary>
        /// Outliers and delayed orders
        /// </summary>
        public List<OrderCycleTimeOutlierDto> Outliers { get; set; } = new();
    }

    /// <summary>
    /// Order Cycle Time Trend Data Point
    /// </summary>
    public class OrderCycleTimeTrendDto
    {
        public DateTime Date { get; set; }
        public decimal AverageCycleTimeDays { get; set; }
        public int OrderCount { get; set; }
    }

    /// <summary>
    /// Order Cycle Time Breakdown by Component
    /// </summary>
    public class OrderCycleTimeBreakdownDto
    {
        public decimal AverageApprovalTimeDays { get; set; }
        public decimal AverageProcurementTimeDays { get; set; }
        public decimal AverageShippingTimeDays { get; set; }
        public decimal AverageReceivingTimeDays { get; set; }
    }

    /// <summary>
    /// Order Cycle Time by Supplier
    /// </summary>
    public class OrderCycleTimeBySupplierDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal AverageCycleTimeDays { get; set; }
        public int OrderCount { get; set; }
        public int DelayedOrderCount { get; set; }
    }

    /// <summary>
    /// Order Cycle Time by Item Type
    /// </summary>
    public class OrderCycleTimeByItemTypeDto
    {
        public string ItemType { get; set; } = string.Empty;
        public decimal AverageCycleTimeDays { get; set; }
        public int OrderCount { get; set; }
    }

    /// <summary>
    /// Order Cycle Time Outlier
    /// </summary>
    public class OrderCycleTimeOutlierDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal CycleTimeDays { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DelayReason { get; set; } = string.Empty;
    }
}
