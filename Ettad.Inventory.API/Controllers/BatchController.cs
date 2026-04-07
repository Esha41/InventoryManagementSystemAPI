using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Batches;
using Ettad.Inventory.Service.Batches.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ettad.CrossCutting.Comman.Models;
using Ettad.ResponseHandler.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BatchController : ApiControllerBase
    {
        private readonly IBatchService _batchService;

        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [HttpGet("summary")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetSummary([FromQuery] long depotId)
        {
            var result = await _batchService.GetSummaryAsync(depotId);
            return ProcessResponse(result);
        }

        [HttpGet("by-number/{batchNumber}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetByBatchNumber(string batchNumber, [FromQuery] bool? serialNumberOnly = null, [FromQuery] int? quantity = null, [FromQuery] bool? filterByIsAssigned = null)
        {
            var result = await _batchService.GetByBatchNumberAsync(batchNumber, serialNumberOnly, quantity, filterByIsAssigned);
            return ProcessResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetById(long id, [FromQuery] bool? serialNumberOnly = null, [FromQuery] int? quantity = null, [FromQuery] bool? filterByIsAssigned = null)
        {
            var result = await _batchService.GetByIdAsync(id, serialNumberOnly, quantity, filterByIsAssigned);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetAll([FromQuery] long? depotId = null)
        {
            var result = await _batchService.GetAllAsync(depotId);
            return ProcessResponse(result);
        }

        [HttpPost("search")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> Search([FromQuery] long? depotId, [FromBody] PagedListRequest request)
        {
            var result = await _batchService.SearchAsync(depotId, request);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateBatchDto dto)
        {
            var result = await _batchService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpPut("{id}/assets")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> BulkUpdateAssets(long id, [FromForm] string dtoJson, [FromForm] List<IFormFile>? files = null)
        {
            if (string.IsNullOrWhiteSpace(dtoJson))
                return BadRequest("dtoJson is required");

            BulkUpdateBatchAssetsDto? dto;
            try
            {
                dto = System.Text.Json.JsonSerializer.Deserialize<BulkUpdateBatchAssetsDto>(dtoJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                });
            }
            catch (System.Text.Json.JsonException ex)
            {
                return BadRequest($"Invalid dtoJson payload: {ex.Message}");
            }

            if (dto == null)
                return BadRequest("Invalid dtoJson payload");

            var result = await _batchService.BulkUpdateAssetsAsync(id, dto, files);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Asset.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _batchService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        [HttpDelete("{batchId}/assets/{assetId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> RemoveAssetFromBatch(long batchId, long assetId)
        {
            var result = await _batchService.RemoveAssetFromBatchAsync(batchId, assetId);
            return ProcessResponse(result);
        }
    }
}
