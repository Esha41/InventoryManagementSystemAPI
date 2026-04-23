using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Weapon : BaseItem
    {
        /// <summary>Defaults to <see cref="WeaponCaliberCategory.Small"/> (1).</summary>
        public WeaponCaliberCategory CaliberCategory { get; set; } = WeaponCaliberCategory.Small;

        public string? Caliber { get; set; }
        public long? CaliberUnitId { get; set; }
        public int? YearOfManufacture { get; set; }
        public long? CountryOfManufactureId { get; set; }
        public string? Model { get; set; }

        #region Navigation Properties
        public Unit CaliberUnit { get; set; }
        public Country CountryOfManufacture { get; set; }
        #endregion
    }
}
