namespace Ettad.Data.Enums;

/// <summary>
/// Per-workflow order auto-reject anchor mode. Persisted on <see cref="Ettad.Data.Entities.Workflows.WorkflowAutoRejectTrigger"/> as lowercase strings "role" or "step".
/// </summary>
public enum AutoRejectTriggerMode
{
    /// <summary>Per-workflow triggers cleared; global policy may apply.</summary>
    None = 0,
    Role = 1,
    Step = 2
}
