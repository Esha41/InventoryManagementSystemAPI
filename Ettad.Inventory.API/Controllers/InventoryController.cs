using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Inventories;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ApiControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Get inventory by ID with all details and navigation properties
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _inventoryService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all inventories with details and navigation properties
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _inventoryService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new inventory with details
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Inventory.Create")]
        public async Task<IActionResult> Create([FromBody] CreateInventoryDto dto)
        {
            var result = await _inventoryService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update an existing inventory and its details
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateInventoryDto dto)
        {
            var result = await _inventoryService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Soft delete an inventory
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Inventory.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _inventoryService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all lots for a specific item with usage tracking
        /// </summary>
        [HttpGet("item/{itemId}/lots")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLotsByItemId(long itemId)
        {
            var result = await _inventoryService.GetLotsByItemIdAsync(itemId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get available lots for a specific item and quantity (excludes expired and empty lots)
        /// </summary>
        [HttpGet("item/{itemId}/available-lots")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetAvailableLotsForQuantity(long itemId, [FromQuery] long quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            var result = await _inventoryService.GetAvailableLotsForQuantityAsync(itemId, quantity);
            return ProcessResponse(result);
        }
    }
}

