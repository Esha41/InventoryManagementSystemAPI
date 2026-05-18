using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.RequestManagement.Service.Orders.Services;

namespace Ettad.RequestManagement.Service.Orders.Mapper;

public sealed class OrderPriorityResolver : IValueResolver<Order, OrderDto, RequestPriority>
{
    private readonly IOrderPriorityService _priorityService;

    public OrderPriorityResolver(IOrderPriorityService priorityService)
    {
        _priorityService = priorityService;
    }

    public RequestPriority Resolve(
        Order source,
        OrderDto destination,
        RequestPriority destMember,
        ResolutionContext context)
        => _priorityService.ResolvePriorityForOrderDto(source);
}
