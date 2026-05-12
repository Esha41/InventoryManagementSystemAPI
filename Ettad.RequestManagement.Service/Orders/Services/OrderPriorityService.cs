using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Orders.Services;

/// <inheritdoc />
public sealed class OrderPriorityService : IOrderPriorityService
{
    private const int VeryUrgentThresholdDays = 7;
    private const int UrgentThresholdDays     = 14;

    private readonly IDateTimeProvider _dateTimeProvider;

    public OrderPriorityService(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public RequestPriority CalculatePriorityFromUsageDate(DateTime usageDateFrom)
    {
        var days = GetDaysUntilUsageDate(usageDateFrom);

        // Overdue orders (days < 0) are treated as VeryUrgent —
        // validator blocks past dates at creation, but dates become
        // past over time as active orders age.
        return days switch
        {
            <= VeryUrgentThresholdDays => RequestPriority.VeryUrgent,
            <= UrgentThresholdDays     => RequestPriority.Urgent,
            _                          => RequestPriority.Normal
        };
    }

    /// <inheritdoc />
    public int GetDaysUntilUsageDate(DateTime usageDateFrom)
    {
        var now = _dateTimeProvider.Now;


        var usageLocalDate = usageDateFrom.Kind == DateTimeKind.Utc
            ? usageDateFrom.ToLocalTime().Date
            : usageDateFrom.Date;

        return (usageLocalDate - now.Date).Days;
    }
}
