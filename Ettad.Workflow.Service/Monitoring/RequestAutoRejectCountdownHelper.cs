using Ettad.Data.Constants;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Workflows.Service.Dtos;
using Ettad.Workflows.Service.Monitoring.Dtos;

namespace Ettad.Workflows.Service.Monitoring;

internal static class RequestAutoRejectCountdownHelper
{
    public static RequestAutoRejectCountdownDto Compute(
        long requestId,
        IReadOnlyList<WorkflowApprovalStep> steps,
        OrderAutoRejectEffectivePolicy? organizationPolicy,
        WorkflowAutoRejectTriggerConfig? workflowConfig,
        DateTime now,
        IReadOnlyList<WorkflowType>? applicableWorkflowTypes = null)
    {
        if (organizationPolicy == null || !organizationPolicy.IsEnabled)
            return None(requestId);

        var typeFilter = applicableWorkflowTypes ?? OrderAutoRejectConstants.OrderWorkflowTypes;
        var filtered = steps.Where(s => typeFilter.Contains(s.RequestType)).ToList();
        if (filtered.Count == 0)
            return None(requestId);

        var triggerApproval = SelectTriggerApproval(filtered, workflowConfig);
        if (triggerApproval?.WorkflowStep == null)
            return None(requestId);

        var current = filtered.FirstOrDefault(s => s.IsCurrent && IsAwaitingApproval(s.Status));
        if (current?.WorkflowStep == null)
            return None(requestId);

        if (current.WorkflowStep.StepOrder < triggerApproval.WorkflowStep.StepOrder)
            return None(requestId);

        var daysSince = (now.Date - triggerApproval.CreationDate.Date).Days;
        var daysRemaining = organizationPolicy.ThresholdDays - daysSince;
        var dueDate = triggerApproval.CreationDate.Date.AddDays(organizationPolicy.ThresholdDays);

        var maxLead = organizationPolicy.ReminderLeadDays.Count > 0 ? organizationPolicy.ReminderLeadDays.Max() : 0;
        string state;
        if (daysRemaining <= 0)
            state = "expired";
        else if (maxLead > 0 && daysRemaining <= maxLead)
            state = "warning";
        else
            state = "running";

        return new RequestAutoRejectCountdownDto
        {
            RequestId = requestId,
            TriggerReachedAt = triggerApproval.CreationDate,
            ThresholdDays = organizationPolicy.ThresholdDays,
            DaysRemaining = Math.Max(daysRemaining, 0),
            DueDate = dueDate,
            State = state
        };
    }

    /// <summary>Shared trigger row selection for countdown and Hangfire scan.</summary>
    internal static WorkflowApprovalStep? SelectTriggerApproval(
        IReadOnlyList<WorkflowApprovalStep> filtered,
        WorkflowAutoRejectTriggerConfig? workflowConfig)
    {
        if (workflowConfig?.IsEnabled != true)
            return null;

        if (workflowConfig.Mode == AutoRejectTriggerMode.Disabled)
            return null;

        return TrySelectTriggerApprovalByWorkflowConfig(filtered, workflowConfig);
    }

    private static WorkflowApprovalStep? TrySelectTriggerApprovalByWorkflowConfig(
        IReadOnlyList<WorkflowApprovalStep> filtered,
        WorkflowAutoRejectTriggerConfig config)
    {
        var candidates = filtered
            .Where(s =>
                s.WorkflowStep != null
                && MatchesTriggerConfig(s, config))
            .ToList();

        if (candidates.Count == 0)
            return null;

        var sorted = candidates.OrderBy(s => s.CreationDate).ToList();
        return sorted.First();
    }

    private static bool MatchesTriggerConfig(WorkflowApprovalStep approval, WorkflowAutoRejectTriggerConfig config)
    {
        if (approval.WorkflowStep == null)
            return false;

        if (config.Mode == AutoRejectTriggerMode.Disabled)
            return false;

        if (config.Mode == AutoRejectTriggerMode.Step)
            return config.StepIds.Contains(approval.WorkflowStep.Id);

        if (config.Mode == AutoRejectTriggerMode.Role)
        {
            if (config.RoleIds.Contains(approval.WorkflowStep.ApplicationRoleId))
                return true;
            if (approval.WorkflowStep.ParallelRoles?.Any() == true)
                return approval.WorkflowStep.ParallelRoles.Any(pr => config.RoleIds.Contains(pr.RoleId));
        }

        return false;
    }

    internal static RequestAutoRejectCountdownDto NoneDto(long requestId) => None(requestId);

    private static RequestAutoRejectCountdownDto None(long requestId) => new()
    {
        RequestId = requestId,
        State = "none",
        ThresholdDays = 0,
        DaysRemaining = 0,
        DueDate = null,
        TriggerReachedAt = null
    };

    private static bool IsAwaitingApproval(RequestStatus status) =>
        status is RequestStatus.New or RequestStatus.UnderProcess;

    public static void AccumulateSummaryBuckets(RequestAutoRejectCountdownDto dto, RequestAutoRejectDashboardSummaryDto summary)
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
