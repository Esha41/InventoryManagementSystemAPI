using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.AssetSupply;
using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
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
        /// Get available assets to supply for an order (FIFO order, with serial numbers, not assigned)
        /// </summary>
        /// <param name="orderId">The order ID</param>
        /// <param name="depotIds">Optional list of depot IDs to filter assets</param>
        [HttpGet("order/{orderId}/available-assets")]
        [ProducesResponseType(typeof(APIOperationResponse<OrderAssetsToSupplyDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page")]
        public async Task<IActionResult> GetAssetsToSupply(long orderId, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _assetSupplyService.GetAssetsToSupplyAsync(orderId, depotIds);
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
        /// Create and submit a new asset supply (creates assignments immediately)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.AssetSupply.Create")]
        public async Task<IActionResult> Create([FromBody] CreateAssetSupplyDto dto)
        {
            var result = await _assetSupplyService.CreateAndSubmitAsync(dto);
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
    }
}
