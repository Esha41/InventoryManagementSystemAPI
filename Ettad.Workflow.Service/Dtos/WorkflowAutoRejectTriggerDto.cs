namespace Ettad.Workflows.Service.Dtos;

/// <summary>Returned by PUT workflow auto-reject triggers.</summary>
public sealed record WorkflowAutoRejectTriggerDto
{
    public long Id { get; init; }

    public long WorkflowId { get; init; }

    /// <summary>null or empty when disabled.</summary>
    public string? Mode { get; init; }

    public List<string> TriggerRoleIds { get; init; } = new();

    public List<long> TriggerStepIds { get; init; } = new();

    public bool ResetOnReApproval { get; init; } = true;
}
