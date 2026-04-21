using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Workflows.Service.Monitoring.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Monitoring;

public class OrderAutoRejectCountdownService : IOrderAutoRejectCountdownService
{
    public const string OrderAutoRejectViewPermission = "Permissions.OrderAutoReject.View";

    public const int MaxBulkRequestIds = 200;

    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OrderAutoRejectCountdownService> _logger;

    public OrderAutoRejectCountdownService(
        ApplicationDbContext context,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IConfiguration configuration,
        ILogger<OrderAutoRejectCountdownService> logger)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<(bool Forbidden, OrderAutoRejectCountdownDto? Dto)> GetForRequestAsync(long requestId, CancellationToken cancellationToken = default)
    {
        if (!await CanViewCountdownAsync(requestId, cancellationToken))
            return (true, null);

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_context, _configuration, _logger, cancellationToken);
        var steps = await LoadStepsForRequestAsync(requestId, cancellationToken);
        var dto = OrderAutoRejectCountdownHelper.Compute(requestId, steps, policy, _dateTimeProvider.Now);
        return (false, dto);
    }

    public async Task<IReadOnlyList<OrderAutoRejectCountdownDto>> GetBulkAsync(IReadOnlyList<long> requestIds, CancellationToken cancellationToken = default)
    {
        var distinct = requestIds.Distinct().Take(MaxBulkRequestIds).ToList();
        if (distinct.Count == 0)
            return Array.Empty<OrderAutoRejectCountdownDto>();

        var allowed = await GetAllowedBulkRequestIdsAsync(distinct, cancellationToken);
        if (allowed.Count == 0)
            return Array.Empty<OrderAutoRejectCountdownDto>();

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_context, _configuration, _logger, cancellationToken);
        var now = _dateTimeProvider.Now;

        var intIds = allowed.Select(x => (int)x).ToList();
        var allSteps = await _context.WorkflowApprovalSteps
            .AsNoTracking()
            .Include(s => s.WorkflowStep)
            .Where(s => intIds.Contains(s.TargetRequestId))
            .ToListAsync(cancellationToken);

        var byRequest = allSteps.GroupBy(s => (long)s.TargetRequestId).ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<OrderAutoRejectCountdownDto>();
        foreach (var id in allowed)
        {
            byRequest.TryGetValue(id, out var steps);
            var dto = OrderAutoRejectCountdownHelper.Compute(id, steps ?? new List<WorkflowApprovalStep>(), policy, now);
            result.Add(dto);
        }

        return result;
    }

    public async Task<OrderAutoRejectDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        var summary = new OrderAutoRejectDashboardSummaryDto();
        if (!_currentUserService.IsUserHasClaim(OrderAutoRejectViewPermission))
            return summary;

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_context, _configuration, _logger, cancellationToken);
        if (policy == null || !policy.IsEnabled)
            return summary;

        var orderRequestIds = await _context.BaseRequests
            .AsNoTracking()
            .Where(br => br.RequestType == RequestType.Order && br.Status == RequestStatus.UnderProcess)
            .Select(br => br.Id)
            .ToListAsync(cancellationToken);

        var now = _dateTimeProvider.Now;
        var intIds = orderRequestIds.Select(x => (int)x).ToList();

        var allSteps = await _context.WorkflowApprovalSteps
            .AsNoTracking()
            .Include(s => s.WorkflowStep)
            .Where(s => intIds.Contains(s.TargetRequestId))
            .ToListAsync(cancellationToken);

        var byRequest = allSteps.GroupBy(s => (long)s.TargetRequestId).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var requestId in orderRequestIds)
        {
            byRequest.TryGetValue(requestId, out var steps);
            var dto = OrderAutoRejectCountdownHelper.Compute(requestId, steps ?? new List<WorkflowApprovalStep>(), policy, now);
            OrderAutoRejectCountdownHelper.AccumulateSummaryBuckets(dto, summary);
        }

        return summary;
    }

    private async Task<List<WorkflowApprovalStep>> LoadStepsForRequestAsync(long requestId, CancellationToken cancellationToken)
    {
        return await _context.WorkflowApprovalSteps
            .AsNoTracking()
            .Include(s => s.WorkflowStep)
            .Where(s => s.TargetRequestId == (int)requestId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Resolves which of the given request IDs the current user may see countdowns for,
    /// using batched queries (no per-id authorization calls).
    /// </summary>
    private async Task<List<long>> GetAllowedBulkRequestIdsAsync(IReadOnlyList<long> distinct, CancellationToken cancellationToken)
    {
        if (_currentUserService.IsUserHasClaim(OrderAutoRejectViewPermission))
            return distinct.ToList();

        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return new List<long>();

        var distinctSet = distinct.ToHashSet();
        var intIds = distinct.Select(x => (int)x).ToList();

        var asRequester = await _context.BaseRequests
            .AsNoTracking()
            .Where(br => distinctSet.Contains(br.Id) && br.RequesterId == userId)
            .Select(br => br.Id)
            .ToListAsync(cancellationToken);

        var asApprover = await _context.WorkflowApprovalSteps
            .AsNoTracking()
            .Where(s => intIds.Contains(s.TargetRequestId) && s.ApproverUserId == userId)
            .Select(s => (long)s.TargetRequestId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var userRoles = await _context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);

        var allowedSet = new HashSet<long>(asRequester);
        foreach (var id in asApprover)
            allowedSet.Add(id);

        if (userRoles.Count > 0)
        {
            var asRole = await (
                from s in _context.WorkflowApprovalSteps.AsNoTracking()
                join ws in _context.WorkflowSteps.AsNoTracking() on s.WorkflowStepId equals ws.Id
                where intIds.Contains(s.TargetRequestId) && s.IsCurrent && userRoles.Contains(ws.ApplicationRoleId)
                select (long)s.TargetRequestId
            ).Distinct().ToListAsync(cancellationToken);

            foreach (var id in asRole)
                allowedSet.Add(id);
        }

        return distinct.Where(id => allowedSet.Contains(id)).ToList();
    }

    private async Task<bool> CanViewCountdownAsync(long requestId, CancellationToken cancellationToken)
    {
        if (_currentUserService.IsUserHasClaim(OrderAutoRejectViewPermission))
            return true;

        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return false;

        if (await _context.BaseRequests.AsNoTracking()
                .AnyAsync(br => br.Id == requestId && br.RequesterId == userId, cancellationToken))
            return true;

        if (await _context.WorkflowApprovalSteps.AsNoTracking()
                .AnyAsync(s => s.TargetRequestId == (int)requestId && s.ApproverUserId == userId, cancellationToken))
            return true;

        var userRoles = await _context.UserRoles.AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);

        if (userRoles.Count == 0)
            return false;

        return await (
            from s in _context.WorkflowApprovalSteps.AsNoTracking()
            join ws in _context.WorkflowSteps.AsNoTracking() on s.WorkflowStepId equals ws.Id
            where s.TargetRequestId == (int)requestId && s.IsCurrent && userRoles.Contains(ws.ApplicationRoleId)
            select s.Id
        ).AnyAsync(cancellationToken);
    }
}
