using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Workflows.Service.Monitoring.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Monitoring;

public class OrderAutoRejectCountdownService : IOrderAutoRejectCountdownService
{
    public const string OrderAutoRejectViewPermission = "Permissions.OrderAutoReject.View";

    public const int MaxBulkRequestIds = 200;

    private readonly ICrossCuttingRepository<BaseRequest> _baseRequestRepository;
    private readonly ICrossCuttingRepository<WorkflowApprovalStep> _workflowApprovalStepRepository;
    private readonly ICrossCuttingRepository<WorkflowStep> _workflowStepRepository;
    private readonly ICrossCuttingRepository<IdentityUserRole<string>> _userRoleRepository;
    private readonly ICrossCuttingRepository<OrderAutoRejectPolicy> _orderAutoRejectPolicyRepository;
    private readonly ICrossCuttingRepository<Ettad.Data.Entities.Settings.Settings> _settingsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OrderAutoRejectCountdownService> _logger;

    public OrderAutoRejectCountdownService(
        ICrossCuttingRepository<BaseRequest> baseRequestRepository,
        ICrossCuttingRepository<WorkflowApprovalStep> workflowApprovalStepRepository,
        ICrossCuttingRepository<WorkflowStep> workflowStepRepository,
        ICrossCuttingRepository<IdentityUserRole<string>> userRoleRepository,
        ICrossCuttingRepository<OrderAutoRejectPolicy> orderAutoRejectPolicyRepository,
        ICrossCuttingRepository<Ettad.Data.Entities.Settings.Settings> settingsRepository,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IConfiguration configuration,
        ILogger<OrderAutoRejectCountdownService> logger)
    {
        _baseRequestRepository = baseRequestRepository;
        _workflowApprovalStepRepository = workflowApprovalStepRepository;
        _workflowStepRepository = workflowStepRepository;
        _userRoleRepository = userRoleRepository;
        _orderAutoRejectPolicyRepository = orderAutoRejectPolicyRepository;
        _settingsRepository = settingsRepository;
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<(bool Forbidden, OrderAutoRejectCountdownDto? Dto)> GetForRequestAsync(long requestId, CancellationToken cancellationToken = default)
    {
        if (!await CanViewCountdownAsync(requestId, cancellationToken))
            return (true, null);

        var request = await _baseRequestRepository.FindOneAsync(br => br.Id == requestId);

        if (request?.Status == RequestStatus.AutoRejected)
            return (false, new OrderAutoRejectCountdownDto { RequestId = requestId, State = "none" });

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_orderAutoRejectPolicyRepository, _settingsRepository, _configuration, _logger, cancellationToken);
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

        var requestStatuses = await _baseRequestRepository
            .Find(br => allowed.Contains(br.Id))
            .ToDictionaryAsync(br => br.Id, br => br.Status, cancellationToken);

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_orderAutoRejectPolicyRepository, _settingsRepository, _configuration, _logger, cancellationToken);
        var now = _dateTimeProvider.Now;

        var allSteps = await _workflowApprovalStepRepository
            .Find(s => allowed.Contains(s.TargetRequestId), false, nameof(WorkflowApprovalStep.WorkflowStep))
            .ToListAsync(cancellationToken);

        var byRequest = allSteps.GroupBy(s => s.TargetRequestId).ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<OrderAutoRejectCountdownDto>();
        foreach (var id in allowed)
        {
            if (requestStatuses.TryGetValue(id, out var status) && status == RequestStatus.AutoRejected)
            {
                result.Add(new OrderAutoRejectCountdownDto { RequestId = id, State = "none" });
                continue;
            }

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

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_orderAutoRejectPolicyRepository, _settingsRepository, _configuration, _logger, cancellationToken);
        if (policy == null || !policy.IsEnabled)
            return summary;

        var orderRequestIds = await _baseRequestRepository
            .Find(br => br.RequestType == RequestType.Order && br.Status == RequestStatus.UnderProcess)
            .Select(br => br.Id)
            .ToListAsync(cancellationToken);

        var now = _dateTimeProvider.Now;
        var allSteps = await _workflowApprovalStepRepository
            .Find(s => orderRequestIds.Contains(s.TargetRequestId), false, nameof(WorkflowApprovalStep.WorkflowStep))
            .ToListAsync(cancellationToken);

        var byRequest = allSteps.GroupBy(s => s.TargetRequestId).ToDictionary(g => g.Key, g => g.ToList());

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
        return await _workflowApprovalStepRepository
            .Find(s => s.TargetRequestId == requestId, false, nameof(WorkflowApprovalStep.WorkflowStep))
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

        var asRequester = await _baseRequestRepository
            .Find(br => distinctSet.Contains(br.Id) && br.RequesterId == userId)
            .Select(br => br.Id)
            .ToListAsync(cancellationToken);

        var asApprover = await _workflowApprovalStepRepository
            .Find(s => distinct.Contains(s.TargetRequestId) && s.ApproverUserId == userId)
            .Select(s => s.TargetRequestId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var userRoles = await _userRoleRepository
            .Find(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);

        var allowedSet = new HashSet<long>(asRequester);
        foreach (var id in asApprover)
            allowedSet.Add(id);

        if (userRoles.Count > 0)
        {
            var asRole = await (
                from s in _workflowApprovalStepRepository.Find(s => distinct.Contains(s.TargetRequestId) && s.IsCurrent)
                join ws in _workflowStepRepository.Find(ws => userRoles.Contains(ws.ApplicationRoleId)) on s.WorkflowStepId equals ws.Id
                select s.TargetRequestId
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

        if (await _baseRequestRepository
                .Find(br => br.Id == requestId && br.RequesterId == userId)
                .AnyAsync(cancellationToken))
            return true;

        if (await _workflowApprovalStepRepository
                .Find(s => s.TargetRequestId == requestId && s.ApproverUserId == userId)
                .AnyAsync(cancellationToken))
            return true;

        var userRoles = await _userRoleRepository
            .Find(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);

        if (userRoles.Count == 0)
            return false;

        return await (
            from s in _workflowApprovalStepRepository.Find(s => s.TargetRequestId == requestId && s.IsCurrent)
            join ws in _workflowStepRepository.Find(ws => userRoles.Contains(ws.ApplicationRoleId)) on s.WorkflowStepId equals ws.Id
            select s.Id
        ).AnyAsync(cancellationToken);
    }
}
