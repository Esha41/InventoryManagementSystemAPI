using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Constants;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Data.Orders;
using Ettad.Notification.Service.Interfaces;
using Ettad.Workflows.Service.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Monitoring;

public class OrderAutoRejectBackgroundService : IOrderAutoRejectBackgroundService
{
    private readonly ICrossCuttingRepository<BaseRequest> _baseRequestRepository;
    private readonly ICrossCuttingRepository<Order> _orderRepository;
    private readonly ICrossCuttingRepository<OrderAutoRejectPolicy> _orderAutoRejectPolicyRepository;
    private readonly ICrossCuttingRepository<Ettad.Data.Entities.Settings.Settings> _settingsRepository;
    private readonly ICrossCuttingRepository<WorkflowApprovalStep> _workflowApprovalStepRepository;
    private readonly ICrossCuttingRepository<WorkflowApprovalStepReminder> _workflowApprovalStepReminderRepository;
    private readonly ICrossCuttingRepository<IdentityUserRole<string>> _userRoleRepository;
    private readonly ICrossCuttingRepository<ApplicationUser> _userRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INotificationHelperService _notificationHelperService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OrderAutoRejectBackgroundService> _logger;
    private readonly IWorkflowAutoRejectConfigCache _workflowAutoRejectConfigCache;

    public OrderAutoRejectBackgroundService(
        ICrossCuttingRepository<BaseRequest> baseRequestRepository,
        ICrossCuttingRepository<Order> orderRepository,
        ICrossCuttingRepository<OrderAutoRejectPolicy> orderAutoRejectPolicyRepository,
        ICrossCuttingRepository<Ettad.Data.Entities.Settings.Settings> settingsRepository,
        ICrossCuttingRepository<WorkflowApprovalStep> workflowApprovalStepRepository,
        ICrossCuttingRepository<WorkflowApprovalStepReminder> workflowApprovalStepReminderRepository,
        ICrossCuttingRepository<IdentityUserRole<string>> userRoleRepository,
        ICrossCuttingRepository<ApplicationUser> userRepository,
        ITransactionManager transactionManager,
        IDateTimeProvider dateTimeProvider,
        INotificationHelperService notificationHelperService,
        IConfiguration configuration,
        IWorkflowAutoRejectConfigCache workflowAutoRejectConfigCache,
        ILogger<OrderAutoRejectBackgroundService> logger)
    {
        _baseRequestRepository = baseRequestRepository;
        _orderRepository = orderRepository;
        _orderAutoRejectPolicyRepository = orderAutoRejectPolicyRepository;
        _settingsRepository = settingsRepository;
        _workflowApprovalStepRepository = workflowApprovalStepRepository;
        _workflowApprovalStepReminderRepository = workflowApprovalStepReminderRepository;
        _userRoleRepository = userRoleRepository;
        _userRepository = userRepository;
        _transactionManager = transactionManager;
        _dateTimeProvider = dateTimeProvider;
        _notificationHelperService = notificationHelperService;
        _configuration = configuration;
        _workflowAutoRejectConfigCache = workflowAutoRejectConfigCache;
        _logger = logger;
    }

