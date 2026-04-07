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
        public async Task<IActionResult> GetByBatchNumber(
            string batchNumber,
            [FromQuery] long? depotId = null,
            [FromQuery] bool? serialNumberOnly = null,
            [FromQuery] bool? filterByIsAssigned = null,
            [FromQuery] int assetsPage = 1,
            [FromQuery] int assetsPageSize = 50,
            [FromQuery] bool includeAllAssets = false)
        {
            var result = await _batchService.GetByBatchNumberAsync(batchNumber, depotId, serialNumberOnly, filterByIsAssigned, assetsPage, assetsPageSize, includeAllAssets);
            return ProcessResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetById(
            long id,
            [FromQuery] bool? serialNumberOnly = null,
            [FromQuery] bool? filterByIsAssigned = null,
            [FromQuery] int assetsPage = 1,
            [FromQuery] int assetsPageSize = 50,
            [FromQuery] bool includeAllAssets = false)
        {
            var result = await _batchService.GetByIdAsync(id, serialNumberOnly, filterByIsAssigned, assetsPage, assetsPageSize, includeAllAssets);
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
        public async Task<IActionResult> UpdateBatch(long id, [FromBody] UpdateBatchDto dto)
        {
            var result = await _batchService.UpdateBatchAsync(id, dto);
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

        /// <summary>Export all assets in this batch as an Excel file (same columns as batch import).</summary>
        [HttpGet("{id}/assets/export")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> ExportBatchAssets(long id, [FromQuery] string language = "en")
        {
            var result = await _batchService.ExportBatchAssetsExcelAsync(id, language);
            if (!result.Succeeded || result.Data == null || result.Data.Length == 0)
                return ProcessResponse(result);

            var fileName = $"Batch_{id}_Assets_{DateTime.UtcNow:yyyyMMdd}.xlsx";
            return File(
                result.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        /// <summary>Validate batch asset Excel updates without saving.</summary>
        [HttpPost("{id}/assets/import-preview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> ImportBatchAssetsPreview(IFormFile file, long id, [FromQuery] string language = "en")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is required" });

            var result = await _batchService.ImportBatchAssetsPreviewAsync(id, file, language);
            return ProcessResponse(result);
        }

        /// <summary>Apply batch asset updates from Excel.</summary>
        [HttpPost("{id}/assets/import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> ImportBatchAssets(IFormFile file, long id, [FromQuery] string language = "en")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is required" });

            var result = await _batchService.ImportBatchAssetsAsync(id, file, language);
            return ProcessResponse(result);
        }
    }
}
