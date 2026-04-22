using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Interfaces.Services;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderItemTrackingController : ApiControllerBase
    {
        private readonly IOrderItemTrackingService _orderItemTrackingService;

        public OrderItemTrackingController(IOrderItemTrackingService orderItemTrackingService)
        {
            _orderItemTrackingService = orderItemTrackingService;
        }

        /// <summary>
        /// Get all history for an order (by orderId or RequestNo)
        /// </summary>
        /// <param name="orderId">Order ID (optional)</param>
        /// <param name="requestNo">Request Number (optional)</param>
        /// <returns>List of order item history records</returns>
        [HttpGet("history")]
        [ProducesResponseType(typeof(APIOperationResponse<List<OrderItemHistoryDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Order.View", "Permissions.Order.Page")]
        public async Task<IActionResult> GetOrderItemHistory([FromQuery] long? orderId = null, [FromQuery] string? requestNo = null)
        {
            var result = await _orderItemTrackingService.GetOrderItemHistoryAsync(orderId, requestNo);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get history for a specific item in an order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="itemId">Item ID</param>
        /// <returns>List of item history records</returns>
        [HttpGet("history/{orderId}/items/{itemId}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<OrderItemHistoryDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Order.View", "Permissions.Order.Page")]
        public async Task<IActionResult> GetItemHistory(long orderId, long itemId)
        {
            var result = await _orderItemTrackingService.GetItemHistoryAsync(orderId, itemId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get final approved quantities snapshot (by orderId or RequestNo)
        /// </summary>
        /// <param name="orderId">Order ID (optional)</param>
        /// <param name="requestNo">Request Number (optional)</param>
        /// <returns>List of approved quantity history records</returns>
        [HttpGet("approved-quantities")]
        [ProducesResponseType(typeof(APIOperationResponse<List<OrderItemHistoryDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Order.View", "Permissions.Order.Page")]
        public async Task<IActionResult> GetApprovedQuantities([FromQuery] long? orderId = null, [FromQuery] string? requestNo = null)
        {
            var result = await _orderItemTrackingService.GetApprovedQuantitiesAsync(orderId, requestNo);
            return ProcessResponse(result);
        }
    }
}
