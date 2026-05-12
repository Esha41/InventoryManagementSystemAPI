using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Common.Interfaces;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Common.Services
{
    public class RequestItemWeaponAssociationEnrichmentService : IRequestItemWeaponAssociationEnrichmentService
    {
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;

        public RequestItemWeaponAssociationEnrichmentService(
            ICrossCuttingRepository<BaseItem> baseItemRepository)
        {
            _baseItemRepository = baseItemRepository;
        }

        public async Task EnrichAsync(OrderDto? order)
        {
            if (order == null)
            {
                return;
            }

            await EnrichAsync(order.RequestItems);
        }

        public async Task EnrichAsync(IEnumerable<OrderDto>? orders)
        {
            if (orders == null)
            {
                return;
            }

            var requestItems = orders
                .Where(order => order?.RequestItems != null)
                .SelectMany(order => order.RequestItems);

            await EnrichAsync(requestItems);
        }

        public async Task EnrichAsync(IEnumerable<RequestItemDto>? requestItems)
        {
            if (requestItems == null)
            {
                return;
            }

            var items = requestItems.Where(item => item != null).ToList();
            if (items.Count == 0)
            {
                return;
            }

            var weaponIds = items
                .SelectMany(item => item.WeaponAssociations ?? Enumerable.Empty<RequestItemWeaponAssociationDto>())
                .Where(association => association.AssociatedWeaponItemId.HasValue)
                .Select(association => association.AssociatedWeaponItemId!.Value)
                .Distinct()
                .ToList();

            if (weaponIds.Count == 0)
            {
                return;
            }

            var weapons = await _baseItemRepository.FindAsync(
                item => weaponIds.Contains(item.Id) && !item.IsDeleted);

            var nameMap = weapons.ToDictionary(item => item.Id, item => item.Name);

            foreach (var item in items)
            {
                foreach (var association in item.WeaponAssociations ?? Enumerable.Empty<RequestItemWeaponAssociationDto>())
                {
                    if (!association.AssociatedWeaponItemId.HasValue)
                    {
                        continue;
                    }

                    if (nameMap.TryGetValue(association.AssociatedWeaponItemId.Value, out var name))
                    {
                        association.AssociatedWeaponName = name;
                    }
                }
            }
        }
    }
}
