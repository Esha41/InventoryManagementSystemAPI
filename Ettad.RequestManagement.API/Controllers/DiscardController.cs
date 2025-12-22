using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.Discards;
using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;
using Ettad.ResponseHandler.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscardController : ApiControllerBase
    {
        private readonly IDiscardService _discardService;

        public DiscardController(IDiscardService discardService)
        {
            _discardService = discardService;
        }

        /// <summary>
        /// Get discard by ID with all details and navigation properties
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Discard.View", "Permissions.Discard.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _discardService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all discards with details and navigation properties
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Discard.View", "Permissions.Discard.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _discardService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new discard (Supports both Multipart/Form-Data and Application/JSON)
        /// </summary>
        /// <returns>Created discard ID</returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Discard.Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                CreateDiscardDto dto;
                List<IFormFile>? files = null;

                if (Request.HasFormContentType)
                {
                    // Handle Multipart/Form-Data
                    dto = new CreateDiscardDto();
                    await TryUpdateModelAsync(dto);
                    files = Request.Form.Files.ToList();
                }
                else
                {
                    // Handle Application/JSON
                    using var reader = new StreamReader(Request.Body);
                    var body = await reader.ReadToEndAsync();
                    dto = System.Text.Json.JsonSerializer.Deserialize<CreateDiscardDto>(body, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                if (dto == null)
                    return BadRequest(APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Invalid request data"));

                var result = files != null && files.Count > 0
                    ? await _discardService.CreateAsync(dto, files)
                    : await _discardService.CreateAsync(dto);

                return ProcessResponse(result);
            }
            catch (Exception ex)
            {
                return BadRequest(APIOperationResponse<long>.Fail(ResponseType.BadRequest, ex.Message));
            }
        }

        /// <summary>
        /// Change the priority of an existing discard
        /// </summary>
        [HttpPatch("{id}/priority")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Discard.Edit")]
        public async Task<IActionResult> ChangePriority(long id, [FromBody] RequestPriority priority)
        {
            var result = await _discardService.ChangePriorityAsync(id, priority);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Soft delete a discard
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Discard.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _discardService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

