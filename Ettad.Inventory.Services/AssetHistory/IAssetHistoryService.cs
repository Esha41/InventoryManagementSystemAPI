using Ettad.Data.Enums;
using Ettad.Inventory.Service.AssetHistory.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.AssetHistory
{
    public interface IAssetHistoryService
    {
        /// <summary>
        /// Record a history entry for an asset action
        /// </summary>
        Task RecordHistoryAsync(long assetId, AssetHistoryActionType actionType, AssetHistoryContext context);

        /// <summary>
        /// Get complete history for an asset
        /// </summary>
        Task<APIOperationResponse<List<AssetHistoryDto>>> GetAssetHistoryAsync(long assetId);

        /// <summary>
        /// Get history entries related to a specific order
        /// </summary>
        Task<APIOperationResponse<List<AssetHistoryDto>>> GetAssetHistoryByOrderAsync(long orderId);

        /// <summary>
        /// Get history entries related to a specific supply
        /// </summary>
        Task<APIOperationResponse<List<AssetHistoryDto>>> GetAssetHistoryBySupplyAsync(long supplyId);

        /// <summary>
        /// Get history entries by action type
        /// </summary>
        Task<APIOperationResponse<List<AssetHistoryDto>>> GetHistoryByActionTypeAsync(AssetHistoryActionType actionType, DateTime? fromDate = null, DateTime? toDate = null);
    }
}