    public async Task ScanAsync(CancellationToken cancellationToken = default)
    {
        var effective = await OrderAutoRejectPolicyLoader.LoadAsync(
            _orderAutoRejectPolicyRepository,
            _settingsRepository,
            _configuration,
            _logger,
            cancellationToken);
        if (effective == null || !effective.IsEnabled)
        {
            _logger.LogDebug("Order auto-reject skipped (no policy or disabled).");
            return;
        }

        var now = _dateTimeProvider.Now;

        var orderRequestIds = await _baseRequestRepository
            .Find(br => br.RequestType == RequestType.Order && br.Status == RequestStatus.UnderProcess)
            .Select(br => br.Id)
            .ToListAsync(cancellationToken);

        if (orderRequestIds.Count == 0)
            return;

        var allSteps = await _workflowApprovalStepRepository
            .Find(
                s => orderRequestIds.Contains(s.TargetRequestId)
                     && OrderAutoRejectConstants.OrderWorkflowTypes.Contains(s.RequestType),
                false,
                nameof(WorkflowApprovalStep.WorkflowStep),
                $"{nameof(WorkflowApprovalStep.WorkflowStep)}.{nameof(WorkflowStep.ParallelRoles)}",
                nameof(WorkflowApprovalStep.Reminders))
            .ToListAsync(cancellationToken);

        var stepsByRequestId = allSteps
            .GroupBy(s => s.TargetRequestId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var requestId in orderRequestIds)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await using var transaction = await _transactionManager.BeginAsync(cancellationToken);
                try
                {
                    stepsByRequestId.TryGetValue(requestId, out var steps);
                    await ProcessOrderAsync(requestId, steps ?? new List<WorkflowApprovalStep>(), effective, now, cancellationToken);
                    await _transactionManager.CommitAsync(cancellationToken);
                }
                catch
                {
                    await _transactionManager.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order auto-reject scan failed for request {RequestId}", requestId);
            }
        }
    }

    private async Task ProcessOrderAsync(
        long requestId,
        List<WorkflowApprovalStep> steps,
        OrderAutoRejectEffectivePolicy policy,
        DateTime now,
        CancellationToken cancellationToken)
    {
        if (steps.Count == 0)
            return;

        var filtered = steps.Where(s => OrderAutoRejectConstants.OrderWorkflowTypes.Contains(s.RequestType)).ToList();
        if (filtered.Count == 0)
            return;

        long? workflowId = filtered.FirstOrDefault()?.WorkflowStep?.WorkflowId;
        var workflowConfig = workflowId.HasValue
            ? await _workflowAutoRejectConfigCache.GetConfigAsync(workflowId.Value, cancellationToken)
            : WorkflowAutoRejectTriggerConfig.Disabled;

        var triggerApproval = RequestAutoRejectCountdownHelper.SelectTriggerApproval(filtered, workflowConfig);

        if (triggerApproval?.WorkflowStep == null)
            return;

        var current = filtered.FirstOrDefault(s => s.IsCurrent && IsAwaitingApproval(s.Status));
        if (current?.WorkflowStep == null)
            return;

        if (current.WorkflowStep.StepOrder < triggerApproval.WorkflowStep.StepOrder)
            return;

        var daysSince = (now.Date - triggerApproval.CreationDate.Date).Days;
        var daysRemaining = policy.ThresholdDays - daysSince;

        if (daysRemaining <= 0)
        {
            await AutoRejectAsync(current, requestId, policy, now, cancellationToken);
            return;
        }

        var sentLeads = new HashSet<int>(current.Reminders.Select(r => r.LeadDays));
        var nextTier = policy.ReminderLeadDays
            .Where(lead => daysRemaining <= lead && !sentLeads.Contains(lead))
            .OrderBy(lead => lead)
            .FirstOrDefault();

        if (nextTier == 0)
            return;

        var reminder = new WorkflowApprovalStepReminder
        {
            WorkflowApprovalStepId = current.Id,
            LeadDays = nextTier,
            SentAt = now,
            CreationDate = now,
            CreatedBy = null
        };
        current.ExpirationWarningSentAt = now;
        current.ModificationDate = now;

        var baseRequest = await _baseRequestRepository.FindOneAsync(br => br.Id == requestId);

        var recipientIds = await ResolveRecipientUserIdsAsync(baseRequest?.RequesterId, policy, cancellationToken);

        if (!string.IsNullOrEmpty(current.ApproverUserId))
        {
            var set = new HashSet<string>(recipientIds ?? Enumerable.Empty<string>(), StringComparer.Ordinal);
            set.Add(current.ApproverUserId);
            recipientIds = set.ToList();
        }

        var requestRef = FormatRequestRef(baseRequest, requestId);

        await _notificationHelperService.SendNotificationAsync(
            $"{requestRef} still needs an approval",
            $"Someone still needs to approve {requestRef}. If that does not happen {WithinCalendarDaysPhrase(daysRemaining)}, "
            + $"the system will reject it automatically. "
            + $"Your organization allows {CalendarDaysPhrase(policy.ThresholdDays)} for this approval, counting from when the previous approval in the process was completed.",
            "Request",
            requestId,
            recipientIds,
            null);

        await _workflowApprovalStepReminderRepository.AddAsync(reminder);
        await _workflowApprovalStepRepository.UpdateAsync(current);
    }

    private async Task AutoRejectAsync(
        WorkflowApprovalStep current,
        long requestId,
        OrderAutoRejectEffectivePolicy policy,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var baseRequest = await _baseRequestRepository.FindOneAsync(br => br.Id == requestId);
        if (baseRequest == null || baseRequest.Status != RequestStatus.UnderProcess)
            return;

        var requestRef = FormatRequestRef(baseRequest, requestId);

        var reason =
            $"{requestRef} was rejected automatically because nobody completed the required approval in time. "
            + $"The allowed time was {CalendarDaysPhrase(policy.ThresholdDays)}, starting from when the previous approval in the process was completed.";

        current.Status = RequestStatus.AutoRejected;
        current.IsCurrent = false;
        current.ApprovedDate = now;
        current.Comments =
            $"Auto-rejected: required approval not received within {CalendarDaysPhrase(policy.ThresholdDays)} after the previous approval step.";
        current.ModificationDate = now;

        var others = await _workflowApprovalStepRepository
            .Find(x => x.TargetRequestId == requestId
                        && x.IsCurrent
                        && x.Id != current.Id
                        && (x.Status == RequestStatus.New || x.Status == RequestStatus.UnderProcess))
            .ToListAsync(cancellationToken);

        foreach (var s in others)
        {
            s.IsCurrent = false;
            s.ModificationDate = now;
        }

        baseRequest.Status = RequestStatus.AutoRejected;
        baseRequest.ModificationDate = now;

        if (baseRequest.RequestType == RequestType.Order)
        {
            var order = await _orderRepository.FindOneAsync(o => o.Id == requestId);
            if (order != null)
            {
                baseRequest.Priority = OrderPriorityFromUsage.CalculatePriorityFromUsageStart(order.UsageDateFrom, now);
            }
        }

        var notifyUserIds = await ResolveRecipientUserIdsAsync(baseRequest.RequesterId, policy, cancellationToken);

        await _notificationHelperService.SendNotificationAsync(
            $"{requestRef} was automatically rejected",
            reason,
            "Request",
            baseRequest.Id,
            notifyUserIds,
            null);

        await _workflowApprovalStepRepository.UpdateAsync(current);
        foreach (var s in others)
            await _workflowApprovalStepRepository.UpdateAsync(s);
        await _baseRequestRepository.UpdateAsync(baseRequest);
    }

    private async Task<List<string>?> ResolveRecipientUserIdsAsync(
        string? requesterId,
        OrderAutoRejectEffectivePolicy policy,
        CancellationToken cancellationToken)
    {
        var set = new HashSet<string>(StringComparer.Ordinal);
        if (policy.NotifyRequester && !string.IsNullOrEmpty(requesterId))
            set.Add(requesterId);

        if (policy.NotifyRoleIds.Count == 0)
            return set.Count == 0 ? null : set.ToList();

        var roleIds = policy.NotifyRoleIds.ToList();
        var userIds = await (
            from ur in _userRoleRepository.Find(ur => roleIds.Contains(ur.RoleId))
            join u in _userRepository.Find(u => u.IsActive && !u.IsDeleted) on ur.UserId equals u.Id
            select ur.UserId
        ).Distinct().ToListAsync(cancellationToken);

        foreach (var id in userIds)
            set.Add(id);

        return set.Count == 0 ? null : set.ToList();
    }

    private static bool IsAwaitingApproval(RequestStatus status) =>
        status is RequestStatus.New or RequestStatus.UnderProcess;

    private static string CalendarDaysPhrase(int days) =>
        days == 1 ? "1 calendar day" : $"{days} calendar days";

    private static string WithinCalendarDaysPhrase(int days) =>
        days == 1 ? "within 1 calendar day" : $"within {days} calendar days";

    private static string FormatRequestRef(BaseRequest? baseRequest, long requestId) =>
        !string.IsNullOrWhiteSpace(baseRequest?.RequestNo)
            ? $"Request #{baseRequest.RequestNo.Trim()}"
            : $"Request #{requestId}";
}
