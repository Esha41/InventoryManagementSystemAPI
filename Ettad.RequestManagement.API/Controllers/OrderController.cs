using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Orders;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPermissionService _permissionService;

        public OrderController(IOrderService orderService, IPermissionService permissionService)
        {
            _orderService = orderService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIOperationResponse<OrderDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Order.View", "Permissions.Order.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _orderService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>List of orders</returns>
        [HttpGet]
        [ProducesResponseType(typeof(APIOperationResponse<List<OrderDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Order.View", "Permissions.Order.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new order.
        /// Files are submitted in two buckets:
        /// - <paramref name="attachmentUploads"/> contains per-AttachmentRequirement groups
        ///   (each item has an AttachmentRequirementId + one or more Files).
        /// - <paramref name="otherFiles"/> is the optional entity-only bucket for files that
        ///   are not bound to any requirement.
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Order.Create")]
        public async Task<IActionResult> Create(
            [FromForm] CreateOrderDto dto,
            [FromForm] List<AttachmentUploadGroupDto>? attachmentUploads = null,
            [FromForm] List<IFormFile>? otherFiles = null)
        {
            try
            {
                var map = (attachmentUploads ?? new List<AttachmentUploadGroupDto>())
                    .Where(g => g.AttachmentRequirementId > 0 && g.Files != null)
                    .GroupBy(g => g.AttachmentRequirementId)
                    .ToDictionary(
                        grp => grp.Key,
                        grp => (IReadOnlyList<IFormFile>)grp
                            .SelectMany(g => g.Files ?? new List<IFormFile>())
                            .Where(f => f != null && f.Length > 0)
                            .ToList());

                var result = await _orderService.CreateAsync(dto, map, otherFiles);
                return ProcessResponse(result);
            }
            catch (Exception ex)
            {
                return BadRequest(APIOperationResponse<long>.Fail(ResponseType.BadRequest, ex.Message));
            }
        }

        /// <summary>
        /// Delete an order (soft delete)
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Success result</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Order.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _orderService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Add a new item to an existing order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="itemDto">Item data to add</param>
        /// <returns>Created item ID</returns>
        [HttpPost("{orderId}/items")]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Order.Create", "Permissions.Order.Edit")]
        public async Task<IActionResult> AddOrderItem(long orderId, [FromBody] CreateUpdateRequestItemDto itemDto)
        {
            var result = await _orderService.AddOrderItemAsync(orderId, itemDto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update the quantity of an existing order item
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="itemId">Request Item ID</param>
        /// <param name="newQuantity">New quantity value</param>
        /// <returns>Success result</returns>
        [HttpPut("{orderId}/items/{itemId}/quantity")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Order.Edit")]
        public async Task<IActionResult> UpdateOrderItemQuantity(long orderId, long itemId, [FromBody] long newQuantity)
        {
            var oldQuantity = await _orderService.GetOrderItemCurrentQuantityAsync(orderId, itemId);
            if (oldQuantity == null)
                return NotFound(APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order item not found"));

            // Edit is required (enforced by CheckAuthorize); IncreaseQuantity/DecreaseQuantity control which operation is allowed
            if (newQuantity > oldQuantity)
            {
                var hasIncrease = await _permissionService.HasPermissionAsync("Permissions.Order.IncreaseQuantity")
                    || await _permissionService.HasPermissionAsync("OrderIncreaseQuantity");
                if (!hasIncrease)
                    return StatusCode((int)HttpStatusCode.Forbidden, APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have permission to increase quantity"));
            }
            else if (newQuantity < oldQuantity)
            {
                var hasDecrease = await _permissionService.HasPermissionAsync("Permissions.Order.DecreaseQuantity")
                    || await _permissionService.HasPermissionAsync("OrderDecreaseQuantity");
                if (!hasDecrease)
                    return StatusCode((int)HttpStatusCode.Forbidden, APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have permission to decrease quantity"));
            }

            var result = await _orderService.UpdateOrderItemQuantityAsync(orderId, itemId, newQuantity);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Delete an item from an order (cannot delete if it's the last item)
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="itemId">Request Item ID</param>
        /// <returns>Success result</returns>
        [HttpDelete("{orderId}/items/{itemId}")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Order.Edit", "Permissions.Order.Delete")]
        public async Task<IActionResult> DeleteOrderItem(long orderId, long itemId)
        {
            var result = await _orderService.DeleteOrderItemAsync(orderId, itemId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Verify if an item can be fulfilled from department's allowance
        /// </summary>
        /// <param name="itemId">Item ID to verify</param>
        /// <param name="requestedQuantity">Requested quantity</param>
        /// <returns>Allowance verification details</returns>
        [HttpGet("verify-allowance")]
        [ProducesResponseType(typeof(APIOperationResponse<AllowanceVerificationDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Order.View", "Permissions.Order.Create")]
        public async Task<IActionResult> VerifyItemAllowance([FromQuery] long itemId, [FromQuery] long requestedQuantity)
        {
            var result = await _orderService.VerifyItemAllowanceAsync(itemId, requestedQuantity);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Set pickup date for an order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="dto">Pickup date data</param>
        /// <returns>Success result</returns>
        [HttpPut("{id}/set-pickup-date")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("SetSupplyPickupDate", "ConfirmSupplyPickupDate")]
        public async Task<IActionResult> SetPickupDate(long id, [FromBody] SetPickupDateDto dto)
        {
            var result = await _orderService.SetSupplyDateAsync(id, dto.PickupDate);
            return ProcessResponse(result);
        }
    }
}
