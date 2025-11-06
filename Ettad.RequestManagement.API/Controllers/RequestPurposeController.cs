using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.RequestPurposes;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestPurposeController : ApiControllerBase
    {
        private readonly IRequestPurposeService _requestPurposeService;

        public RequestPurposeController(IRequestPurposeService requestPurposeService)
        {
            _requestPurposeService = requestPurposeService;
        }

        /// <summary>
        /// Create a new request purpose for Discard
        /// </summary>
        [HttpPost("discard")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.RequestPurpose.Create")]
        public async Task<IActionResult> CreateForDiscard([FromBody] CreateUpdateRequestPurposeDto dto)
        {
            var result = await _requestPurposeService.CreateForDiscardAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new request purpose for Return
        /// </summary>
        [HttpPost("return")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.RequestPurpose.Create")]
        public async Task<IActionResult> CreateForReturn([FromBody] CreateUpdateRequestPurposeDto dto)
        {
            var result = await _requestPurposeService.CreateForReturnAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new request purpose for Order
        /// </summary>
        [HttpPost("order")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.RequestPurpose.Create")]
        public async Task<IActionResult> CreateForOrder([FromBody] CreateUpdateRequestPurposeDto dto)
        {
            var result = await _requestPurposeService.CreateForOrderAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get request purpose by ID for Discard
        /// </summary>
        [HttpGet("discard/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestPurpose.View", "Permissions.RequestPurpose.Page")]
        public async Task<IActionResult> GetByIdForDiscard(long id)
        {
            var result = await _requestPurposeService.GetByIdForDiscardAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get request purpose by ID for Return
        /// </summary>
        [HttpGet("return/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestPurpose.View", "Permissions.RequestPurpose.Page")]
        public async Task<IActionResult> GetByIdForReturn(long id)
        {
            var result = await _requestPurposeService.GetByIdForReturnAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get request purpose by ID for Order
        /// </summary>
        [HttpGet("order/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestPurpose.View", "Permissions.RequestPurpose.Page")]
        public async Task<IActionResult> GetByIdForOrder(long id)
        {
            var result = await _requestPurposeService.GetByIdForOrderAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all request purposes for Discard
        /// </summary>
        [HttpGet("discard")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestPurpose.View", "Permissions.RequestPurpose.Page")]
        public async Task<IActionResult> GetAllForDiscard()
        {
            var result = await _requestPurposeService.GetAllForDiscardAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all request purposes for Return
        /// </summary>
        [HttpGet("return")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestPurpose.View", "Permissions.RequestPurpose.Page")]
        public async Task<IActionResult> GetAllForReturn()
        {
            var result = await _requestPurposeService.GetAllForReturnAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all request purposes for Order
        /// </summary>
        [HttpGet("order")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestPurpose.View", "Permissions.RequestPurpose.Page")]
        public async Task<IActionResult> GetAllForOrder()
        {
            var result = await _requestPurposeService.GetAllForOrderAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update an existing request purpose
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestPurpose.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateRequestPurposeDto dto)
        {
            var result = await _requestPurposeService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Soft delete a request purpose
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.RequestPurpose.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _requestPurposeService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

