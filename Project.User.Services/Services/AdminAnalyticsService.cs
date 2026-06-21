using Ettad.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly ICrossCuttingRepository<WorkflowApprovalStep> _approvalStepRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly ILogger<AdminAnalyticsService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMemoryCache _memoryCache;

        // Short-lived cache so concurrent dashboard viewers and the page auto-refresh
        // don't each re-run these aggregate queries against the database.
        private static readonly MemoryCacheEntryOptions AnalyticsCacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
        };
        private const string CacheKeyRequestMetrics = "analytics:request-metrics";
        private const string CacheKeyTopItemsPrefix = "analytics:top-items:";
        private const string CacheKeyTrendsPrefix = "analytics:trends:";
        private const string CacheKeyWorkflowPerfPrefix = "analytics:workflow-perf:";

        public AdminAnalyticsService(
            ICrossCuttingRepository<LoginAttempt> loginAttemptRepository,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<Return> returnRepository,
            ICrossCuttingRepository<Discard> discardRepository,
            ICrossCuttingRepository<ApplicationUser> userRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            ICrossCuttingRepository<BaseRequest> baseRequestRepository,
            ICrossCuttingRepository<WorkflowApprovalStep> approvalStepRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            ILogger<AdminAnalyticsService> logger,
            IDateTimeProvider dateTimeProvider,
            IMemoryCache memoryCache)
        {
            _loginAttemptRepository = loginAttemptRepository ?? throw new ArgumentNullException(nameof(loginAttemptRepository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _returnRepository = returnRepository ?? throw new ArgumentNullException(nameof(returnRepository));
            _discardRepository = discardRepository ?? throw new ArgumentNullException(nameof(discardRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _baseRequestRepository = baseRequestRepository ?? throw new ArgumentNullException(nameof(baseRequestRepository));
            _approvalStepRepository = approvalStepRepository ?? throw new ArgumentNullException(nameof(approvalStepRepository));
            _requestItemRepository = requestItemRepository ?? throw new ArgumentNullException(nameof(requestItemRepository));
            _baseItemRepository = baseItemRepository ?? throw new ArgumentNullException(nameof(baseItemRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
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

                if (_memoryCache.TryGetValue(CacheKeyRequestMetrics, out RequestMetricsDto? cached) && cached != null)
                {
                    return APIOperationResponse<RequestMetricsDto>.Success(cached);
                }

                // Order/Return/Discard all derive from BaseRequest (TPH), so one grouped query
                // replaces the previous ~15 separate Count round-trips.
                var statusCounts = await _baseRequestRepository
                    .Find(r => !r.IsDeleted)
                    .GroupBy(r => new { r.RequestType, r.Status })
                    .Select(g => new { g.Key.RequestType, g.Key.Status, Count = g.Count() })
                    .ToListAsync();

                int CountFor(RequestType? type, params RequestStatus[] statuses) =>
                    statusCounts
                        .Where(c => (type == null || c.RequestType == type) && statuses.Contains(c.Status))
                        .Sum(c => c.Count);

                var pendingOrders = CountFor(RequestType.Order, RequestStatus.New, RequestStatus.UnderProcess);
                var pendingReturns = CountFor(RequestType.Return, RequestStatus.New, RequestStatus.UnderProcess);
                var pendingDiscards = CountFor(RequestType.Discard, RequestStatus.New, RequestStatus.UnderProcess);

                var metrics = new RequestMetricsDto
                {
                    PendingOrders = pendingOrders,
                    PendingReturns = pendingReturns,
                    PendingDiscards = pendingDiscards,
                    TotalPending = pendingOrders + pendingReturns + pendingDiscards,
                    NewRequests = CountFor(null, RequestStatus.New),
                    InProgressRequests = CountFor(null, RequestStatus.UnderProcess),
                    CompletedRequests = CountFor(null, RequestStatus.Approved),
                    RejectedRequests = CountFor(null, RequestStatus.Rejected),
                    LastUpdated = _dateTimeProvider.Now
                };

                _memoryCache.Set(CacheKeyRequestMetrics, metrics, AnalyticsCacheOptions);

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

                var trendsCacheKey = CacheKeyTrendsPrefix + (period ?? string.Empty).ToLower();
                if (_memoryCache.TryGetValue(trendsCacheKey, out RequestTrendsDto? cachedTrends) && cachedTrends != null)
                {
                    return APIOperationResponse<RequestTrendsDto>.Success(cachedTrends);
                }

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

                _memoryCache.Set(trendsCacheKey, trends, AnalyticsCacheOptions);

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

        public async Task<APIOperationResponse<TopRequestedItemsDto>> GetTopRequestedItemsAsync(int limit)
        {
            try
            {
                _logger.LogInformation("Fetching top {Limit} requested items", limit);

                var topItemsCacheKey = CacheKeyTopItemsPrefix + limit;
                if (_memoryCache.TryGetValue(topItemsCacheKey, out TopRequestedItemsDto? cachedItems) && cachedItems != null)
                {
                    return APIOperationResponse<TopRequestedItemsDto>.Success(cachedItems);
                }

                // Orders only: "top requested" reflects demand, not returns/discards.
                // Single grouped query (count + total quantity) instead of the previous
                // per-item N+1, with category resolved from BaseItem.ItemType.
                var topItemIds = await _requestItemRepository
                    .Find(ri => !ri.Request.IsDeleted && ri.Request.RequestType == RequestType.Order)
                    .GroupBy(ri => ri.ItemId)
                    .Select(g => new
                    {
                        ItemId = g.Key,
                        Count = g.Count(),
                        TotalQuantity = g.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(x => x.Count)
                    .Take(limit)
                    .ToListAsync();

                var itemIds = topItemIds.Select(x => x.ItemId).ToList();

                var itemLookup = await _baseItemRepository
                    .Find(bi => itemIds.Contains(bi.Id))
                    .Select(bi => new { bi.Id, bi.Name, bi.NameAr, bi.ItemType })
                    .ToListAsync();

                var topItems = topItemIds
                    .Select(t =>
                    {
                        var info = itemLookup.FirstOrDefault(x => x.Id == t.ItemId);
                        return new RequestedItemDto
                        {
                            ItemName = info?.Name ?? "Unknown",
                            ItemNameAr = info?.NameAr,
                            RequestCount = t.Count,
                            TotalQuantity = t.TotalQuantity,
                            Category = info?.ItemType.ToString() ?? "Item"
                        };
                    })
                    .ToList();

                var result = new TopRequestedItemsDto
                {
                    Items = topItems
                };

                _memoryCache.Set(topItemsCacheKey, result, AnalyticsCacheOptions);

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

        public async Task<APIOperationResponse<WorkflowPerformanceDto>> GetWorkflowPerformanceAsync(int days)
        {
            try
            {
                if (days <= 0) days = 90;
                _logger.LogInformation("Fetching workflow performance for last {Days} days", days);

                var cacheKey = CacheKeyWorkflowPerfPrefix + days;
                if (_memoryCache.TryGetValue(cacheKey, out WorkflowPerformanceDto? cached) && cached != null)
                {
                    return APIOperationResponse<WorkflowPerformanceDto>.Success(cached);
                }

                var now = _dateTimeProvider.Now;
                var windowStart = now.AddDays(-days);

                var terminalStatuses = new[]
                {
                    RequestStatus.Approved,
                    RequestStatus.Rejected,
                    RequestStatus.AutoRejected
                };

                // Completion cohort: requests whose final decision step landed inside the window.
                // Anchoring on decision date (not submission) avoids survivorship bias — a slow
                // request that was just approved is counted with its full duration, so p90 is honest.
                var decisionSteps = await _approvalStepRepository
                    .Find(s => s.ApprovedDate != null
                               && s.ApprovedDate >= windowStart
                               && terminalStatuses.Contains(s.Status))
                    .GroupBy(s => s.TargetRequestId)
                    .Select(g => new { RequestId = g.Key, DecidedAt = g.Max(x => x.ApprovedDate) })
                    .ToListAsync();

                var decidedIds = decisionSteps.Select(d => d.RequestId).ToList();
                var decidedAtLookup = decisionSteps.ToDictionary(d => d.RequestId, d => d.DecidedAt);

                var decidedRequests = await _baseRequestRepository
                    .Find(r => decidedIds.Contains(r.Id) && !r.IsDeleted && terminalStatuses.Contains(r.Status))
                    .Select(r => new { r.Id, r.CreationDate, r.Status })
                    .ToListAsync();

                var approvedRequests = decidedRequests.Where(r => r.Status == RequestStatus.Approved).ToList();
                var approvedCount = approvedRequests.Count;
                var rejectedCount = decidedRequests.Count(r => r.Status == RequestStatus.Rejected || r.Status == RequestStatus.AutoRejected);
                var decided = approvedCount + rejectedCount;
                var approvalRate = decided > 0 ? Math.Round((double)approvedCount / decided * 100, 1) : 0;

                var durations = new List<double>();
                foreach (var request in approvedRequests)
                {
                    if (decidedAtLookup.TryGetValue(request.Id, out var decidedAt) && decidedAt.HasValue)
                    {
                        var hours = (decidedAt.Value - request.CreationDate).TotalHours;
                        if (hours >= 0) durations.Add(hours);
                    }
                }
                durations.Sort();

                var completedAging = BucketByDays(durations.Select(h => h / 24.0));

                // Pending aging = a live snapshot of everything still open (not window-bounded).
                var pendingCreationDates = await _baseRequestRepository
                    .Find(r => !r.IsDeleted &&
                               (r.Status == RequestStatus.New ||
                                r.Status == RequestStatus.UnderProcess ||
                                r.Status == RequestStatus.ReturnedForReview))
                    .Select(r => r.CreationDate)
                    .ToListAsync();

                var pendingAging = BucketByDays(
                    pendingCreationDates.Select(created => (now - created).TotalDays));

                var dto = new WorkflowPerformanceDto
                {
                    Days = days,
                    CompletedCount = durations.Count,
                    AvgCycleHours = durations.Count > 0 ? Math.Round(durations.Average(), 1) : 0,
                    MedianCycleHours = Math.Round(Percentile(durations, 0.5), 1),
                    P90CycleHours = Math.Round(Percentile(durations, 0.9), 1),
                    ApprovedCount = approvedCount,
                    RejectedCount = rejectedCount,
                    ApprovalRate = approvalRate,
                    TotalPending = pendingCreationDates.Count,
                    CompletedAging = completedAging,
                    PendingAging = pendingAging,
                    LastUpdated = now
                };

                _memoryCache.Set(cacheKey, dto, AnalyticsCacheOptions);

                _logger.LogInformation("Workflow performance retrieved successfully");
                return APIOperationResponse<WorkflowPerformanceDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching workflow performance");
                return APIOperationResponse<WorkflowPerformanceDto>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "Failed to retrieve workflow performance");
            }
        }

        // Buckets duration-in-days values into the fixed aging brackets used by the card.
        private static AgingBucketsDto BucketByDays(IEnumerable<double> dayValues)
        {
            var aging = new AgingBucketsDto();
            foreach (var days in dayValues)
            {
                if (days <= 3) aging.UpTo3Days++;
                else if (days <= 7) aging.From3To7Days++;
                else if (days <= 14) aging.From7To14Days++;
                else aging.Over14Days++;
            }
            return aging;
        }

        private static double Percentile(List<double> sortedValues, double percentile)
        {
            if (sortedValues.Count == 0) return 0;
            if (sortedValues.Count == 1) return sortedValues[0];

            var rank = percentile * (sortedValues.Count - 1);
            var low = (int)Math.Floor(rank);
            var high = (int)Math.Ceiling(rank);
            if (low == high) return sortedValues[low];

            return sortedValues[low] + (sortedValues[high] - sortedValues[low]) * (rank - low);
        }
    }
}
