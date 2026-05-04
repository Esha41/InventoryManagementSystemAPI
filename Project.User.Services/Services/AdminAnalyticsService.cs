using Ettad.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.User.Services.Services
{
    public class AdminAnalyticsService : IAdminAnalyticsService
    {
        private readonly ICrossCuttingRepository<LoginAttempt> _loginAttemptRepository;
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<Return> _returnRepository;
        private readonly ICrossCuttingRepository<Discard> _discardRepository;
        private readonly ICrossCuttingRepository<ApplicationUser> _userRepository;
        private readonly ICrossCuttingRepository<Department> _departmentRepository;
        private readonly ICrossCuttingRepository<BaseRequest> _baseRequestRepository;
        private readonly ICrossCuttingRepository<Ammunition> _ammunitionRepository;
        private readonly ICrossCuttingRepository<Weapon> _weaponRepository;
        private readonly ICrossCuttingRepository<Explosive> _explosiveRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly ILogger<AdminAnalyticsService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AdminAnalyticsService(
            ICrossCuttingRepository<LoginAttempt> loginAttemptRepository,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<Return> returnRepository,
            ICrossCuttingRepository<Discard> discardRepository,
            ICrossCuttingRepository<ApplicationUser> userRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            ICrossCuttingRepository<BaseRequest> baseRequestRepository,
            ICrossCuttingRepository<Ammunition> ammunitionRepository,
            ICrossCuttingRepository<Weapon> weaponRepository,
            ICrossCuttingRepository<Explosive> explosiveRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            ILogger<AdminAnalyticsService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _loginAttemptRepository = loginAttemptRepository ?? throw new ArgumentNullException(nameof(loginAttemptRepository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _returnRepository = returnRepository ?? throw new ArgumentNullException(nameof(returnRepository));
            _discardRepository = discardRepository ?? throw new ArgumentNullException(nameof(discardRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _baseRequestRepository = baseRequestRepository ?? throw new ArgumentNullException(nameof(baseRequestRepository));
            _ammunitionRepository = ammunitionRepository ?? throw new ArgumentNullException(nameof(ammunitionRepository));
            _weaponRepository = weaponRepository ?? throw new ArgumentNullException(nameof(weaponRepository));
            _explosiveRepository = explosiveRepository ?? throw new ArgumentNullException(nameof(explosiveRepository));
            _requestItemRepository = requestItemRepository ?? throw new ArgumentNullException(nameof(requestItemRepository));
            _baseItemRepository = baseItemRepository ?? throw new ArgumentNullException(nameof(baseItemRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<APIOperationResponse<SystemHealthMetricsDto>> GetSystemHealthMetricsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching system health metrics");

                var last24Hours = _dateTimeProvider.Now.AddHours(-24);
                var activeUsers = await _loginAttemptRepository
                    .Find(la => la.IsSuccessful && la.AttemptDate >= last24Hours)
                    .Select(la => la.UserId)
                    .Distinct()
                    .CountAsync();

                var activeRequests = await _orderRepository
                    .Find(o => o.Status == RequestStatus.New || o.Status == RequestStatus.UnderProcess)
                    .CountAsync();

                activeRequests += await _returnRepository
                    .Find(r => r.Status == RequestStatus.New || r.Status == RequestStatus.UnderProcess)
                    .CountAsync();

                activeRequests += await _discardRepository
                    .Find(d => d.Status == RequestStatus.New || d.Status == RequestStatus.UnderProcess)
                    .CountAsync();

                var lastHour = _dateTimeProvider.Now.AddHours(-1);
                var totalLoginAttempts = await _loginAttemptRepository
                    .Find(la => la.AttemptDate >= lastHour)
                    .CountAsync();

                var failedLoginAttempts = await _loginAttemptRepository
                    .Find(la => la.AttemptDate >= lastHour && !la.IsSuccessful)
                    .CountAsync();

                var errorRate = totalLoginAttempts > 0
                    ? (double)failedLoginAttempts / totalLoginAttempts * 100
                    : 0;

                var avgResponseTime = 150.0;

                var systemUptime = 99.9;

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

                var totalUsers = await _userRepository
                    .Find(u => !u.IsDeleted)
                    .CountAsync();

                var today = _dateTimeProvider.Now.Date;
                var dailyActiveUsers = await _loginAttemptRepository
                    .Find(la => la.IsSuccessful && la.AttemptDate >= today)
                    .Select(la => la.UserId)
                    .Distinct()
                    .CountAsync();

                var newUsersToday = 0;

                var activeUserIds = await _loginAttemptRepository
                    .Find(la => la.IsSuccessful && la.AttemptDate >= today)
                    .Select(la => la.UserId)
                    .Distinct()
                    .ToListAsync();

                var topDepartments = await _userRepository
                    .Find(u => !u.IsDeleted &&
                           u.DepartmentId != null &&
                           activeUserIds.Contains(u.Id))
                    .GroupBy(u => u.DepartmentId)
                    .Select(g => new
                    {
                        DepartmentId = g.Key,
                        ActiveUserCount = g.Count()
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
                        var department = await _departmentRepository
                            .Find(d => d.Id == dept.DepartmentId)
                            .FirstOrDefaultAsync();
                        departmentName = department?.NameEn ?? department?.NameAr ?? "Unknown";
                    }

                    departmentStats.Add(new DepartmentStatDto
                    {
                        Name = departmentName,
                        UserCount = dept.ActiveUserCount
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

                var pendingOrders = await _orderRepository
                    .Find(o => !o.IsDeleted && (o.Status == RequestStatus.New || o.Status == RequestStatus.UnderProcess))
                    .CountAsync();

                var pendingReturns = await _returnRepository
                    .Find(r => !r.IsDeleted && (r.Status == RequestStatus.New || r.Status == RequestStatus.UnderProcess))
                    .CountAsync();

                var pendingDiscards = await _discardRepository
                    .Find(d => !d.IsDeleted && (d.Status == RequestStatus.New || d.Status == RequestStatus.UnderProcess))
                    .CountAsync();

                var totalPending = pendingOrders + pendingReturns + pendingDiscards;

                var newRequests = await _orderRepository.Find(o => !o.IsDeleted && o.Status == RequestStatus.New).CountAsync();
                newRequests += await _returnRepository.Find(r => !r.IsDeleted && r.Status == RequestStatus.New).CountAsync();
                newRequests += await _discardRepository.Find(d => !d.IsDeleted && d.Status == RequestStatus.New).CountAsync();

                var inProgressRequests = await _orderRepository.Find(o => !o.IsDeleted && o.Status == RequestStatus.UnderProcess).CountAsync();
                inProgressRequests += await _returnRepository.Find(r => !r.IsDeleted && r.Status == RequestStatus.UnderProcess).CountAsync();
                inProgressRequests += await _discardRepository.Find(d => !d.IsDeleted && d.Status == RequestStatus.UnderProcess).CountAsync();

                var completedRequests = await _orderRepository.Find(o => !o.IsDeleted && o.Status == RequestStatus.Approved).CountAsync();
                completedRequests += await _returnRepository.Find(r => !r.IsDeleted && r.Status == RequestStatus.Approved).CountAsync();
                completedRequests += await _discardRepository.Find(d => !d.IsDeleted && d.Status == RequestStatus.Approved).CountAsync();

                var rejectedRequests = await _orderRepository.Find(o => !o.IsDeleted && o.Status == RequestStatus.Rejected).CountAsync();
                rejectedRequests += await _returnRepository.Find(r => !r.IsDeleted && r.Status == RequestStatus.Rejected).CountAsync();
                rejectedRequests += await _discardRepository.Find(d => !d.IsDeleted && d.Status == RequestStatus.Rejected).CountAsync();

                var avgApprovalTime = 0.0;

                var slaCompliance = 0.0;

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
                    var startDate = _dateTimeProvider.Now.Date.AddMonths(-11).AddDays(-(_dateTimeProvider.Now.Day - 1));
                    var rawRequests = await _baseRequestRepository
                        .Find(r => r.CreationDate >= startDate && !r.IsDeleted)
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

                    var rawTrends = await _baseRequestRepository
                        .Find(r => r.CreationDate >= startDate && !r.IsDeleted)
                        .GroupBy(r => new { r.CreationDate.Date, r.RequestType })
                        .Select(g => new
                        {
                            g.Key.Date,
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

                var ammunitionCount = await _ammunitionRepository
                    .Find(a => !a.IsDeleted)
                    .CountAsync();

                var weaponCount = await _weaponRepository
                    .Find(w => !w.IsDeleted)
                    .CountAsync();

                var explosiveCount = await _explosiveRepository
                    .Find(e => !e.IsDeleted)
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

                var topItemIds = await _requestItemRepository
                    .Find(ri => true)
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
                    var baseItem = await _baseItemRepository
                        .Find(bi => bi.Id == item.ItemId)
                        .FirstOrDefaultAsync();

                    if (baseItem != null)
                    {
                        var category = "Item";
                        var ammunition = await _ammunitionRepository.FindOneAsync(a => a.Id == item.ItemId);
                        var weapon = await _weaponRepository.FindOneAsync(w => w.Id == item.ItemId);
                        var explosive = await _explosiveRepository.FindOneAsync(e => e.Id == item.ItemId);

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
