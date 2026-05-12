using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Inventory.Service.Common.Services
{
    public class CaliberCompatibilityService : ICaliberCompatibilityService
    {
        private readonly ICrossCuttingRepository<Caliber> _caliberRepository;

        public CaliberCompatibilityService(ICrossCuttingRepository<Caliber> caliberRepository)
        {
            _caliberRepository = caliberRepository;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<long>> GetWeaponCaliberIdsCompatibleWithAmmunitionCaliberIdAsync(
            long ammunitionCaliberId,
            CancellationToken cancellationToken = default)
        {
            var row = await _caliberRepository.FindOneAsync(c => c.Id == ammunitionCaliberId && !c.IsDeleted);
            if (row == null)
                return Array.Empty<long>();

            if (row.ItemType == ItemType.Weapon)
                return new List<long> { row.Id };

            var nameEn = row.NameEn?.Trim() ?? string.Empty;
            var nameAr = row.NameAr?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(nameEn) && string.IsNullOrEmpty(nameAr))
                return Array.Empty<long>();

            // Use ToLower() — EF Core translates to SQL (OrdinalIgnoreCase overload is not translatable).
            var nameEnKey = string.IsNullOrEmpty(nameEn) ? null : nameEn.ToLowerInvariant();
            var nameArKey = string.IsNullOrEmpty(nameAr) ? null : nameAr.ToLowerInvariant();

            var query = _caliberRepository.Find(
                c =>
                    !c.IsDeleted &&
                    c.ItemType == ItemType.Weapon &&
                    (
                        (nameEnKey != null && c.NameEn != null &&
                         c.NameEn.Trim().ToLower() == nameEnKey) ||
                        (nameArKey != null && c.NameAr != null &&
                         c.NameAr.Trim().ToLower() == nameArKey)
                    ));

            return await query.Select(c => c.Id).ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> IsWeaponCaliberCompatibleWithAmmunitionCaliberAsync(
            long weaponCaliberId,
            long ammunitionCaliberId,
            CancellationToken cancellationToken = default)
        {
            var compatible = await GetWeaponCaliberIdsCompatibleWithAmmunitionCaliberIdAsync(ammunitionCaliberId, cancellationToken);
            return compatible.Contains(weaponCaliberId);
        }
    }
}
