using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Net;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ettad.ResponseHandler.Consts;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetController : ApiControllerBase
    {
        private readonly IAssetService _assetService;
        private readonly IBulkAssetDeleteService _bulkAssetDeleteService;

        public AssetController(IAssetService assetService, IBulkAssetDeleteService bulkAssetDeleteService)
        {
            _assetService = assetService;
            _bulkAssetDeleteService = bulkAssetDeleteService;

            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _assetService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get asset by serial number
        /// </summary>
        [HttpGet("serial/{serialNumber}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetBySerialNumber(string serialNumber)
        {
            var result = await _assetService.GetBySerialNumberAsync(serialNumber);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetAll([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _assetService.GetAllAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all assets belonging to a specific catalog item, optionally scoped to one depot.
        /// Used by the inventory dashboard to show individual weapon records in the expanded accordion.
        /// </summary>
        [HttpGet("item/{itemId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetAssetsByItemId(long itemId, [FromQuery] long? depotId = null)
        {
            var result = await _assetService.GetAssetsByItemIdAsync(itemId, depotId);
            return ProcessResponse(result);
        }

        [HttpPost("search")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> Search([FromQuery] long? depotId, [FromBody] PagedListRequest request)
        {
            var result = await _assetService.GetAssetsPaginatedAsync(depotId, request);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> Create([FromForm] CreateAssetDto dto, [FromForm] List<IFormFile>? files = null)
        {
            var result = await _assetService.CreateAsync(dto, files);
            return ProcessResponse(result);
        }
        
        [HttpPost("Bulk")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> BulkCreate([FromForm] string dtosJson, [FromForm] List<IFormFile>? files = null)
        {
            var result = await _assetService.CreateBulkAsync(dtosJson, files);
            return ProcessResponse(result);
        }

        //[HttpPost("bulk-template")]
        //[Consumes("application/json")]
        //[ProducesResponseType((int)HttpStatusCode.Created)]
        //[CheckAuthorize("Permissions.Asset.Create")]
        //public async Task<IActionResult> BulkCreateFromTemplate([FromBody] CreateBulkAssetsFromTemplateDto dto)
        //{
        //    var result = await _assetService.CreateBulkFromTemplateAsync(dto);
        //    return ProcessResponse(result);
        //}

        [HttpPost("bulk-template")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> BulkCreateFromTemplateWithFiles([FromForm] string dtoJson, [FromForm] List<IFormFile>? files = null)
        {
            CreateBulkAssetsFromTemplateDto? dto;
            try
            {
                dto = JsonSerializer.Deserialize<CreateBulkAssetsFromTemplateDto>(dtoJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                return ProcessResponse(APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(
                    ResponseType.BadRequest,
                    $"Invalid dtoJson payload: {ex.Message}"));
            }

            if (dto == null)
            {
                return ProcessResponse(APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(
                    ResponseType.BadRequest,
                    "dtoJson is required"));
            }

            var result = await _assetService.CreateBulkFromTemplateAsync(dto, files);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> Update(long id, [FromForm] UpdateAssetDto dto, [FromForm] List<IFormFile>? files = null)
        {
            var result = await _assetService.UpdateAsync(id, dto, files);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update serial number for an existing asset
        /// </summary>
        [HttpPut("{assetId}/serial-number")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> UpdateSerialNumber(long assetId, [FromBody] UpdateSerialNumberDto? dto)
        {
            var serialNumber = dto?.SerialNumber;
            var result = await _assetService.UpdateSerialNumberAsync(assetId, serialNumber);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Asset.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _assetService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Starts an async chunked soft-delete for large asset sets (Hangfire-backed).
        /// </summary>
        [HttpPost("bulk-delete")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [CheckAuthorize("Permissions.Asset.Delete")]
        public async Task<IActionResult> StartBulkDeleteAssets([FromBody] StartBulkDeleteAssetsDto dto)
        {
            var result = await _bulkAssetDeleteService.StartBulkDeleteAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Poll bulk-delete progress (creator-only).
        /// </summary>
        [HttpGet("bulk-delete/{jobId:guid}/status")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Delete")]
        public async Task<IActionResult> GetBulkDeleteAssetsStatus(Guid jobId)
        {
            var result = await _bulkAssetDeleteService.GetBulkDeleteStatusAsync(jobId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Import assets from Excel file
        /// </summary>
        [HttpPost("Import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> Import(IFormFile file, [FromForm] long depotId, [FromQuery] string language = "en")
        {
            var result = await _assetService.ImportAsync(file, depotId, language);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Preview asset import from Excel file (validation only, no data saved)
        /// </summary>
        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromForm] long depotId, [FromQuery] string language = "en")
        {
            var result = await _assetService.ImportPreviewAsync(file, depotId, language);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Generate asset import template with Excel data validation (dropdowns for lookups)
        /// </summary>
        [HttpGet("template")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> GenerateImportTemplate([FromQuery] long depotId, [FromQuery] string language = "en")
        {
            var templateResult = await _assetService.GenerateImportTemplateAsync(depotId, language);
            if (!templateResult.Succeeded || templateResult.Data == null)
                return ProcessResponse(templateResult);

            var fileName = $"Asset_Import_Template_Depot_{depotId}_{DateTime.UtcNow:yyyyMMdd}.xlsx";
            return File(
                templateResult.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}

