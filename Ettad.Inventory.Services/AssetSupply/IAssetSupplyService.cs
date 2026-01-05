using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.AssetSupply
{
    public interface IAssetSupplyService
    {
        /// <summary>
        /// Get available assets to supply for an order.
        /// Returns assets ordered by FIFO (oldest first), with serial numbers only, and not already assigned.
        /// </summary>
        Task<APIOperationResponse<OrderAssetsToSupplyDto>> GetAssetsToSupplyAsync(long orderId);

        /// <summary>
        /// Get asset supply by ID
        /// </summary>
        Task<APIOperationResponse<AssetSupplyDto>> GetByIdAsync(long id);

        /// <summary>
        /// Get asset supply by order ID
        /// </summary>
        Task<APIOperationResponse<AssetSupplyDto>> GetByOrderIdAsync(long orderId);

        /// <summary>
        /// Get draft asset supply by order ID (for editing)
        /// </summary>
        Task<APIOperationResponse<AssetSupplyDto>> GetDraftByOrderIdAsync(long orderId);

        /// <summary>
        /// Get all asset supplies
        /// </summary>
        Task<APIOperationResponse<List<AssetSupplyDto>>> GetAllAsync();

        /// <summary>
        /// Create a new asset supply
        /// </summary>
        Task<APIOperationResponse<long>> CreateAsync(CreateAssetSupplyDto dto);

        /// <summary>
        /// Update an existing asset supply (only while in Draft status)
        /// </summary>
        Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateAssetSupplyDto dto);

        /// <summary>
        /// Submit an asset supply for processing
        /// </summary>
        Task<APIOperationResponse<bool>> SubmitSupplyAsync(long id, SubmitAssetSupplyDto dto);

        /// <summary>
        /// Complete a supply - creates assignments for all assets
        /// </summary>
        Task<APIOperationResponse<bool>> CompleteSupplyAsync(long id);

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
    }
}

