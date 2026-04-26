using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Inventory.Service.AssetSupply.Interfaces
{
    public interface IAssetSupplyService
    {
        /// <summary>
        /// Get batches in the given depots that contain assets matching the order's requested items.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="depotIds">List of depot IDs to filter batches</param>
        Task<APIOperationResponse<List<BatchForOrderDepotDto>>> GetBatchesForOrderDepotsAsync(long orderId, List<long> depotIds);

        /// <summary>
        /// Get available assets to supply for an order.
        /// Returns assets ordered by FIFO (oldest first), with serial numbers only, and not already assigned.
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="depotIds">Optional list of depot IDs to filter assets. If provided, only assets from these depots will be returned.</param>
        /// <param name="batchIds">Optional list of batch IDs to filter assets. If provided, only assets from these batches will be returned.</param>
        Task<APIOperationResponse<OrderAssetsToSupplyDto>> GetAssetsToSupplyAsync(long orderId, List<long>? depotIds = null, List<long>? batchIds = null);

        /// <summary>
        /// Get asset supply by ID
        /// </summary>
        Task<APIOperationResponse<AssetSupplyDto>> GetByIdAsync(long id);

        /// <summary>
        /// Get asset supply by order ID
        /// </summary>
        Task<APIOperationResponse<AssetSupplyDto>> GetByOrderIdAsync(long orderId);

        /// <summary>
        /// Get all asset supplies
        /// </summary>
        Task<APIOperationResponse<List<AssetSupplyDto>>> GetAllAsync();

        /// <summary>
        /// Create and submit a new asset supply, including file attachments.
        /// </summary>
        Task<APIOperationResponse<long>> CreateAndSubmitAsync(CreateAssetSupplyDto dto, List<IFormFile> files);

        /// <summary>
        /// Cancel an asset supply
        /// </summary>
        Task<APIOperationResponse<bool>> CancelSupplyAsync(long id, string? reason);

        /// <summary>
        /// Return a single asset from assignment
        /// </summary>
        Task<APIOperationResponse<bool>> ReturnAssetAsync(ReturnAssetDto dto);

        /// <summary>
        /// Return multiple assets at once
        /// </summary>
        Task<APIOperationResponse<bool>> ReturnMultipleAssetsAsync(ReturnMultipleAssetsDto dto);

        /// <summary>
        /// Save depot and batch selections for weapon supply (replaces existing selections for the order).
        /// </summary>
        Task<APIOperationResponse<bool>> SaveWeaponSupplySelectionAsync(SaveWeaponSupplySelectionDto dto);

        /// <summary>
        /// Get saved depot and batch selections for weapon supply for an order.
        /// </summary>
        Task<APIOperationResponse<List<DepotBatchSelectionDto>>> GetWeaponSupplySelectionAsync(long orderId);

        /// <summary>
        /// Get the selected batches with their assets for weapon supply.
        /// Prioritizes assets with serial numbers; fills remaining quantity with non-serial assets.
        /// </summary>
        Task<APIOperationResponse<List<BatchDto>>> GetSelectedBatchesWithAssetsAsync(long orderId);
    }
}
