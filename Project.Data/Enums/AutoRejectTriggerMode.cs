namespace Ettad.Data.Enums;

public enum AutoRejectTriggerMode
{
    /// <summary>Per-workflow trigger not configured; no countdown anchor for this workflow.</summary>
    None = 0,
    Role = 1,
    Step = 2,
    Disabled = 3
}
