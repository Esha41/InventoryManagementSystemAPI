using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.Common.Dtos;
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
        /// Return tracking lines for a processed return (inventory detail or asset per row, with linked files).
        /// </summary>
        [HttpGet("{id}/tracking-lines")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize(
            "Permissions.Return.View", "Permissions.Return.Page",
            "Permissions.RequestReciever.View", "Permissions.RequestReciever.Page")]
        public async Task<IActionResult> GetTrackingLines(long id)
        {
            var result = await _returnService.GetReturnTrackingLinesAsync(id);
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
        /// Create a new return. Files may be sent as:
        /// - <paramref name="attachmentUploads"/> per AttachmentRequirement (indexed groups), plus optional <paramref name="OtherFiles"/>;
        /// - legacy <paramref name="files"/> only when structured attachmentUploads is absent/empty (all go to the other bucket).
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(APIOperationResponse<long>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.Return.Create")]
        public async Task<IActionResult> Create(
            [FromForm] CreateReturnDto dto,
            [FromForm] List<AttachmentUploadGroupDto>? attachmentUploads = null,
            [FromForm] List<IFormFile>? otherFiles = null,
            [FromForm] List<IFormFile>? files = null)
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

                var hasStructuredUploads = map.Count > 0;

                var mergedOther = new List<IFormFile>();
                if (otherFiles != null)
                {
                    mergedOther.AddRange(otherFiles.Where(f => f != null && f.Length > 0));
                }

                if (!hasStructuredUploads && files != null)
                {
                    mergedOther.AddRange(files.Where(f => f != null && f.Length > 0));
                }

                var result = await _returnService.CreateAsync(dto, map, mergedOther.Count > 0 ? mergedOther : null);
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

        /// <summary>
        /// Set or update the depot for a return
        /// </summary>
        [HttpPut("{id}/set-depot")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("SetReturnDepot")]
        public async Task<IActionResult> SetDepot(long id, [FromBody] SetReturnDepotDto dto)
        {
            var result = await _returnService.SetDepotAsync(id, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Set or update the delivery date for a return
        /// </summary>
        [HttpPut("{id}/set-delivery-date")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("SetReturnDeliveryDate")]
        public async Task<IActionResult> SetDeliveryDate(long id, [FromBody] SetReturnDeliveryDateDto dto)
        {
            var result = await _returnService.SetDeliveryDateAsync(id, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Process return items (ammo/explosive inventory update and weapon asset status update), then approve and close the return.
        /// Expects <c>multipart/form-data</c> (DTO fields + optional <c>files</c>).
        /// </summary>
        [HttpPut("{id}/process-items")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("ProcessReturnItems")]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> ProcessItems(
            long id,
            [FromForm] ProcessReturnItemsDto dto,
            [FromForm] List<IFormFile>? files = null)
        {
            var result = await _returnService.ProcessReturnItemsAsync(id, dto, files);
            return ProcessResponse(result);
        }
    }
}
