using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Constants;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Workflows.Service.Dtos;
using Ettad.Workflows.Service.Monitoring.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Monitoring;

public class RequestAutoRejectCountdownService : IRequestAutoRejectCountdownService, IOrderAutoRejectCountdownService
{
    public const string OrderAutoRejectViewPermission = "Permissions.OrderAutoReject.View";

    public const int MaxBulkRequestIds = 200;

    private readonly ICrossCuttingRepository<BaseRequest> _baseRequestRepository;
    private readonly ICrossCuttingRepository<WorkflowApprovalStep> _workflowApprovalStepRepository;
    private readonly ICrossCuttingRepository<WorkflowStep> _workflowStepRepository;
    private readonly ICrossCuttingRepository<IdentityUserRole<string>> _userRoleRepository;
    private readonly ICrossCuttingRepository<OrderAutoRejectPolicy> _orderAutoRejectPolicyRepository;
    private readonly ICrossCuttingRepository<Ettad.Data.Entities.Settings.Settings> _settingsRepository;
    private readonly IWorkflowAutoRejectConfigCache _workflowAutoRejectConfigCache;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RequestAutoRejectCountdownService> _logger;

    public RequestAutoRejectCountdownService(
        ICrossCuttingRepository<BaseRequest> baseRequestRepository,
        ICrossCuttingRepository<WorkflowApprovalStep> workflowApprovalStepRepository,
        ICrossCuttingRepository<WorkflowStep> workflowStepRepository,
        ICrossCuttingRepository<IdentityUserRole<string>> userRoleRepository,
        ICrossCuttingRepository<OrderAutoRejectPolicy> orderAutoRejectPolicyRepository,
        ICrossCuttingRepository<Ettad.Data.Entities.Settings.Settings> settingsRepository,
        IWorkflowAutoRejectConfigCache workflowAutoRejectConfigCache,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IConfiguration configuration,
        ILogger<RequestAutoRejectCountdownService> logger)
    {
        _baseRequestRepository = baseRequestRepository;
        _workflowApprovalStepRepository = workflowApprovalStepRepository;
        _workflowStepRepository = workflowStepRepository;
        _userRoleRepository = userRoleRepository;
        _orderAutoRejectPolicyRepository = orderAutoRejectPolicyRepository;
        _settingsRepository = settingsRepository;
        _workflowAutoRejectConfigCache = workflowAutoRejectConfigCache;
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public Task<(bool Forbidden, RequestAutoRejectCountdownDto? Dto)> GetForRequestAsync(long requestId, CancellationToken cancellationToken = default)
        => GetCountdownAsync(requestId, "order", cancellationToken);

    public Task<IReadOnlyList<RequestAutoRejectCountdownDto>> GetBulkAsync(IReadOnlyList<long> requestIds, CancellationToken cancellationToken = default)
        => GetBulkAsync(requestIds, "order", cancellationToken);

    public async Task<(bool Forbidden, RequestAutoRejectCountdownDto? Dto)> GetCountdownAsync(long requestId, string requestType, CancellationToken cancellationToken = default)
    {
        if (!TryResolveRequestKind(requestType, out var expectedBaseRequestType, out var workflowFilter))
            return (false, RequestAutoRejectCountdownHelper.NoneDto(requestId));

        if (!await CanViewCountdownAsync(requestId, cancellationToken))
            return (true, null);

        var request = await _baseRequestRepository.FindOneAsync(br => br.Id == requestId);
        if (request == null || request.RequestType != expectedBaseRequestType)
            return (false, RequestAutoRejectCountdownHelper.NoneDto(requestId));

        if (request.Status == RequestStatus.AutoRejected)
            return (false, new RequestAutoRejectCountdownDto { RequestId = requestId, State = "none" });

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_orderAutoRejectPolicyRepository, _settingsRepository, _configuration, _logger, cancellationToken);
        var steps = await LoadStepsForRequestAsync(requestId, cancellationToken);
        var workflowConfig = await ResolveWorkflowTriggerConfigAsync(steps, workflowFilter, cancellationToken);
        var dto = RequestAutoRejectCountdownHelper.Compute(requestId, steps, policy, workflowConfig, _dateTimeProvider.Now, workflowFilter);
        return (false, dto);
    }

    public async Task<IReadOnlyList<RequestAutoRejectCountdownDto>> GetBulkAsync(IReadOnlyList<long> requestIds, string requestType, CancellationToken cancellationToken = default)
    {
        if (!TryResolveRequestKind(requestType, out var expectedBaseRequestType, out var workflowFilter))
            return Array.Empty<RequestAutoRejectCountdownDto>();

        var distinct = requestIds.Distinct().Take(MaxBulkRequestIds).ToList();
        if (distinct.Count == 0)
            return Array.Empty<RequestAutoRejectCountdownDto>();

        var allowed = await GetAllowedBulkRequestIdsAsync(distinct, cancellationToken);
        if (allowed.Count == 0)
            return Array.Empty<RequestAutoRejectCountdownDto>();

        var allowedSet = allowed.ToHashSet();
        var matching = await _baseRequestRepository
            .Find(br => allowedSet.Contains(br.Id) && br.RequestType == expectedBaseRequestType)
            .Select(br => br.Id)
            .ToListAsync(cancellationToken);

        if (matching.Count == 0)
            return Array.Empty<RequestAutoRejectCountdownDto>();

        var matchingSet = matching.ToHashSet();
        var requestStatuses = await _baseRequestRepository
            .Find(br => matchingSet.Contains(br.Id))
            .ToDictionaryAsync(br => br.Id, br => br.Status, cancellationToken);

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_orderAutoRejectPolicyRepository, _settingsRepository, _configuration, _logger, cancellationToken);
        var now = _dateTimeProvider.Now;

