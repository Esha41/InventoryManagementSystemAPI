using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Weapon : BaseItem
    {
        /// <summary>Defaults to <see cref="WeaponCaliberCategory.Small"/> (1).</summary>
        public WeaponCaliberCategory CaliberCategory { get; set; } = WeaponCaliberCategory.Small;

        public long? CaliberId { get; set; }
        public long? CaliberUnitId { get; set; }
        public int? YearOfManufacture { get; set; }
        public long? CountryOfManufactureId { get; set; }
        public string? Model { get; set; }

        #region Navigation Properties
        public Caliber LookupCaliber { get; set; }
        public Unit CaliberUnit { get; set; }
        public Country CountryOfManufacture { get; set; }
        public ICollection<WeaponAccessory> WeaponAccessories { get; set; }
        #endregion
    }
}
