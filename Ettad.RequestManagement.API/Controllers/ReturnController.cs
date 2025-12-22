using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.Returns;
using Ettad.RequestManagement.Service.Returns.Dtos;
using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ettad.ResponseHandler.Consts;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReturnController : ApiControllerBase
    {
        private readonly IReturnService _returnService;

        public ReturnController(IReturnService returnService)
        {
            _returnService = returnService;
        }

        /// <summary>
        /// Get return by ID with all details and navigation properties
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Return.View", "Permissions.Return.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _returnService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all returns with details and navigation properties
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Return.View", "Permissions.Return.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _returnService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new return (Supports both Multipart/Form-Data and Application/JSON)
        /// </summary>
        /// <returns>Created return ID</returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Return.Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                CreateReturnDto dto;
                List<IFormFile>? files = null;

                if (Request.HasFormContentType)
                {
                    // Handle Multipart/Form-Data
                    dto = new CreateReturnDto();
                    await TryUpdateModelAsync(dto);
                    files = Request.Form.Files.ToList();
                }
                else
                {
                    // Handle Application/JSON
                    using var reader = new StreamReader(Request.Body);
                    var body = await reader.ReadToEndAsync();
                    dto = System.Text.Json.JsonSerializer.Deserialize<CreateReturnDto>(body, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                if (dto == null)
                    return BadRequest(APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Invalid request data"));

                var result = files != null && files.Count > 0
                    ? await _returnService.CreateAsync(dto, files)
                    : await _returnService.CreateAsync(dto);

                return ProcessResponse(result);
            }
            catch (Exception ex)
            {
                return BadRequest(APIOperationResponse<long>.Fail(ResponseType.BadRequest, ex.Message));
            }
        }

        /// <summary>
        /// Change the priority of an existing return
        /// </summary>
        [HttpPatch("{id}/priority")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Return.Edit")]
        public async Task<IActionResult> ChangePriority(long id, [FromBody] RequestPriority priority)
        {
            var result = await _returnService.ChangePriorityAsync(id, priority);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Soft delete a return
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Return.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _returnService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

