using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Weapons.Dtos
{
    public class CreateUpdateWeaponDto : CreateUpdateBaseItemDto
    {
        public string? Caliber { get; set; }
        public long? CaliberUnitId { get; set; }
        public int? YearOfManufacture { get; set; }
        public long? CountryOfManufactureId { get; set; }
        public string? Model { get; set; }
    }
}
