namespace Ettad.Inventory.Service.AdvancedAnalytics.Dtos
{
    /// <summary>
    /// Order Status Distribution Chart Data
    /// </summary>
    public class OrderStatusDistributionDto
    {
        public List<OrderStatusDataPointDto> StatusDistribution { get; set; } = new();
        public int TotalOrders { get; set; }
    }

    public class OrderStatusDataPointDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Request Trends Over Time
    /// </summary>
    public class RequestTrendsDto
    {
        public List<RequestTrendDataPointDto> TrendData { get; set; } = new();
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
    }

    public class RequestTrendDataPointDto
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public int ReturnCount { get; set; }
        public int DiscardCount { get; set; }
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// Inventory Value by Warehouse
    /// </summary>
    public class InventoryValueDto
    {
        public List<WarehouseValueDto> ByWarehouse { get; set; } = new();
        public decimal TotalValue { get; set; }
        public int TotalItems { get; set; }
    }

    public class WarehouseValueDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public int ItemCount { get; set; }
        public int LowStockItems { get; set; }
    }

    /// <summary>
    /// Asset Assignment Status
    /// </summary>
    public class AssetAssignmentStatusDto
    {
        public List<AssetStatusDataPointDto> StatusDistribution { get; set; } = new();
        public int TotalAssets { get; set; }
        public int AssignedAssets { get; set; }
        public int UnassignedAssets { get; set; }
    }

    public class AssetStatusDataPointDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Supply Fulfillment Status
    /// </summary>
    public class SupplyFulfillmentStatusDto
    {
        public List<SupplyStatusDataPointDto> StatusDistribution { get; set; } = new();
        public int TotalSupplies { get; set; }
        public decimal AverageFulfillmentRate { get; set; }
    }

    public class SupplyStatusDataPointDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Department Request Volume
    /// </summary>
    public class DepartmentRequestVolumeDto
    {
        public List<DepartmentVolumeDto> ByDepartment { get; set; } = new();
        public int TotalRequests { get; set; }
    }

    public class DepartmentVolumeDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int RequestCount { get; set; }
        public int OrderCount { get; set; }
        public int ReturnCount { get; set; }
        public int DiscardCount { get; set; }
    }

    /// <summary>
    /// Notification Statistics
    /// </summary>
    public class NotificationStatisticsDto
    {
        public List<NotificationTrendDataPointDto> TrendData { get; set; } = new();
        public List<NotificationByTypeDto> ByType { get; set; } = new();
        public List<NotificationByEntityDto> ByEntity { get; set; } = new();
        public NotificationReadStatusDto ReadStatus { get; set; } = new();
        public int TotalNotifications { get; set; }
        public int UnreadNotifications { get; set; }
        public int ReadNotifications { get; set; }
        public decimal AverageReadTimeHours { get; set; }
    }

    public class NotificationTrendDataPointDto
    {
        public DateTime Date { get; set; }
        public int SentCount { get; set; }
        public int ReadCount { get; set; }
        public int UnreadCount { get; set; }
    }

    public class NotificationByTypeDto
    {
        public string EntityType { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class NotificationByEntityDto
    {
        public string EntityType { get; set; } = string.Empty;
        public int NotificationCount { get; set; }
        public int ReadCount { get; set; }
        public int UnreadCount { get; set; }
        public decimal ReadRate { get; set; }
    }

    public class NotificationReadStatusDto
    {
        public int TotalSent { get; set; }
        public int TotalRead { get; set; }
        public int TotalUnread { get; set; }
        public decimal ReadRate { get; set; }
        public decimal AverageReadTimeHours { get; set; }
    }

    /// <summary>
    /// Import/Export Statistics DTO
    /// </summary>
    public class ImportExportStatisticsDto
    {
        public List<ImportExportTrendDataPointDto> TrendData { get; set; } = new();
        public List<ImportExportByTypeDto> ByType { get; set; } = new();
        public List<ImportExportByEntityDto> ByEntity { get; set; } = new();
        public ImportExportSummaryDto Summary { get; set; } = new();
        public int TotalImports { get; set; }
        public int TotalExports { get; set; }
    }

    public class ImportExportTrendDataPointDto
    {
        public DateTime Date { get; set; }
        public int ImportCount { get; set; }
        public int ExportCount { get; set; }
        public int TotalOperations { get; set; }
    }

    public class ImportExportByTypeDto
    {
        public string Type { get; set; } = string.Empty; // "Inventory", "Asset", "Export", etc.
        public int ImportCount { get; set; }
        public int ExportCount { get; set; }
        public int TotalCount { get; set; }
        public decimal Percentage { get; set; }
    }

    public class ImportExportByEntityDto
    {
        public string EntityType { get; set; } = string.Empty;
        public int ImportCount { get; set; }
        public int ExportCount { get; set; }
        public int TotalCount { get; set; }
        public decimal ImportPercentage { get; set; }
        public decimal ExportPercentage { get; set; }
    }

    public class ImportExportSummaryDto
    {
        public int TotalImports { get; set; }
        public int TotalExports { get; set; }
        public int TotalOperations { get; set; }
        public decimal ImportPercentage { get; set; }
        public decimal ExportPercentage { get; set; }
        public DateTime? LastImportDate { get; set; }
        public DateTime? LastExportDate { get; set; }
    }

    /// <summary>
    /// User Login Analytics
    /// Tracks user logins and calculates login duration
    /// </summary>
    public class UserLoginAnalyticsDto
    {
        public List<UserLoginDataPointDto> UserLogins { get; set; } = new();
        public List<LoginTrendDataPointDto> TrendData { get; set; } = new();
        public int TotalLogins { get; set; }
        public int UniqueUsers { get; set; }
        public decimal AverageLoginDurationHours { get; set; }
        public int ActiveUsersToday { get; set; }
    }

    public class UserLoginDataPointDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public int LoginCount { get; set; }
        public decimal TotalLoginDurationHours { get; set; }
        public decimal AverageLoginDurationHours { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public class LoginTrendDataPointDto
    {
        public DateTime Date { get; set; }
        public int LoginCount { get; set; }
        public int UniqueUsers { get; set; }
        public decimal AverageDurationHours { get; set; }
    }
}
