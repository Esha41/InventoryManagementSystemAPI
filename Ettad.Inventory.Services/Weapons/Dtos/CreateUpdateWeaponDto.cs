using Ettad.Data.Enums;
using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Weapons.Dtos
{
    public class CreateUpdateWeaponDto : CreateUpdateBaseItemDto
    {
        /// <summary>Small / Medium / Large — optional; defaults to Small on create when omitted.</summary>
        public WeaponCaliberCategory? CaliberCategory { get; set; }

        public string? Caliber { get; set; }
        public long? CaliberUnitId { get; set; }
        public int? YearOfManufacture { get; set; }
        public long? CountryOfManufactureId { get; set; }
        public string? Model { get; set; }
    }
}
