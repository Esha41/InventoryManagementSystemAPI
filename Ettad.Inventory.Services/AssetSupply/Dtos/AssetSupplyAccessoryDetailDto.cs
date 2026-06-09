namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    public class AssetSupplyAccessoryDetailDto
    {
        public long AccessoryId { get; set; }
        public string? ItemNo { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public long DefaultQuantity { get; set; }
        public long SuppliedQuantity { get; set; }
    }
}
