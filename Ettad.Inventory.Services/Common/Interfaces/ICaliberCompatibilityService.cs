namespace Ettad.Inventory.Service.Common.Interfaces
{
    /// <summary>
    /// Maps ammunition-type <see cref="Ettad.Data.Entities.Caliber"/> rows to weapon-type caliber rows
    /// (same label, different <see cref="Ettad.Data.Enums.ItemType"/> / PK) for issue association and validation.
    /// </summary>
    public interface ICaliberCompatibilityService
    {
        /// <summary>
        /// Returns weapon-type caliber primary keys compatible with the given ammunition caliber id.
        /// </summary>
        Task<IReadOnlyList<long>> GetWeaponCaliberIdsCompatibleWithAmmunitionCaliberIdAsync(long ammunitionCaliberId, CancellationToken cancellationToken = default);

        /// <summary>
        /// True if the weapon's caliber id is compatible with the ammunition row's caliber id.
        /// </summary>
        Task<bool> IsWeaponCaliberCompatibleWithAmmunitionCaliberAsync(long weaponCaliberId, long ammunitionCaliberId, CancellationToken cancellationToken = default);
    }
}
