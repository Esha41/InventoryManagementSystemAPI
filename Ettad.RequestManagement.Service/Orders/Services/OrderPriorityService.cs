using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Orders;

namespace Ettad.RequestManagement.Service.Orders.Services;

/// <inheritdoc />
public sealed class OrderPriorityService : IOrderPriorityService
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public OrderPriorityService(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public RequestPriority CalculatePriorityFromUsageDate(DateTime usageDateFrom) =>
        OrderPriorityFromUsage.CalculatePriorityFromUsageStart(usageDateFrom, _dateTimeProvider.Now);

    /// <inheritdoc />
    public int GetDaysUntilUsageDate(DateTime usageDateFrom) =>
        OrderPriorityFromUsage.GetCalendarDaysUntilUsageStart(usageDateFrom, _dateTimeProvider.Now);

    /// <inheritdoc />
    public RequestPriority ResolvePriorityForOrderDto(Order order)
    {
        if (!OrderPriorityLifecycle.UsesLivePriorityFromUsageDate(order.Status))
            return order.Priority;

        return OrderPriorityFromUsage.CalculatePriorityFromUsageStart(order.UsageDateFrom, _dateTimeProvider.Now);
    }
}
