using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.AssetHistory.Dtos;
using Ettad.Inventory.Service.AssetHistory.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    /// <summary>
    /// Controller for viewing asset history/audit trail
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetHistoryController : ApiControllerBase
    {
        private readonly IAssetHistoryService _historyService;

        public AssetHistoryController(IAssetHistoryService historyService)
        {
            _historyService = historyService;
        }

        /// <summary>
        /// Get complete history for an asset
        /// </summary>
        [HttpGet("asset/{assetId}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<AssetHistoryDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page", "WeaponAssetMasterPage")]
        public async Task<IActionResult> GetAssetHistory(long assetId)
        {
            var result = await _historyService.GetAssetHistoryAsync(assetId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get history entries related to a specific order
        /// </summary>
        [HttpGet("order/{orderId}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<AssetHistoryDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page")]
        public async Task<IActionResult> GetHistoryByOrder(long orderId)
        {
            var result = await _historyService.GetAssetHistoryByOrderAsync(orderId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get history entries related to a specific supply
        /// </summary>
        [HttpGet("supply/{supplyId}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<AssetHistoryDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AssetSupply.View", "Permissions.AssetSupply.Page")]
        public async Task<IActionResult> GetHistoryBySupply(long supplyId)
        {
            var result = await _historyService.GetAssetHistoryBySupplyAsync(supplyId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get history entries by action type with optional date range
        /// </summary>
        [HttpGet("action-type/{actionType}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<AssetHistoryDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page", "WeaponAssetMasterPage")]
        public async Task<IActionResult> GetHistoryByActionType(
            AssetHistoryActionType actionType,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var result = await _historyService.GetHistoryByActionTypeAsync(actionType, fromDate, toDate);
            return ProcessResponse(result);
        }
    }
}

