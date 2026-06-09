namespace Ettad.Inventory.Service.WeaponAccessories.Dtos
{
    public class WeaponAccessoryDto
    {
        public long WeaponId { get; set; }
        public long AccessoryId { get; set; }
        public long DefaultQuantity { get; set; }
        public WeaponAccessoryItemSummaryDto? Accessory { get; set; }
        public WeaponAccessoryItemSummaryDto? Weapon { get; set; }
    }

    public class WeaponAccessoryItemSummaryDto
    {
        public long Id { get; set; }
        public string? ItemNo { get; set; }
        public string Name { get; set; }
        public string? NameAr { get; set; }
    }
}
