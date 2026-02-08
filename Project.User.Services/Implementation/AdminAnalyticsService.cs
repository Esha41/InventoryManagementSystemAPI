using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.User.Services.Implementation
{
    public class AdminAnalyticsService : IAdminAnalyticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminAnalyticsService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AdminAnalyticsService(
            ApplicationDbContext context,
            ILogger<AdminAnalyticsService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<APIOperationResponse<SystemHealthMetricsDto>> GetSystemHealthMetricsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching system health metrics");

                // Get active users count (users who logged in within last 24 hours)
                var last24Hours = _dateTimeProvider.Now.AddHours(-24);
                var activeUsers = await _context.LoginAttempts
                    .AsNoTracking()
                    .Where(la => la.IsSuccessful && la.AttemptDate >= last24Hours)
                    .Select(la => la.UserId)
                    .Distinct()
                    .CountAsync();

                // Get current active requests count
                var activeRequests = await _context.Orders
                    .Where(o => o.Status == RequestStatus.New || o.Status == RequestStatus.UnderProcess)
                    .CountAsync();

                activeRequests += await _context.Returns
                    .Where(r => r.Status == RequestStatus.New || r.Status == RequestStatus.UnderProcess)
                    .CountAsync();

                activeRequests += await _context.Discards
                    .Where(d => d.Status == RequestStatus.New || d.Status == RequestStatus.UnderProcess)
                    .CountAsync();

                // Calculate error rate (failed logins in last hour)
                var lastHour = _dateTimeProvider.Now.AddHours(-1);
                var totalLoginAttempts = await _context.LoginAttempts
                    .Where(la => la.AttemptDate >= lastHour)
                    .CountAsync();

                var failedLoginAttempts = await _context.LoginAttempts
                    .Where(la => la.AttemptDate >= lastHour && !la.IsSuccessful)
                    .CountAsync();

                var errorRate = totalLoginAttempts > 0
                    ? (double)failedLoginAttempts / totalLoginAttempts * 100
                    : 0;

                // Calculate average response time (mock - would need actual performance monitoring)
                var avgResponseTime = 150.0; // Placeholder

                // Calculate system uptime (mock - would need actual server uptime tracking)
                var systemUptime = 99.9;

                // Determine system status
                var status = "healthy";
                if (errorRate > 10 || activeRequests > 100)
                {
                    status = "degraded";
                }
                if (errorRate > 25 || activeRequests > 200)
                {
                    status = "critical";
                }

                var metrics = new SystemHealthMetricsDto
                {
                    ActiveUsers = activeUsers,
                    SystemUptime = systemUptime,
                    AvgResponseTime = avgResponseTime,
                    ActiveRequests = activeRequests,
                    ErrorRate = Math.Round(errorRate, 2),
                    Status = status,
                    LastUpdated = _dateTimeProvider.Now
                };

                _logger.LogInformation("System health metrics retrieved successfully");
                return APIOperationResponse<SystemHealthMetricsDto>.Success(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching system health metrics");
                return APIOperationResponse<SystemHealthMetricsDto>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "Failed to retrieve system health metrics");
            }
        }

        public async Task<APIOperationResponse<PerformanceMetricsDto>> GetPerformanceMetricsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching performance metrics");

                // These would typically come from actual system monitoring
                // For now, providing reasonable mock values
                var metrics = new PerformanceMetricsDto
                {
                    CpuUsage = 45.5,
                    MemoryUsage = 62.3,
                    DiskUsage = 38.7,
                    DatabaseConnections = 12,
                    AvgQueryTime = 85.4,
                    LastUpdated = _dateTimeProvider.Now
                };

                _logger.LogInformation("Performance metrics retrieved successfully");
                return APIOperationResponse<PerformanceMetricsDto>.Success(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching performance metrics");
                return APIOperationResponse<PerformanceMetricsDto>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "Failed to retrieve performance metrics");
            }
        }

        public async Task<APIOperationResponse<UserActivityMetricsDto>> GetUserActivityMetricsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching user activity metrics");

                // Get total users (excluding deleted)
                var totalUsers = await _context.Users
                    .Where(u => !u.IsDeleted)
                    .CountAsync();

                // Get daily active users (logged in today)
                var today = _dateTimeProvider.Now.Date;
                var dailyActiveUsers = await _context.LoginAttempts
                    .Where(la => la.IsSuccessful && la.AttemptDate >= today)
                    .Select(la => la.UserId)
                    .Distinct()
                    .CountAsync();

                // Get new users today - ApplicationUser doesn't have CreationDate
                // We'll count users created today by checking if they exist in the system
                var newUsersToday = 0; // Placeholder - would need to track user creation separately

                // Get top departments by DAILY ACTIVE USER count (not total user count)
                // First, get all users who logged in today
                var activeUserIds = await _context.LoginAttempts
                    .Where(la => la.IsSuccessful && la.AttemptDate >= today)
                    .Select(la => la.UserId)
                    .Distinct()
                    .ToListAsync();

                // Then group active users by department
                var topDepartments = await _context.Users
                    .Where(u => !u.IsDeleted && 
                           u.DepartmentId != null && 
                           activeUserIds.Contains(u.Id))
                    .GroupBy(u => u.DepartmentId)
                    .Select(g => new
                    {
                        DepartmentId = g.Key,
                        ActiveUserCount = g.Count() // Daily active users per department
                    })
                    .OrderByDescending(x => x.ActiveUserCount)
                    .Take(5)
                    .ToListAsync();

                var departmentStats = new List<DepartmentStatDto>();
                foreach (var dept in topDepartments)
                {
                    string departmentName;
                    
                    if (dept.DepartmentId == null)
                    {
                        departmentName = "No Department Assigned";
                    }
                    else
                    {
                        var department = await _context.Departments
                            .FirstOrDefaultAsync(d => d.Id == dept.DepartmentId);
                        departmentName = department?.NameEn ?? department?.NameAr ?? "Unknown";
                    }

                    departmentStats.Add(new DepartmentStatDto
                    {
                        Name = departmentName,
                        UserCount = dept.ActiveUserCount // Daily active users for this department
                    });
                }

                var metrics = new UserActivityMetricsDto
                {
                    DailyActiveUsers = dailyActiveUsers,
                    NewUsersToday = newUsersToday,
                    TotalUsers = totalUsers,
                    TopDepartments = departmentStats,
                    LastUpdated = _dateTimeProvider.Now
                };

                _logger.LogInformation("User activity metrics retrieved successfully");
                return APIOperationResponse<UserActivityMetricsDto>.Success(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user activity metrics");
                return APIOperationResponse<UserActivityMetricsDto>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "Failed to retrieve user activity metrics");
            }
        }

        public async Task<APIOperationResponse<RequestMetricsDto>> GetRequestMetricsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching request metrics");

                // Get pending orders count
                var pendingOrders = await _context.Orders
                    .Where(o => !o.IsDeleted && (o.Status == RequestStatus.New || o.Status == RequestStatus.UnderProcess))
                    .CountAsync();

                // Get pending returns count
                var pendingReturns = await _context.Returns
                    .Where(r => !r.IsDeleted && (r.Status == RequestStatus.New || r.Status == RequestStatus.UnderProcess))
                    .CountAsync();

                // Get pending discards count
                var pendingDiscards = await _context.Discards
                    .Where(d => !d.IsDeleted && (d.Status == RequestStatus.New || d.Status == RequestStatus.UnderProcess))
                    .CountAsync();

                var totalPending = pendingOrders + pendingReturns + pendingDiscards;

                // Get new requests count (status = New only)
                var newRequests = await _context.Orders.Where(o => !o.IsDeleted && o.Status == RequestStatus.New).CountAsync();
                newRequests += await _context.Returns.Where(r => !r.IsDeleted && r.Status == RequestStatus.New).CountAsync();
                newRequests += await _context.Discards.Where(d => !d.IsDeleted && d.Status == RequestStatus.New).CountAsync();

                // Get in-progress requests count (status = UnderProcess)
                var inProgressRequests = await _context.Orders.Where(o => !o.IsDeleted && o.Status == RequestStatus.UnderProcess).CountAsync();
                inProgressRequests += await _context.Returns.Where(r => !r.IsDeleted && r.Status == RequestStatus.UnderProcess).CountAsync();
                inProgressRequests += await _context.Discards.Where(d => !d.IsDeleted && d.Status == RequestStatus.UnderProcess).CountAsync();

                // Get completed requests count (status = Approved or Completed)
                var completedRequests = await _context.Orders.Where(o => !o.IsDeleted && o.Status == RequestStatus.Approved).CountAsync();
                completedRequests += await _context.Returns.Where(r => !r.IsDeleted && r.Status == RequestStatus.Approved).CountAsync();
                completedRequests += await _context.Discards.Where(d => !d.IsDeleted && d.Status == RequestStatus.Approved).CountAsync();

                // Get rejected requests count (status = Rejected)
                var rejectedRequests = await _context.Orders.Where(o => !o.IsDeleted && o.Status == RequestStatus.Rejected).CountAsync();
                rejectedRequests += await _context.Returns.Where(r => !r.IsDeleted && r.Status == RequestStatus.Rejected).CountAsync();
                rejectedRequests += await _context.Discards.Where(d => !d.IsDeleted && d.Status == RequestStatus.Rejected).CountAsync();

                // TODO: Calculate average approval time from request history
                var avgApprovalTime = 0.0; // Placeholder

                // TODO: Calculate SLA compliance from request timestamps
                var slaCompliance = 0.0; // Placeholder

                var metrics = new RequestMetricsDto
                {
                    PendingOrders = pendingOrders,
                    PendingReturns = pendingReturns,
                    PendingDiscards = pendingDiscards,
                    TotalPending = totalPending,
                    NewRequests = newRequests,
                    InProgressRequests = inProgressRequests,
                    CompletedRequests = completedRequests,
                    RejectedRequests = rejectedRequests,
                    AvgApprovalTime = avgApprovalTime,
                    SlaCompliance = slaCompliance,
                    LastUpdated = _dateTimeProvider.Now
                };

                _logger.LogInformation("Request metrics retrieved successfully");
                return APIOperationResponse<RequestMetricsDto>.Success(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching request metrics");
                return APIOperationResponse<RequestMetricsDto>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "Failed to retrieve request metrics");
            }
        }

        public async Task<APIOperationResponse<RequestTrendsDto>> GetRequestTrendsAsync(string period)
        {
            try
            {
                _logger.LogInformation("Fetching request trends for period: {Period}", period);

                var dates = new List<string>();
                var orders = new List<int>();
                var returns = new List<int>();
                var discards = new List<int>();

                if (period.ToLower() == "yearly")
                {
                    // Aggregation for the last 12 months
                    var startDate = _dateTimeProvider.Now.Date.AddMonths(-11).AddDays(-(_dateTimeProvider.Now.Day - 1));
                    var rawRequests = await _context.BaseRequests
                        .AsNoTracking()
                        .Where(r => r.CreationDate >= startDate && !r.IsDeleted)
                        .Select(r => new { r.CreationDate, r.RequestType })
                        .ToListAsync();

                    for (int i = 0; i < 12; i++)
                    {
                        var monthDate = startDate.AddMonths(i);
                        dates.Add(monthDate.ToString("MMM yyyy"));
                        
                        orders.Add(rawRequests.Count(x => x.CreationDate.Year == monthDate.Year && x.CreationDate.Month == monthDate.Month && x.RequestType == RequestType.Order));
                        returns.Add(rawRequests.Count(x => x.CreationDate.Year == monthDate.Year && x.CreationDate.Month == monthDate.Month && x.RequestType == RequestType.Return));
                        discards.Add(rawRequests.Count(x => x.CreationDate.Year == monthDate.Year && x.CreationDate.Month == monthDate.Month && x.RequestType == RequestType.Discard));
                    }
                }
                else
                {
                    int days = period.ToLower() switch
                    {
                        "weekly" => 7,
                        "monthly" => 30,
                        _ => 7 
                    };

                    var startDate = _dateTimeProvider.Now.Date.AddDays(-(days - 1));

                    var rawTrends = await _context.BaseRequests
                        .AsNoTracking()
                        .Where(r => r.CreationDate >= startDate && !r.IsDeleted)
                        .GroupBy(r => new { r.CreationDate.Date, r.RequestType })
                        .Select(g => new
                        {
                            Date = g.Key.Date,
                            Type = g.Key.RequestType,
                            Count = g.Count()
                        })
                        .ToListAsync();

                    for (int i = days - 1; i >= 0; i--)
                    {
                        var date = _dateTimeProvider.Now.Date.AddDays(-i);
                        dates.Add(date.ToString("MMM dd"));
                        
                        orders.Add(rawTrends.FirstOrDefault(x => x.Date == date && x.Type == RequestType.Order)?.Count ?? 0);
                        returns.Add(rawTrends.FirstOrDefault(x => x.Date == date && x.Type == RequestType.Return)?.Count ?? 0);
                        discards.Add(rawTrends.FirstOrDefault(x => x.Date == date && x.Type == RequestType.Discard)?.Count ?? 0);
                    }
                }

                var trends = new RequestTrendsDto
                {
                    Dates = dates,
                    Orders = orders,
                    Returns = returns,
                    Discards = discards,
                    Period = period
                };

                _logger.LogInformation("Request trends retrieved successfully");
                return APIOperationResponse<RequestTrendsDto>.Success(trends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching request trends");
                return APIOperationResponse<RequestTrendsDto>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "Failed to retrieve request trends");
            }
        }

        public async Task<APIOperationResponse<InventoryDistributionDto>> GetInventoryDistributionAsync()
        {
            try
            {
                _logger.LogInformation("Fetching inventory distribution");

                // Get counts by category
                var ammunitionCount = await _context.Ammunitions
                    .AsNoTracking()
                    .Where(a => !a.IsDeleted)
                    .CountAsync();

                var weaponCount = await _context.Weapons
                    .AsNoTracking()
                    .Where(w => !w.IsDeleted)
                    .CountAsync();

                var explosiveCount = await _context.Explosives
                    .AsNoTracking()
                    .Where(e => !e.IsDeleted)
                    .CountAsync();

                var totalItems = ammunitionCount + weaponCount + explosiveCount;

                var categories = new List<CategoryDistributionDto>();

                if (totalItems > 0)
                {
                    if (ammunitionCount > 0)
                    {
                        categories.Add(new CategoryDistributionDto
                        {
                            Name = "Ammunition",
                            Value = ammunitionCount,
                            Percentage = Math.Round((double)ammunitionCount / totalItems * 100, 2)
                        });
                    }

                    if (weaponCount > 0)
                    {
                        categories.Add(new CategoryDistributionDto
                        {
                            Name = "Weapons",
                            Value = weaponCount,
                            Percentage = Math.Round((double)weaponCount / totalItems * 100, 2)
                        });
                    }

                    if (explosiveCount > 0)
                    {
                        categories.Add(new CategoryDistributionDto
                        {
                            Name = "Explosives",
                            Value = explosiveCount,
                            Percentage = Math.Round((double)explosiveCount / totalItems * 100, 2)
                        });
                    }
                }

                var distribution = new InventoryDistributionDto
                {
                    Categories = categories
                };

                _logger.LogInformation("Inventory distribution retrieved successfully");
                return APIOperationResponse<InventoryDistributionDto>.Success(distribution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching inventory distribution");
                return APIOperationResponse<InventoryDistributionDto>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "Failed to retrieve inventory distribution");
            }
        }

        public async Task<APIOperationResponse<TopRequestedItemsDto>> GetTopRequestedItemsAsync(int limit)
        {
            try
            {
                _logger.LogInformation("Fetching top {Limit} requested items", limit);

                // Get most requested items by counting RequestItems grouped by ItemId
                var topItemIds = await _context.RequestItems
                    .GroupBy(ri => ri.ItemId)
                    .Select(g => new
                    {
                        ItemId = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .Take(limit)
                    .ToListAsync();

                var topItems = new List<RequestedItemDto>();

                foreach (var item in topItemIds)
                {
                    // Get the item details from BaseItems
                    var baseItem = await _context.BaseItems
                        .FirstOrDefaultAsync(bi => bi.Id == item.ItemId);

                    if (baseItem != null)
                    {
                        // Determine category based on discriminator or type
                        var category = "Item";
                        var ammunition = await _context.Ammunitions.FirstOrDefaultAsync(a => a.Id == item.ItemId);
                        var weapon = await _context.Weapons.FirstOrDefaultAsync(w => w.Id == item.ItemId);
                        var explosive = await _context.Explosives.FirstOrDefaultAsync(e => e.Id == item.ItemId);

                        if (ammunition != null)
                        {
                            category = "Ammunition";
                        }
                        else if (weapon != null)
                        {
                            category = "Weapon";
                        }
                        else if (explosive != null)
                        {
                            category = "Explosive";
                        }

                        topItems.Add(new RequestedItemDto
                        {
                            ItemName = baseItem.Name ?? "Unknown",
                            RequestCount = item.Count,
                            Category = category
                        });
                    }
                }

                var result = new TopRequestedItemsDto
                {
                    Items = topItems
                };

                _logger.LogInformation("Top requested items retrieved successfully");
                return APIOperationResponse<TopRequestedItemsDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching top requested items");
                return APIOperationResponse<TopRequestedItemsDto>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "Failed to retrieve top requested items");
            }
        }
    }
}