        var allSteps = await _workflowApprovalStepRepository
            .Find(s => matching.Contains(s.TargetRequestId), false, nameof(WorkflowApprovalStep.WorkflowStep))
            .ToListAsync(cancellationToken);

        var byRequest = allSteps.GroupBy(s => s.TargetRequestId).ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<RequestAutoRejectCountdownDto>();
        foreach (var id in matching.OrderBy(x => x))
        {
            if (requestStatuses.TryGetValue(id, out var status) && status == RequestStatus.AutoRejected)
            {
                result.Add(new RequestAutoRejectCountdownDto { RequestId = id, State = "none" });
                continue;
            }

            byRequest.TryGetValue(id, out var steps);
            var workflowConfig = await ResolveWorkflowTriggerConfigAsync(steps ?? new List<WorkflowApprovalStep>(), workflowFilter, cancellationToken);
            var dto = RequestAutoRejectCountdownHelper.Compute(id, steps ?? new List<WorkflowApprovalStep>(), policy, workflowConfig, now, workflowFilter);
            result.Add(dto);
        }

        return result;
    }

    public async Task<RequestAutoRejectDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        var summary = new RequestAutoRejectDashboardSummaryDto();
        if (!_currentUserService.IsUserHasClaim(OrderAutoRejectViewPermission))
            return summary;

        var policy = await OrderAutoRejectPolicyLoader.LoadAsync(_orderAutoRejectPolicyRepository, _settingsRepository, _configuration, _logger, cancellationToken);
        if (policy == null || !policy.IsEnabled)
            return summary;

        var orderRequestIds = await _baseRequestRepository
            .Find(br => br.RequestType == RequestType.Order && br.Status == RequestStatus.UnderProcess)
            .Select(br => br.Id)
            .ToListAsync(cancellationToken);

        var workflowFilter = (IReadOnlyList<WorkflowType>)OrderAutoRejectConstants.OrderWorkflowTypes;
        var now = _dateTimeProvider.Now;
        var allSteps = await _workflowApprovalStepRepository
            .Find(s => orderRequestIds.Contains(s.TargetRequestId), false, nameof(WorkflowApprovalStep.WorkflowStep))
            .ToListAsync(cancellationToken);

        var byRequest = allSteps.GroupBy(s => s.TargetRequestId).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var requestId in orderRequestIds)
        {
            byRequest.TryGetValue(requestId, out var steps);
            var workflowConfig = await ResolveWorkflowTriggerConfigAsync(steps ?? new List<WorkflowApprovalStep>(), workflowFilter, cancellationToken);
            var dto = RequestAutoRejectCountdownHelper.Compute(requestId, steps ?? new List<WorkflowApprovalStep>(), policy, workflowConfig, now, workflowFilter);
            RequestAutoRejectCountdownHelper.AccumulateSummaryBuckets(dto, summary);
        }

        return summary;
    }

    private static bool TryResolveRequestKind(
        string requestType,
        out RequestType expectedBaseRequestType,
        out IReadOnlyList<WorkflowType> workflowFilter)
    {
        switch ((requestType ?? "").Trim().ToLowerInvariant())
        {
            case "order":
                expectedBaseRequestType = RequestType.Order;
                workflowFilter = OrderAutoRejectConstants.OrderWorkflowTypes;
                return true;
            case "return":
                expectedBaseRequestType = RequestType.Return;
                workflowFilter = OrderAutoRejectConstants.ReturnWorkflowTypes;
                return true;
            case "discard":
                expectedBaseRequestType = RequestType.Discard;
                workflowFilter = OrderAutoRejectConstants.DiscardWorkflowTypes;
                return true;
            default:
                expectedBaseRequestType = default;
                workflowFilter = Array.Empty<WorkflowType>();
                return false;
        }
    }

    private async Task<List<WorkflowApprovalStep>> LoadStepsForRequestAsync(long requestId, CancellationToken cancellationToken)
    {
        return await _workflowApprovalStepRepository
            .Find(s => s.TargetRequestId == requestId, false, nameof(WorkflowApprovalStep.WorkflowStep))
            .ToListAsync(cancellationToken);
    }

    private async Task<WorkflowAutoRejectTriggerConfig> ResolveWorkflowTriggerConfigAsync(
        IReadOnlyList<WorkflowApprovalStep> steps,
        IReadOnlyList<WorkflowType> applicableWorkflowTypes,
        CancellationToken cancellationToken)
    {
        long? workflowId = null;
        foreach (var s in steps)
        {
            if (s.WorkflowStep == null)
                continue;
            if (!applicableWorkflowTypes.Contains(s.RequestType))
                continue;
            workflowId = s.WorkflowStep.WorkflowId;
            break;
        }

        if (!workflowId.HasValue)
            return WorkflowAutoRejectTriggerConfig.Disabled;

        return await _workflowAutoRejectConfigCache.GetConfigAsync(workflowId.Value, cancellationToken);
    }

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
