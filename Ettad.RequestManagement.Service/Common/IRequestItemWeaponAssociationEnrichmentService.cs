using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Common
{
    public interface IRequestItemWeaponAssociationEnrichmentService
    {
        Task EnrichAsync(IEnumerable<RequestItemDto>? requestItems);

        Task EnrichAsync(OrderDto? order);

        Task EnrichAsync(IEnumerable<OrderDto>? orders);
    }
}
