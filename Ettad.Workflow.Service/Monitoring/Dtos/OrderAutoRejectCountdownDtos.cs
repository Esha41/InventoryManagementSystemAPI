namespace Ettad.Workflows.Service.Monitoring.Dtos;

public class OrderAutoRejectCountdownDto
{
    public long RequestId { get; set; }

    public DateTime? TriggerApprovedAt { get; set; }

    public int ThresholdDays { get; set; }

    public int DaysRemaining { get; set; }

    public DateTime? DueDate { get; set; }

    /// <summary>none | running | warning | expired</summary>
    public string State { get; set; } = "none";
}

public class OrderAutoRejectDashboardSummaryDto
{
    /// <summary>Requests with 1 day or less remaining (still &gt; 0).</summary>
    public int ExpiringWithinOneDay { get; set; }

    /// <summary>Requests with 2–3 days remaining.</summary>
    public int ExpiringWithinThreeDays { get; set; }

    /// <summary>Requests with 4–7 days remaining.</summary>
    public int ExpiringWithinSevenDays { get; set; }

    /// <summary>Requests past due (job not run yet or in same day window).</summary>
    public int Overdue { get; set; }
}
