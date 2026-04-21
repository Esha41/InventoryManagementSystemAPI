using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Workflows.Service.Monitoring.Dtos;

namespace Ettad.Workflows.Service.Monitoring;

internal static class OrderAutoRejectCountdownHelper
{
    private static readonly WorkflowType[] OrderWorkflowTypes =
    {
        WorkflowType.NormalOrder,
        WorkflowType.OrderFromAllowance,
        WorkflowType.NormalOrderForTrainingPurpose,
        WorkflowType.NormalOrder_Weapon,
        WorkflowType.OrderFromAllowance_Weapon,
        WorkflowType.NormalOrderForTrainingPurpose_Weapon
    };

    public static OrderAutoRejectCountdownDto Compute(
        long requestId,
        IReadOnlyList<WorkflowApprovalStep> steps,
        OrderAutoRejectEffectivePolicy? policy,
        DateTime now)
    {
        if (policy == null || !policy.IsEnabled || string.IsNullOrEmpty(policy.TriggerRoleId))
            return None(requestId);

        var filtered = steps.Where(s => OrderWorkflowTypes.Contains(s.RequestType)).ToList();
        if (filtered.Count == 0)
            return None(requestId);

        var triggerApproval = filtered
            .Where(s =>
                s.WorkflowStep != null
                && s.WorkflowStep.ApplicationRoleId == policy.TriggerRoleId
                && s.Status == RequestStatus.Approved
                && s.ApprovedDate != null)
            .OrderByDescending(s => s.ApprovedDate)
            .FirstOrDefault();

        if (triggerApproval?.WorkflowStep == null)
            return None(requestId);

        var current = filtered.FirstOrDefault(s => s.IsCurrent && IsAwaitingApproval(s.Status));
        if (current?.WorkflowStep == null)
            return None(requestId);

        if (current.WorkflowStep.StepOrder <= triggerApproval.WorkflowStep.StepOrder)
            return None(requestId);

        var daysSince = (now.Date - triggerApproval.ApprovedDate!.Value.Date).Days;
        var daysRemaining = policy.ThresholdDays - daysSince;
        var dueDate = triggerApproval.ApprovedDate.Value.Date.AddDays(policy.ThresholdDays);

        var maxLead = policy.ReminderLeadDays.Count > 0 ? policy.ReminderLeadDays.Max() : 0;
        string state;
        if (daysRemaining <= 0)
            state = "expired";
        else if (maxLead > 0 && daysRemaining <= maxLead)
            state = "warning";
        else
            state = "running";

        return new OrderAutoRejectCountdownDto
        {
            RequestId = requestId,
            TriggerApprovedAt = triggerApproval.ApprovedDate,
            ThresholdDays = policy.ThresholdDays,
            DaysRemaining = Math.Max(daysRemaining, 0),
            DueDate = dueDate,
            State = state
        };
    }

    private static OrderAutoRejectCountdownDto None(long requestId) => new()
    {
        RequestId = requestId,
        State = "none",
        ThresholdDays = 0,
        DaysRemaining = 0,
        DueDate = null,
        TriggerApprovedAt = null
    };

    private static bool IsAwaitingApproval(RequestStatus status) =>
        status is RequestStatus.New or RequestStatus.UnderProcess;

    public static void AccumulateSummaryBuckets(OrderAutoRejectCountdownDto dto, OrderAutoRejectDashboardSummaryDto summary)
    {
        if (dto.State == "none")
            return;

        if (dto.State == "expired" || dto.DaysRemaining <= 0)
        {
            summary.Overdue++;
            return;
        }

        var d = dto.DaysRemaining;
        if (d <= 1)
            summary.ExpiringWithinOneDay++;
        else if (d <= 3)
            summary.ExpiringWithinThreeDays++;
        else if (d <= 7)
            summary.ExpiringWithinSevenDays++;
    }
}
