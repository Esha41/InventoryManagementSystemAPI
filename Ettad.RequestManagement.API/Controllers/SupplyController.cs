using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
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

        /// <summary>
        /// Create a supply transaction and update inventory quantities
        /// </summary>
        /// <param name="dto">Supply creation data</param>
        /// <returns>Supply transaction ID</returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.Create")]
        public async Task<IActionResult> CreateSupply([FromBody] CreateSupplyDto dto)
        {
            var result = await _supplyService.CreateSupplyAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get supply history for an order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Supply history details</returns>
        [HttpGet("order/{orderId}")]
        [ProducesResponseType(typeof(APIOperationResponse<SupplyDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.View", "Permissions.Order.View")]
        public async Task<IActionResult> GetSupplyByOrderId(long orderId)
        {
            var result = await _supplyService.GetSupplyByOrderIdAsync(orderId);
            return ProcessResponse(result);
        }
    }
}

