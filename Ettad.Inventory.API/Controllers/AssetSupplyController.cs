using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.Inventory.Service.AssetSupply.Interfaces;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    /// <summary>
    /// Controller for managing asset supply operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetSupplyController : ApiControllerBase
    {
        private readonly IAssetSupplyService _assetSupplyService;

        public AssetSupplyController(IAssetSupplyService assetSupplyService)
        {
            _assetSupplyService = assetSupplyService;
        }

        /// <summary>
        /// Get saved depot and batch selections for weapon supply.
        /// </summary>
        [HttpGet("order/{orderId}/selection")]
        [ProducesResponseType(typeof(APIOperationResponse<List<DepotBatchSelectionDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page", "SelectDepots")]
        public async Task<IActionResult> GetWeaponSupplySelection(long orderId)
        {
            var result = await _assetSupplyService.GetWeaponSupplySelectionAsync(orderId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get batches in the given depots that contain assets matching the order's requested items.
        /// </summary>
        /// <summary>
        /// Get depot IDs that have ready-to-issue assets matching the order's requested items.
        /// </summary>
        [HttpGet("order/{orderId}/depots-with-available-items")]
        [ProducesResponseType(typeof(APIOperationResponse<List<long>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page", "SelectDepots")]
        public async Task<IActionResult> GetDepotsWithAvailableItems(long orderId)
        {
            var result = await _assetSupplyService.GetDepotsWithAvailableItemsAsync(orderId);
            return ProcessResponse(result);
        }

        [HttpGet("order/{orderId}/batches-for-depots")]
        [ProducesResponseType(typeof(APIOperationResponse<List<BatchForOrderDepotDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page", "SelectDepots")]
        public async Task<IActionResult> GetBatchesForOrderDepots(long orderId, [FromQuery] List<long> depotIds)
        {
            if (depotIds == null || !depotIds.Any())
                return BadRequest("At least one depot ID is required");
            var result = await _assetSupplyService.GetBatchesForOrderDepotsAsync(orderId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get available assets to supply for an order (FIFO order, with serial numbers, not assigned)
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="depotIds">Optional list of depot IDs to filter assets</param>
        /// <param name="batchIds">Optional list of batch IDs to filter assets (when provided, only assets from these batches are returned)</param>
        [HttpGet("order/{orderId}/available-assets")]
        [ProducesResponseType(typeof(APIOperationResponse<OrderAssetsToSupplyDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page")]
        public async Task<IActionResult> GetAssetsToSupply(long orderId, [FromQuery] List<long>? depotIds = null, [FromQuery] List<long>? batchIds = null)
        {
            var result = await _assetSupplyService.GetAssetsToSupplyAsync(orderId, depotIds, batchIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get asset supply by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIOperationResponse<AssetSupplyDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _assetSupplyService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get asset supply by order ID
        /// </summary>
        [HttpGet("order/{orderId}")]
        [ProducesResponseType(typeof(APIOperationResponse<AssetSupplyDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page")]
        public async Task<IActionResult> GetByOrderId(long orderId)
        {
            var result = await _assetSupplyService.GetByOrderIdAsync(orderId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all asset supplies
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(APIOperationResponse<List<AssetSupplyDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _assetSupplyService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create and submit a new asset supply (creates assignments immediately) with file attachments.
        /// Uses multipart/form-data similar to SupplyController.Submit.
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.AssetSupply.Create")]
        public async Task<IActionResult> Create([FromForm] CreateAssetSupplyDto dto, [FromForm] List<IFormFile> files)
        {
            var result = await _assetSupplyService.CreateAndSubmitAsync(dto, files);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Cancel an asset supply
        /// </summary>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.Edit")]
        public async Task<IActionResult> Cancel(long id, [FromBody] string? reason)
        {
            var result = await _assetSupplyService.CancelSupplyAsync(id, reason);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Return a single asset from assignment
        /// </summary>
        [HttpPost("return")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.Edit")]
        public async Task<IActionResult> ReturnAsset([FromBody] ReturnAssetDto dto)
        {
            var result = await _assetSupplyService.ReturnAssetAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Return multiple assets at once
        /// </summary>
        [HttpPost("return-multiple")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.Edit")]
        public async Task<IActionResult> ReturnMultipleAssets([FromBody] ReturnMultipleAssetsDto dto)
        {
            var result = await _assetSupplyService.ReturnMultipleAssetsAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get selected batches with their assets for weapon supply.
        /// Assets with serial numbers are returned first; remaining quantity is filled with non-serial assets.
        /// </summary>
        [HttpGet("order/{orderId}/selected-batches")]
        [ProducesResponseType(typeof(APIOperationResponse<List<BatchDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page", "SelectDepots")]
        public async Task<IActionResult> GetSelectedBatchesWithAssets(long orderId)
        {
            var result = await _assetSupplyService.GetSelectedBatchesWithAssetsAsync(orderId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Save depot and batch selections for weapon supply.
        /// </summary>
        [HttpPost("order/{orderId}/save-selection")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("SelectDepots")]
        public async Task<IActionResult> SaveWeaponSupplySelection(long orderId, [FromBody] SaveWeaponSupplySelectionDto dto)
        {
            if (dto == null)
                dto = new SaveWeaponSupplySelectionDto { OrderId = orderId };
            dto.OrderId = orderId;
            var result = await _assetSupplyService.SaveWeaponSupplySelectionAsync(dto);
            return ProcessResponse(result);
        }
    }
}
