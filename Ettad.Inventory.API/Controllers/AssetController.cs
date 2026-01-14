using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Assets;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Net;
using Ettad.CrossCutting.Comman.Time;

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
            _dateTimeProvider = dateTimeProvider;
            
            // Set EPPlus license context
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
        public async Task<IActionResult> GetAll([FromQuery] long? depotId = null)
        {
            var result = await _assetService.GetAllAsync(depotId);
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
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> BulkCreate([FromBody] List<CreateAssetDto> dtos)
        {
            var result = await _assetService.CreateBulkAsync(dtos);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateAssetDto dto)
        {
            var result = await _assetService.UpdateAsync(id, dto);
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
        public async Task<IActionResult> Import(IFormFile file, [FromForm] long depotId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "File is required" });
            }

            if (depotId <= 0)
            {
                return BadRequest(new { message = "Valid depot ID is required" });
            }

            var result = await _assetService.ImportAsync(file, depotId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Preview asset import from Excel file (validation only, no data saved)
        /// </summary>
        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromForm] long depotId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "File is required" });
            }

            if (depotId <= 0)
            {
                return BadRequest(new { message = "Valid depot ID is required" });
            }

            var result = await _assetService.ImportPreviewAsync(file, depotId);
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
            try
            {
                var templateResult = await _assetService.GenerateImportTemplateAsync(depotId, language);
                
                if (!templateResult.Succeeded || templateResult.Data == null)
                {
                    return StatusCode(500, new { message = "Failed to generate template", errors = templateResult.Errors });
                }

                var fileName = $"Asset_Import_Template_Depot_{depotId}_{_dateTimeProvider.Now:yyyyMMdd}.xlsx";
                
                return File(
                    templateResult.Data,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while generating the template", error = ex.Message });
            }
        }
    }
}

