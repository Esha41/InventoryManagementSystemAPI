namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    /// <summary>Combined inventory monitoring payload for a single HTTP round-trip.</summary>
    public class InventoryDashboardSummaryDto
    {
        public WeaponAssetDashboardDto WeaponAssets { get; set; } = new();
        public InventoryPipelineDashboardDto Pipeline { get; set; } = new();
    }
}
