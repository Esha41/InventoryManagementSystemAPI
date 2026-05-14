using Ettad.Data.Enums;

namespace Ettad.Data.Orders;

/// <summary>
/// Pure usage-date → priority rules shared by Request Management and Workflow.
/// Matches calendar-day semantics used by <c>OrderPriorityService</c> (UTC usage dates normalized to local date).
/// </summary>
public static class OrderPriorityFromUsage
{
    private const int VeryUrgentThresholdDays = 7;
    private const int UrgentThresholdDays = 14;

    /// <summary>
    /// Whole calendar days from <paramref name="now"/>'s date to usage start (local calendar day for usage).
    /// Negative means usage start is in the past.
    /// </summary>
    public static int GetCalendarDaysUntilUsageStart(DateTime usageDateFrom, DateTime now)
    {
        var usageLocalDate = usageDateFrom.Kind == DateTimeKind.Utc
            ? usageDateFrom.ToLocalTime().Date
            : usageDateFrom.Date;

        return (usageLocalDate - now.Date).Days;
    }

    /// <summary>
    /// Overdue or ≤7 days → VeryUrgent | ≤14 days → Urgent | else → Normal.
    /// </summary>
    public static RequestPriority CalculatePriorityFromUsageStart(DateTime usageDateFrom, DateTime now)
    {
        var days = GetCalendarDaysUntilUsageStart(usageDateFrom, now);

        return days switch
        {
            <= VeryUrgentThresholdDays => RequestPriority.VeryUrgent,
            <= UrgentThresholdDays => RequestPriority.Urgent,
            _ => RequestPriority.Normal
        };
    }
}
