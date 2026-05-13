namespace Ettad.Data.Enums;

public enum AutoRejectTriggerMode
{
    /// <summary>Per-workflow triggers cleared; global policy may apply.</summary>
    None = 0,
    Role = 1,
    Step = 2,
    Disabled = 3
}
