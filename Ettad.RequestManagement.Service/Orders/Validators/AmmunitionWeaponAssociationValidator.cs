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

            var catalogWeaponIdsSeen = new HashSet<long>();
            for (var i = 0; i < list.Count; i++)
            {
                var row = list[i];
                var prefix = $"Weapon association #{i + 1}: ";
                var rowErrors = ammunitionCaliberId.HasValue
                    ? await ValidateSingleAssociationWithAmmoCaliberAsync(row, ammunitionCaliberId.Value)
                    : await ValidateSingleAssociationWithoutAmmoCaliberAsync(row);
                foreach (var e in rowErrors)
                    errors.Add(prefix + e);

                if (row.AssociatedWeaponItemId is long wid && wid > 0 && !catalogWeaponIdsSeen.Add(wid))
                    errors.Add($"{prefix}The same catalog weapon cannot be listed more than once.");
            }

            return errors;
        }

        private async Task<List<string>> ValidateSingleAssociationWithoutAmmoCaliberAsync(
            CreateRequestItemWeaponAssociationDto item)
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

            var weapon = await _weaponRepository.FindOneAsync(w =>
                w.Id == item.AssociatedWeaponItemId!.Value && !w.IsDeleted);

            if (weapon == null)
                errors.Add("Associated weapon was not found or is not a valid catalog weapon.");

            return errors;
        }

        private async Task<List<string>> ValidateSingleAssociationWithAmmoCaliberAsync(
            CreateRequestItemWeaponAssociationDto item,
            long ammunitionCaliberId)
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

            var weapon = await _weaponRepository.FindOneAsync(w =>
                w.Id == item.AssociatedWeaponItemId!.Value && !w.IsDeleted);

            if (weapon == null)
            {
                errors.Add("Associated weapon was not found or is not a valid catalog weapon.");
                return errors;
            }

            if (!weapon.CaliberId.HasValue)
                errors.Add("The selected catalog weapon does not have a caliber defined.");

            return errors;
        }
    }
}
