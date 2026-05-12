using Ettad.Data.Enums;

namespace Ettad.Workflows.Service.Dtos;

public sealed class UpdateWorkflowAutoRejectTriggersDto
{

    public AutoRejectTriggerMode? Mode { get; set; }

    public List<string>? TriggerRoleIds { get; set; }
    public List<long>? TriggerStepIds { get; set; }
}
