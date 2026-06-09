namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    public class WeaponAccessoryDefaultsDto
    {
        public Dictionary<long, List<WeaponAccessoryDefaultLineDto>> DefaultsByWeaponItemId { get; set; } = new();
    }

    public class WeaponAccessoryDefaultLineDto
    {
        public long AccessoryId { get; set; }
        public string? ItemNo { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameAr { get; set; }
        public long DefaultQuantity { get; set; }
    }
}
