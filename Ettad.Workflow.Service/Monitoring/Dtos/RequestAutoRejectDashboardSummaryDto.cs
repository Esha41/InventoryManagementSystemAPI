namespace Ettad.Workflows.Service.Monitoring.Dtos;

public class RequestAutoRejectDashboardSummaryDto
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

/// <summary>Deprecated: Use <see cref="RequestAutoRejectDashboardSummaryDto"/> instead.</summary>
[Obsolete("Use RequestAutoRejectDashboardSummaryDto instead.")]
public class OrderAutoRejectDashboardSummaryDto : RequestAutoRejectDashboardSummaryDto
{
}
