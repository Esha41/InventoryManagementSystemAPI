using System.Linq;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Workflows.Service.Monitoring;

namespace Ettad.Workflows.Service.Dtos;

/// <summary>Maps <see cref="WorkflowAutoRejectTrigger"/> onto serialized workflow DTO trigger fields.</summary>
public static class WorkflowAutoRejectTriggerMapper
{
    public static void MapToWorkflowDto(WorkflowDto dto, WorkflowAutoRejectTrigger? trigger)
    {
        if (trigger == null || trigger.Mode == AutoRejectTriggerMode.None)
        {
            dto.AutoRejectTriggerMode = null;
            dto.AutoRejectTriggerRoleIds = [];
            dto.AutoRejectTriggerStepIds = [];
            dto.AutoRejectResetOnReApproval = true;
            return;
        }

        dto.AutoRejectTriggerMode = trigger.Mode switch
        {
            AutoRejectTriggerMode.Role => "Role",
            AutoRejectTriggerMode.Step => "Step",
            AutoRejectTriggerMode.None => "None",
            _ => null
        };
        dto.AutoRejectTriggerRoleIds = WorkflowAutoRejectConfigParser.FilterRoleIds(trigger.TriggerRoles).ToList();
        dto.AutoRejectTriggerStepIds = WorkflowAutoRejectConfigParser.FilterStepIds(trigger.TriggerSteps).ToList();
        dto.AutoRejectResetOnReApproval = trigger.ResetOnReApproval;
    }
}
