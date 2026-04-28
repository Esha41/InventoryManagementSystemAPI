using Ettad.Data.Enums;
using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Weapons.Dtos
{
    public class WeaponDto : BaseItemDto
    {
        public WeaponCaliberCategory CaliberCategory { get; set; } = WeaponCaliberCategory.Small;

        public long? CaliberId { get; set; }
        public long? CaliberUnitId { get; set; }
        public int? YearOfManufacture { get; set; }
        public long? CountryOfManufactureId { get; set; }
        public string? Model { get; set; }

        #region Navigation Properties
        public CaliberDto Caliber { get; set; }
        public UnitDto CaliberUnit { get; set; }
        public CountryDto CountryOfManufacture { get; set; }
        #endregion
    }
}
