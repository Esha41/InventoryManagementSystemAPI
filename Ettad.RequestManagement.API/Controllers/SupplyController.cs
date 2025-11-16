using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplyController : ApiControllerBase
    {
        private readonly ISupplyService _supplyService;

        public SupplyController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        /// <summary>
        /// Get supply suggestion for an order based on FEFO (First Expiry First Out) logic
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Supply suggestion with lot allocations</returns>
        [HttpGet("suggestion/{orderId}")]
        [ProducesResponseType(typeof(APIOperationResponse<OrderSupplySuggestionDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Supply.View", "Permissions.Order.View")]
        public async Task<IActionResult> GetSupplySuggestion(long orderId)
        {
            var result = await _supplyService.GetSupplySuggestionAsync(orderId);
            return ProcessResponse(result);
        }
    }
}

