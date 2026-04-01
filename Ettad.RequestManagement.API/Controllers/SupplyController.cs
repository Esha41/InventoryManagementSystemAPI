using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
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
    public class SupplyController : ApiControllerBase
    {
        private readonly ISupplyService _supplyService;
        private readonly IWorkflowSupplySummaryService _workflowSupplySummaryService;

        public SupplyController(ISupplyService supplyService, IWorkflowSupplySummaryService workflowSupplySummaryService)
        {
            _supplyService = supplyService;
            _workflowSupplySummaryService = workflowSupplySummaryService;
        }

        /// <summary>
        /// Read-only workflow supply summary for order requests. Reflects current supply/pickup data while the order is in progress (provisional) and when approved (final). Rejected/cancelled orders are not returned. Requires ViewWorkflowSupplySummary; SuperAdmin bypasses via CheckAuthorize.
        /// </summary>
        [HttpGet("{orderId:long}/workflow-summary")]
        [ProducesResponseType(typeof(APIOperationResponse<WorkflowSupplySummaryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("ViewWorkflowSupplySummary")]
        public async Task<IActionResult> GetWorkflowSupplySummary(long orderId)
        {
            var result = await _workflowSupplySummaryService.GetSummaryForOrderAsync(orderId);
            return ProcessResponse(result);
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
        /// <param name="orderId">Order ID</param>
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
        /// Get supply by Order ID with all details and calculated properties
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Supply details</returns>
        [HttpGet("{orderId}/getByOrderId")]
        [ProducesResponseType(typeof(APIOperationResponse<SupplyDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.View", "Permissions.Supply.Page")]
        public async Task<IActionResult> GetByOrderId(long orderId)
        {
            var result = await _supplyService.GetByOrderIdAsync(orderId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get draft supply by Order ID
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>Draft Supply details or null if not found</returns>
        [HttpGet("{orderId}/draft")]
        [ProducesResponseType(typeof(APIOperationResponse<SupplyDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Supply.View", "Permissions.Supply.Page")]
        public async Task<IActionResult> GetDraftByOrderId(long orderId)
        {
            var result = await _supplyService.GetDraftByOrderIdAsync(orderId);
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
        /// Replace all supply details with new ones in a single atomic operation
        /// </summary>
        /// <param name="supplyId">Supply ID</param>
        /// <param name="details">List of new supply details</param>
        /// <returns>Success result</returns>
        [HttpPut("{supplyId}/details")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.Supply.Edit")]
        public async Task<IActionResult> ReplaceSupplyDetails(long supplyId, [FromBody] List<CreateSupplyDetailDto> details)
        {
            var result = await _supplyService.ReplaceSupplyDetailsAsync(supplyId, details);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Submit a supply (requires receiver information and at least one file attachment)
        /// </summary>
        /// <param name="id">Supply ID</param>
        /// <param name="dto">Submission data</param>
        /// <param name="files">File attachments (at least one required)</param>
        /// <returns>Success result</returns>
        [HttpPost("{id}/submit")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("SubmitSupply")]
        public async Task<IActionResult> Submit(long id, [FromForm] SubmitSupplyDto dto, [FromForm] List<IFormFile> files)
        {
            var result = await _supplyService.SubmitSupplyAsync(id, dto, files);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Set supply pickup date
        /// </summary>
        /// <param name="id">Supply ID</param>
        /// <param name="dto">Supply pickup date data</param>
        /// <returns>Success result</returns>
        [HttpPut("order/{orderId}/set-pickup-date")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("SetSupplyPickupDate")]
        public async Task<IActionResult> SetSupplyPickupDate(long orderId, [FromBody] SetSupplyPickupDateDto dto)
        {
            var result = await _supplyService.SetSupplyPickupDateAsync(orderId, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Confirm supply pickup date
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="dto">Supply pickup date confirmation data</param>
        /// <returns>Success result</returns>
        [HttpPut("order/{orderId}/confirm-pickup-date")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("ConfirmSupplyPickupDate")]
        public async Task<IActionResult> ConfirmSupplyPickupDate(long orderId, [FromBody] ConfirmSupplyPickupDateDto dto)
        {
            var result = await _supplyService.ConfirmSupplyPickupDateAsync(orderId, dto);
            return ProcessResponse(result);
        }

    }
}

