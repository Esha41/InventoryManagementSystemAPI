using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Ettad.RequestManagement.Service.Orders.Dto;

namespace Ettad.RequestManagement.Service.Orders.Validators
{
    /// <summary>
    /// Validates weapon association rows for ammunition request lines.
    /// </summary>
    public class AmmunitionWeaponAssociationValidator
    {
        private readonly ICrossCuttingRepository<Weapon> _weaponRepository;

        public AmmunitionWeaponAssociationValidator(
            ICrossCuttingRepository<Weapon> weaponRepository)
        {
            _weaponRepository = weaponRepository;
        }

        /// <summary>
        /// Validates every association row for an ammunition line. Caller resolves caliber from Ammunitions.
        /// </summary>
        public async Task<List<string>> ValidateAmmunitionLineAsync(
            CreateUpdateRequestItemDto item,
            long? ammunitionCaliberId)
        {
            var errors = new List<string>();

            var list = item.WeaponAssociations;
            if (list == null || list.Count == 0)
            {
                errors.Add("For ammunition, specify at least one weapon association.");
                return errors;
            }

            var allCatalogIds = list
                .Where(r => r.AssociatedWeaponItemId is > 0)
                .Select(r => r.AssociatedWeaponItemId!.Value)
                .Distinct()
                .ToList();

            var weaponMap = allCatalogIds.Count > 0
                ? (await _weaponRepository.FindAsync(w => allCatalogIds.Contains(w.Id) && !w.IsDeleted))
                    .ToDictionary(w => w.Id)
                : new Dictionary<long, Weapon>();

            var catalogWeaponIdsSeen = new HashSet<long>();
            for (var i = 0; i < list.Count; i++)
            {
                var row = list[i];
                var prefix = $"Weapon association #{i + 1}: ";
                var rowErrors = ammunitionCaliberId.HasValue
                    ? ValidateSingleAssociationWithAmmoCaliberAsync(row, ammunitionCaliberId.Value, weaponMap)
                    : ValidateSingleAssociationWithoutAmmoCaliberAsync(row, weaponMap);
                foreach (var e in rowErrors)
                    errors.Add(prefix + e);

                if (row.AssociatedWeaponItemId is long wid && wid > 0 && !catalogWeaponIdsSeen.Add(wid))
                    errors.Add($"{prefix}The same catalog weapon cannot be listed more than once.");
            }

            return errors;
        }

        private List<string> ValidateSingleAssociationWithoutAmmoCaliberAsync(
            CreateRequestItemWeaponAssociationDto item,
            IReadOnlyDictionary<long, Weapon> weaponMap)
        {
            var errors = new List<string>();

            var hasCatalogWeapon = item.AssociatedWeaponItemId.HasValue && item.AssociatedWeaponItemId.Value > 0;
            var otherName = item.AssociatedWeaponOtherName?.Trim() ?? string.Empty;
            var hasOtherWeapon = otherName.Length > 0;

            if (hasCatalogWeapon == hasOtherWeapon)
            {
                errors.Add(
                    hasCatalogWeapon && hasOtherWeapon
                        ? "Specify either a catalog weapon (AssociatedWeaponItemId) or a custom weapon name (AssociatedWeaponOtherName), not both."
                        : "Specify either a catalog weapon (AssociatedWeaponItemId) or a custom weapon name (AssociatedWeaponOtherName).");
                return errors;
            }

            if (hasOtherWeapon)
            {
                if (otherName.Length > 255)
                    errors.Add("Custom weapon name cannot exceed 255 characters.");
                return errors;
            }

            weaponMap.TryGetValue(item.AssociatedWeaponItemId!.Value, out var weapon);

            if (weapon == null)
                errors.Add("Associated weapon was not found or is not a valid catalog weapon.");

            return errors;
        }

        private List<string> ValidateSingleAssociationWithAmmoCaliberAsync(
            CreateRequestItemWeaponAssociationDto item,
            long ammunitionCaliberId,
            IReadOnlyDictionary<long, Weapon> weaponMap)
        {
            var errors = new List<string>();

            var hasCatalogWeapon = item.AssociatedWeaponItemId.HasValue && item.AssociatedWeaponItemId.Value > 0;
            var otherName = item.AssociatedWeaponOtherName?.Trim() ?? string.Empty;
            var hasOtherWeapon = otherName.Length > 0;

            if (hasCatalogWeapon == hasOtherWeapon)
            {
                errors.Add(
                    hasCatalogWeapon && hasOtherWeapon
                        ? "Specify either a catalog weapon (AssociatedWeaponItemId) or a custom weapon name (AssociatedWeaponOtherName), not both."
                        : "Specify either a catalog weapon (AssociatedWeaponItemId) or a custom weapon name (AssociatedWeaponOtherName).");
                return errors;
            }

            if (!item.AssociatedWeaponCaliberId.HasValue)
            {
                errors.Add("AssociatedWeaponCaliberId is required.");
                return errors;
            }

            if (item.AssociatedWeaponCaliberId.Value != ammunitionCaliberId)
            {
                errors.Add("AssociatedWeaponCaliberId must match the ammunition's caliber.");
                return errors;
            }

            if (hasOtherWeapon)
            {
                if (otherName.Length > 255)
                    errors.Add("Custom weapon name cannot exceed 255 characters.");
                return errors;
            }

            weaponMap.TryGetValue(item.AssociatedWeaponItemId!.Value, out var weapon);

            if (weapon == null)
            {
                errors.Add("Associated weapon was not found or is not a valid catalog weapon.");
                return errors;
            }

            return errors;
        }
    }
}
