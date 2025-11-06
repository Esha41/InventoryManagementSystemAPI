using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.Orders;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
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

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
        /// Create a new order
        /// </summary>
        /// <param name="dto">Order creation data</param>
        /// <returns>Created order ID</returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Order.Create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var result = await _orderService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update an existing order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="dto">Order update data</param>
        /// <returns>Success result</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Order.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateOrderDto dto)
        {
            var result = await _orderService.UpdateAsync(id, dto);
            return ProcessResponse(result);
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
    }
}

