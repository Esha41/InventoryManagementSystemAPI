using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Common.Interfaces;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Common.Services
{
    public class RequestItemWeaponAssociationEnrichmentService : IRequestItemWeaponAssociationEnrichmentService
    {
        private readonly ICrossCuttingRepository<Ammunition> _ammunitionRepository;
        private readonly ICrossCuttingRepository<Weapon> _weaponRepository;

        public RequestItemWeaponAssociationEnrichmentService(
            ICrossCuttingRepository<Ammunition> ammunitionRepository,
            ICrossCuttingRepository<Weapon> weaponRepository)
        {
            _ammunitionRepository = ammunitionRepository;
            _weaponRepository = weaponRepository;
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

            await EnrichAmmunitionCalibersAsync(items);

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

            var weapons = await _weaponRepository.FindAsync(
                weapon => weaponIds.Contains(weapon.Id) && !weapon.IsDeleted,
                false,
                nameof(Weapon.LookupCaliber));

            var weaponMap = weapons.ToDictionary(weapon => weapon.Id);

            foreach (var item in items)
            {
                foreach (var association in item.WeaponAssociations ?? Enumerable.Empty<RequestItemWeaponAssociationDto>())
                {
                    if (!association.AssociatedWeaponItemId.HasValue)
                    {
                        continue;
                    }

                    if (!weaponMap.TryGetValue(association.AssociatedWeaponItemId.Value, out var weapon))
                    {
                        continue;
                    }

                    association.AssociatedWeaponName = weapon.Name;
                    association.AssociatedWeaponNameAr = weapon.NameAr;
                    association.AssociatedWeaponCatalogCaliberId = weapon.CaliberId ?? weapon.LookupCaliber?.Id;
                    association.AssociatedWeaponCatalogCaliberNameEn = weapon.LookupCaliber?.NameEn?.Trim();
                    association.AssociatedWeaponCatalogCaliberNameAr = weapon.LookupCaliber?.NameAr?.Trim();
                }
            }
        }

        private async Task EnrichAmmunitionCalibersAsync(IReadOnlyList<RequestItemDto> items)
        {
            var ammoItemIds = items
                .Where(item => item.ItemType == ItemType.Ammunition)
                .Select(item => item.ItemId)
                .Distinct()
                .ToList();

            if (ammoItemIds.Count == 0)
            {
                return;
            }

            var ammunitions = await _ammunitionRepository.FindAsync(
                ammo => ammoItemIds.Contains(ammo.Id) && !ammo.IsDeleted,
                false,
                nameof(Ammunition.LookupCaliber));

            var ammoMap = ammunitions.ToDictionary(ammo => ammo.Id);

            foreach (var item in items.Where(item => item.ItemType == ItemType.Ammunition))
            {
                if (!ammoMap.TryGetValue(item.ItemId, out var ammo))
                {
                    continue;
                }

                item.ItemCaliberId = ammo.CaliberId ?? ammo.LookupCaliber?.Id;
                item.ItemCaliberNameEn = ammo.LookupCaliber?.NameEn?.Trim();
                item.ItemCaliberNameAr = ammo.LookupCaliber?.NameAr?.Trim();
            }
        }
    }
}
