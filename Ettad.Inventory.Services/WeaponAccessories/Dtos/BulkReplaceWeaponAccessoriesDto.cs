namespace Ettad.Inventory.Service.WeaponAccessories.Dtos
{
    public class BulkReplaceWeaponAccessoriesDto
    {
        public long WeaponId { get; set; }
        public List<WeaponAccessoryQuantityDto> Accessories { get; set; } = new();
    }

    public class WeaponAccessoryQuantityDto
    {
        public long AccessoryId { get; set; }
        public long DefaultQuantity { get; set; }
    }
}
