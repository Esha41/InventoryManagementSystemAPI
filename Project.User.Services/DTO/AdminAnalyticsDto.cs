namespace Ettad.User.Services.DTO
{
    /// <summary>
    /// System Health Metrics DTO
    /// </summary>
    public class SystemHealthMetricsDto
    {
        public int ActiveUsers { get; set; }
        public double SystemUptime { get; set; }
        public double AvgResponseTime { get; set; }
        public int ActiveRequests { get; set; }
        public double ErrorRate { get; set; }
        public string Status { get; set; } = "healthy";
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// Performance Metrics DTO
    /// </summary>
    public class PerformanceMetricsDto
    {
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public double DiskUsage { get; set; }
        public int DatabaseConnections { get; set; }
        public double AvgQueryTime { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// User Activity Metrics DTO
    /// </summary>
    public class UserActivityMetricsDto
    {
        public int DailyActiveUsers { get; set; }
        public int NewUsersToday { get; set; }
        public int TotalUsers { get; set; }
        public List<DepartmentStatDto> TopDepartments { get; set; } = new();
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// Department Statistics DTO
    /// </summary>
    public class DepartmentStatDto
    {
        public string Name { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }

    /// <summary>
    /// Request Metrics DTO
    /// </summary>
    public class RequestMetricsDto
    {
        public int PendingOrders { get; set; }
        public int PendingReturns { get; set; }
        public int PendingDiscards { get; set; }
        public int TotalPending { get; set; }
        public int NewRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// Request Trends DTO
    /// </summary>
    public class RequestTrendsDto
    {
        public List<string> Dates { get; set; } = new();
        public List<int> Orders { get; set; } = new();
        public List<int> Returns { get; set; } = new();
        public List<int> Discards { get; set; } = new();
        public string Period { get; set; } = "daily";
    }

    /// <summary>
    /// Top Requested Items DTO
    /// </summary>
    public class TopRequestedItemsDto
    {
        public List<RequestedItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// Requested Item DTO
    /// </summary>
    public class RequestedItemDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string? ItemNameAr { get; set; }
        public int RequestCount { get; set; }
        public long TotalQuantity { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
