namespace Ettad.Workflows.Service.Monitoring.Dtos;

public class RequestAutoRejectCountdownDto
{
    public long RequestId { get; set; }

    public DateTime? TriggerReachedAt { get; set; }

    public int ThresholdDays { get; set; }

    public int DaysRemaining { get; set; }

    public DateTime? DueDate { get; set; }

    /// <summary>none | running | warning | expired</summary>
    public string State { get; set; } = "none";
}

/// <summary>Deprecated: Use <see cref="RequestAutoRejectCountdownDto"/> instead.</summary>
[Obsolete("Use RequestAutoRejectCountdownDto instead.")]
public class OrderAutoRejectCountdownDto : RequestAutoRejectCountdownDto
{
}
