using Ettad.Data.Enums;

namespace Ettad.Workflows.Service.Dtos;

/// <summary>Parsed per-workflow auto-reject trigger configuration.</summary>
public record WorkflowAutoRejectTriggerConfig(
    AutoRejectTriggerMode Mode,
    IReadOnlyList<string> RoleIds,
    IReadOnlyList<long> StepIds,
    bool ResetOnReApproval)
{
    public static WorkflowAutoRejectTriggerConfig Disabled => new(
        AutoRejectTriggerMode.None,
        Array.Empty<string>(),
        Array.Empty<long>(),
        true);

    public bool IsDisabled => Mode == AutoRejectTriggerMode.None;

    public bool IsEnabled => !IsDisabled;
}
