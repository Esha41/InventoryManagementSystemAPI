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
using System.Collections.Generic;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetController : ApiControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;

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

        [HttpPost("bulk-template")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> BulkCreateFromTemplate([FromBody] CreateBulkAssetsFromTemplateDto dto)
        {
            var result = await _assetService.CreateBulkFromTemplateAsync(dto);
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

