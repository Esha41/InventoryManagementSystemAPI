using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Dtos;
using MediatR;

namespace Ettad.Workflows.Service.Commands.UpdateWorkflowAutoRejectTriggers;

public class UpdateWorkflowAutoRejectTriggersCommand : IRequest<APIOperationResponse<WorkflowAutoRejectTriggerDto>>
{
    public long WorkflowId { get; set; }

    /// <summary><see cref="AutoRejectTriggerMode.None"/> = remove per-workflow triggers; <see cref="AutoRejectTriggerMode.Role"/> / <see cref="AutoRejectTriggerMode.Step"/> = active mode.</summary>
    public AutoRejectTriggerMode? Mode { get; set; }

    public List<string> TriggerRoleIds { get; set; } = [];
    public List<long> TriggerStepIds { get; set; } = [];
    public bool ResetOnReApproval { get; set; } = true;
}
