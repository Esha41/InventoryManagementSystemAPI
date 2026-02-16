using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Net;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models; 

namespace Ettad.Inventory.API.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AmmunitionController : ApiControllerBase
    {
        private readonly IAmmunitionService _ammunitionService;
        private readonly ILogger<AmmunitionController> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AmmunitionController(
            IAmmunitionService ammunitionService,
            ILogger<AmmunitionController> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _ammunitionService = ammunitionService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            
            // Set EPPlus license context
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
        }


        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.View", "Permissions.Ammunition.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _ammunitionService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.View", "Permissions.Ammunition.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ammunitionService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpPost("Paginated")]
        [ProducesResponseType(typeof(APIOperationResponse<PaginatedList<AmmunitionDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.View", "Permissions.Ammunition.Page")]
        public async Task<IActionResult> GetAllPaginated([FromBody] PagedListRequest request)
        {
            var result = await _ammunitionService.GetAllPaginatedAsync(request);
            return ProcessResponse(result);
        }

        [HttpGet("type/{ammunitionType}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.View", "Permissions.Ammunition.Page")]
        public async Task<IActionResult> GetByType(AmmunitionType ammunitionType)
        {
            var result = await _ammunitionService.GetByTypeAsync(ammunitionType);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Ammunition.Create")]
        public async Task<IActionResult> Create([FromForm] CreateUpdateAmmunitionDto dto, [FromForm] List<IFormFile>? files = null)
        {
            var result = await _ammunitionService.CreateAsync(dto, files);
            return ProcessResponse(result);
        }

        [HttpPost("Import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.Create")]
        public async Task<IActionResult> Import(IFormFile file, [FromQuery] string language = "en")
        {
            var result = await _ammunitionService.ImportAsync(file, language);
            return ProcessResponse(result);
        }

        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromQuery] string language = "en")
        {
            var result = await _ammunitionService.ImportPreviewAsync(file, language);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Generate ammunition import template with Excel data validation (dropdowns for lookups)
        /// </summary>
        /// <param name="language">Language for template headers (en/ar), defaults to 'en'</param>
        /// <returns>Excel file with data validation dropdowns and all fields from web form</returns>
        [HttpGet("template")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [CheckAuthorize("Permissions.Ammunition.Create")]
        public async Task<IActionResult> GenerateImportTemplate([FromQuery] string language = "en")
        {
            try
            {
                _logger.LogInformation("Generating ammunition import template. Language: {Language}", language);

                var templateResult = await _ammunitionService.GenerateImportTemplateAsync(language);
                
                if (!templateResult.Succeeded || templateResult.Data == null)
                {
                    _logger.LogWarning("Failed to generate ammunition import template");
                    return StatusCode(500, new { message = "Failed to generate template", errors = templateResult.Errors });
                }

                var fileName = $"Ammunition_Import_Template_{_dateTimeProvider.Now:yyyyMMdd}.xlsx";
                
                _logger.LogInformation("Ammunition import template generated successfully. Language: {Language}, FileSize: {FileSize} bytes", 
                    language, templateResult.Data.Length);

                return File(
                    templateResult.Data,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating ammunition import template");
                return StatusCode(500, new { message = "An error occurred while generating the template", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateAmmunitionDto dto)
        {
            var result = await _ammunitionService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Ammunition.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _ammunitionService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}
