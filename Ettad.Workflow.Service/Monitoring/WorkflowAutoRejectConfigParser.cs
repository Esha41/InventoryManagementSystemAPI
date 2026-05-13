using System.Linq;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Workflows.Service.Dtos;

namespace Ettad.Workflows.Service.Monitoring;

/// <summary>Parses normalized <see cref="WorkflowAutoRejectTrigger"/> into runtime config.</summary>
public static class WorkflowAutoRejectConfigParser
{
    public static WorkflowAutoRejectTriggerConfig Parse(WorkflowAutoRejectTrigger? trigger)
    {
        if (trigger == null || trigger.Mode == AutoRejectTriggerMode.None)
            return WorkflowAutoRejectTriggerConfig.Disabled;

        if (trigger.Mode == AutoRejectTriggerMode.Disabled)
            return new WorkflowAutoRejectTriggerConfig(
                AutoRejectTriggerMode.Disabled,
                Array.Empty<string>(),
                Array.Empty<long>());

        var roleIds = FilterRoleIds(trigger.TriggerRoles ?? Enumerable.Empty<WorkflowAutoRejectTriggerRole>());
        var stepIds = FilterStepIds(trigger.TriggerSteps ?? Enumerable.Empty<WorkflowAutoRejectTriggerStep>());

        return new WorkflowAutoRejectTriggerConfig(
            trigger.Mode,
            roleIds,
            stepIds);
    }

    public static IReadOnlyList<string> FilterRoleIds(IEnumerable<WorkflowAutoRejectTriggerRole> roles)
        => roles
            .Select(tr => tr.RoleId)
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .ToList();

    public static IReadOnlyList<long> FilterStepIds(IEnumerable<WorkflowAutoRejectTriggerStep> steps)
        => steps
            .Select(ts => ts.WorkflowStepId)
            .Where(s => s > 0)
            .ToList();
}
