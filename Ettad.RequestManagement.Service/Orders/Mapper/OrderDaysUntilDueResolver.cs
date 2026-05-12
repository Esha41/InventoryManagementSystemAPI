using AutoMapper;
using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.RequestManagement.Service.Orders.Services;

namespace Ettad.RequestManagement.Service.Orders.Mapper;

/// <summary>
/// Resolves how many calendar days remain until the usage start date.
/// Exposed on the DTO so the frontend needs zero date arithmetic.
/// </summary>
public sealed class OrderDaysUntilDueResolver : IValueResolver<Order, OrderDto, int>
{
    private readonly IOrderPriorityService _priorityService;

    public OrderDaysUntilDueResolver(IOrderPriorityService priorityService)
    {
        _priorityService = priorityService;
    }

    public int Resolve(
        Order source,
        OrderDto destination,
        int destMember,
        ResolutionContext context)
        => _priorityService.GetDaysUntilUsageDate(source.UsageDateFrom);
}
