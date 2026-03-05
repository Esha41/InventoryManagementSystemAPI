namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// DTO for saving depot and batch selections for weapon supply.
    /// </summary>
    public class SaveWeaponSupplySelectionDto
    {
        public long OrderId { get; set; }
        public List<DepotBatchSelectionDto> Selections { get; set; } = new();
    }

    public class DepotBatchSelectionDto
    {
        public long DepotId { get; set; }
        public long BatchId { get; set; }
    }
}
