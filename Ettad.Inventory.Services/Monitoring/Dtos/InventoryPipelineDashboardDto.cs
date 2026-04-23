using Ettad.Inventory.Service.Monitoring.Services;

namespace Ettad.Inventory.Service.Monitoring.Dtos
{
    /// <summary>
    /// Supply / order pipeline headline metrics (see <see cref="InventoryDashboardMonitoringService"/> for rules).
    /// </summary>
    public class InventoryPipelineDashboardDto
    {
        /// <summary>Draft <see cref="Ettad.Data.Entities.Supply"/> + draft <see cref="Ettad.Data.Entities.AssetSupply"/> rows in scope.</summary>
        public int DraftSupplyCount { get; set; }
        /// <summary>Approved orders not fully fulfilled via Supply / AssetSupply (see service for definition).</summary>
        public int OrdersAwaitingFulfillmentCount { get; set; }
    }
}
