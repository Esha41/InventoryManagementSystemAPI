namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    public class CreateAssetSupplyAccessoryDto
    {
        public long AccessoryId { get; set; }
        public long DefaultQuantity { get; set; }
        public long SuppliedQuantity { get; set; }
    }
}
