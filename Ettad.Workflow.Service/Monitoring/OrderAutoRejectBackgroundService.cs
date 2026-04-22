using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Constants;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Notification.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Monitoring;

public class OrderAutoRejectBackgroundService : IOrderAutoRejectBackgroundService
{
    private readonly ApplicationDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INotificationHelperService _notificationHelperService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OrderAutoRejectBackgroundService> _logger;

    public OrderAutoRejectBackgroundService(
        ApplicationDbContext context,
        IDateTimeProvider dateTimeProvider,
        INotificationHelperService notificationHelperService,
        IConfiguration configuration,
        ILogger<OrderAutoRejectBackgroundService> logger)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _notificationHelperService = notificationHelperService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task ScanAsync(CancellationToken cancellationToken = default)
    {
        var effective = await OrderAutoRejectPolicyLoader.LoadAsync(_context, _configuration, _logger, cancellationToken);
        if (effective == null || !effective.IsEnabled || string.IsNullOrEmpty(effective.TriggerRoleId))
        {
            _logger.LogDebug("Order auto-reject skipped (no policy, disabled, or no trigger role).");
            return;
        }

        var now = _dateTimeProvider.Now;

        var orderRequestIds = await _context.BaseRequests
            .AsNoTracking()
            .Where(br => br.RequestType == RequestType.Order && br.Status == RequestStatus.UnderProcess)
            .Select(br => br.Id)
            .ToListAsync(cancellationToken);

        if (orderRequestIds.Count == 0)
            return;

        var intIds = orderRequestIds.Select(x => (int)x).ToList();
        var allSteps = await _context.WorkflowApprovalSteps
            .Include(s => s.WorkflowStep)
            .Include(s => s.Reminders)
            .Where(s => intIds.Contains(s.TargetRequestId)
                     && OrderAutoRejectConstants.OrderWorkflowTypes.Contains(s.RequestType))
            .ToListAsync(cancellationToken);

        var stepsByRequestId = allSteps
            .GroupBy(s => (long)s.TargetRequestId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var requestId in orderRequestIds)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                stepsByRequestId.TryGetValue(requestId, out var steps);
                await ProcessOrderAsync(requestId, steps ?? new List<WorkflowApprovalStep>(), effective, now, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order auto-reject scan failed for request {RequestId}", requestId);
                _context.ChangeTracker.Clear();
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


        foreach (var step in steps)
        {
            if (_context.Entry(step).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
                _context.Attach(step);
        }

        var triggerApproval = steps
            .Where(s =>
                s.WorkflowStep != null
                && s.WorkflowStep.ApplicationRoleId == policy.TriggerRoleId
                && s.Status == RequestStatus.Approved
                && s.ApprovedDate != null)
            .OrderByDescending(s => s.ApprovedDate)
            .FirstOrDefault();

        if (triggerApproval?.WorkflowStep == null)
            return;

        var current = steps.FirstOrDefault(s => s.IsCurrent && IsAwaitingApproval(s.Status));
        if (current?.WorkflowStep == null)
            return;

        if (current.WorkflowStep.StepOrder <= triggerApproval.WorkflowStep.StepOrder)
            return;

        var daysSince = (now.Date - triggerApproval.ApprovedDate!.Value.Date).Days;
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
        _context.WorkflowApprovalStepReminders.Add(reminder);
        current.ExpirationWarningSentAt = now;
        current.ModificationDate = now;

        var baseRequest = await _context.BaseRequests.AsNoTracking()
            .FirstOrDefaultAsync(br => br.Id == requestId, cancellationToken);

        var recipientIds = await ResolveRecipientUserIdsAsync(baseRequest?.RequesterId, policy, cancellationToken);

        // Always include the pending approver — they are the person who needs to act.
        if (!string.IsNullOrEmpty(current.ApproverUserId))
        {
            var set = new HashSet<string>(recipientIds ?? Enumerable.Empty<string>(), StringComparer.Ordinal);
            set.Add(current.ApproverUserId);
            recipientIds = set.ToList();
        }

        await _notificationHelperService.SendNotificationAsync(
            "This order still needs an approval",
            $"Someone still needs to approve this order. If that does not happen {WithinCalendarDaysPhrase(daysRemaining)}, "
            + $"the system will reject the order automatically. "
            + $"Your organization allows {CalendarDaysPhrase(policy.ThresholdDays)} for this approval, counting from when the previous approval in the process was completed.",
            "Request",
            requestId,
            recipientIds,
            null);
    }

    private async Task AutoRejectAsync(
        WorkflowApprovalStep current,
        long requestId,
        OrderAutoRejectEffectivePolicy policy,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var baseRequest = await _context.BaseRequests.FirstOrDefaultAsync(br => br.Id == requestId, cancellationToken);
        if (baseRequest == null || baseRequest.Status != RequestStatus.UnderProcess)
            return;

        var reason =
            $"This order was rejected automatically because nobody completed the required approval in time. "
            + $"The allowed time was {CalendarDaysPhrase(policy.ThresholdDays)}, starting from when the previous approval in the process was completed.";

        current.Status = RequestStatus.Rejected;
        current.IsCurrent = false;
        current.ApprovedDate = now;
        current.Comments =
            $"Auto-rejected: required approval not received within {CalendarDaysPhrase(policy.ThresholdDays)} after the previous approval step.";
        current.ModificationDate = now;

        var others = await _context.WorkflowApprovalSteps
            .Where(x => x.TargetRequestId == (int)requestId
                        && x.IsCurrent
                        && x.Id != current.Id
                        && (x.Status == RequestStatus.New || x.Status == RequestStatus.UnderProcess))
            .ToListAsync(cancellationToken);

        foreach (var s in others)
        {
            s.IsCurrent = false;
            s.ModificationDate = now;
        }

        baseRequest.Status = RequestStatus.Rejected;
        baseRequest.ModificationDate = now;

        var notifyUserIds = await ResolveRecipientUserIdsAsync(baseRequest.RequesterId, policy, cancellationToken);

        await _notificationHelperService.SendNotificationAsync(
            "This order was automatically rejected",
            reason,
            "Request",
            baseRequest.Id,
            notifyUserIds,
            null);
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
            from ur in _context.UserRoles.AsNoTracking()
            join u in _context.Users.AsNoTracking() on ur.UserId equals u.Id
            where roleIds.Contains(ur.RoleId) && u.IsActive && !u.IsDeleted
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
}
