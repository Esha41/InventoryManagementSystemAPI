using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Dtos;
using MediatR;

namespace Ettad.Workflows.Service.Commands.UpdateWorkflowAutoRejectTriggers;

public class UpdateWorkflowAutoRejectTriggersCommand : IRequest<APIOperationResponse<WorkflowAutoRejectTriggerDto>>
{
    public long WorkflowId { get; set; }

    public AutoRejectTriggerMode? Mode { get; set; }

    public List<string> TriggerRoleIds { get; set; } = [];
    public List<long> TriggerStepIds { get; set; } = [];
}
