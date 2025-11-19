using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
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
        /// <param name="depotIds">Optional list of depot IDs to filter suggestions from specific depots</param>
        /// <returns>Supply suggestion with lot allocations</returns>
        [HttpGet("suggestion/{orderId}")]
        [ProducesResponseType(typeof(APIOperationResponse<OrderSupplySuggestionDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Supply.View", "Permissions.Order.View")]
        public async Task<IActionResult> GetSupplySuggestion(long orderId, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _supplyService.GetSupplySuggestionAsync(orderId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get supply by ID with all details and calculated properties
        /// </summary>
        /// <param name="id">Supply ID</param>
        /// <returns>Supply details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIOperationResponse<SupplyDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.View", "Permissions.Supply.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _supplyService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all supplies with details and calculated properties
        /// </summary>
        /// <returns>List of supplies</returns>
        [HttpGet]
        [ProducesResponseType(typeof(APIOperationResponse<List<SupplyDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Supply.View", "Permissions.Supply.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supplyService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new supply with draft status
        /// </summary>
        /// <param name="dto">Supply creation data</param>
        /// <returns>Created supply ID</returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Supply.Create")]
        public async Task<IActionResult> Create([FromBody] CreateSupplyDto dto)
        {
            var result = await _supplyService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update supply information (receiver info, notes, etc.) - does not update details
        /// </summary>
        /// <param name="id">Supply ID</param>
        /// <param name="dto">Supply update data</param>
        /// <returns>Success result</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.Edit")]
        public async Task<IActionResult> UpdateSupplyInfo(long id, [FromBody] UpdateSupplyDto dto)
        {
            var result = await _supplyService.UpdateSupplyInfoAsync(id, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Add a new supply detail to an existing supply
        /// </summary>
        /// <param name="supplyId">Supply ID</param>
        /// <param name="detailDto">Supply detail data</param>
        /// <returns>Created detail ID</returns>
        [HttpPost("{supplyId}/details")]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.Edit")]
        public async Task<IActionResult> AddSupplyDetail(long supplyId, [FromBody] CreateSupplyDetailDto detailDto)
        {
            var result = await _supplyService.AddSupplyDetailAsync(supplyId, detailDto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update an existing supply detail
        /// </summary>
        /// <param name="supplyId">Supply ID</param>
        /// <param name="detailId">Supply Detail ID</param>
        /// <param name="detailDto">Supply detail update data</param>
        /// <returns>Success result</returns>
        [HttpPut("{supplyId}/details/{detailId}")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.Edit")]
        public async Task<IActionResult> UpdateSupplyDetail(long supplyId, long detailId, [FromBody] UpdateSupplyDetailDto detailDto)
        {
            var result = await _supplyService.UpdateSupplyDetailAsync(supplyId, detailId, detailDto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Delete a supply detail (cannot delete if it's the last detail)
        /// </summary>
        /// <param name="supplyId">Supply ID</param>
        /// <param name="detailId">Supply Detail ID</param>
        /// <returns>Success result</returns>
        [HttpDelete("{supplyId}/details/{detailId}")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.Edit")]
        public async Task<IActionResult> DeleteSupplyDetail(long supplyId, long detailId)
        {
            var result = await _supplyService.DeleteSupplyDetailAsync(supplyId, detailId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update supply submission status (Draft to Submitted)
        /// </summary>
        /// <param name="id">Supply ID</param>
        /// <param name="newStatus">New submission status value</param>
        /// <returns>Success result</returns>
        [HttpPut("{id}/submission-status")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.Edit")]
        public async Task<IActionResult> UpdateSubmissionStatus(long id, [FromBody] SupplySubmissionStatus newStatus)
        {
            var result = await _supplyService.UpdateSubmissionStatusAsync(id, newStatus);
            return ProcessResponse(result);
        }

    }
}

