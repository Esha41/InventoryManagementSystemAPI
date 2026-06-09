namespace Ettad.Inventory.Service.WeaponAccessories.Dtos
{
    public class CreateUpdateWeaponAccessoryDto
    {
        public long WeaponId { get; set; }
        public long AccessoryId { get; set; }
        public long DefaultQuantity { get; set; }
    }
}
