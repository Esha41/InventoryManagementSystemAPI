using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring.Interfaces
{
    public interface IInventoryDashboardMonitoringService
    {
        Task<APIOperationResponse<WeaponAssetDashboardDto>> GetWeaponAssetDashboardAsync(long? depotId = null, List<long>? depotIds = null);

        Task<APIOperationResponse<InventoryPipelineDashboardDto>> GetPipelineDashboardAsync(long? depotId = null, List<long>? depotIds = null);

        Task<APIOperationResponse<InventoryDashboardSummaryDto>> GetInventoryDashboardSummaryAsync(long? depotId = null, List<long>? depotIds = null);

        Task<APIOperationResponse<List<DraftSupplyListItemDto>>> GetDraftSuppliesListAsync(long? depotId = null, List<long>? depotIds = null);

        Task<APIOperationResponse<List<OrderAwaitingFulfillmentListItemDto>>> GetOrdersAwaitingFulfillmentListAsync(long? depotId = null, List<long>? depotIds = null);
    }
}
