using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Explosives.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Net;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Explosives.Interfaces;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExplosiveController : ApiControllerBase
    {
        private readonly IExplosiveService _explosiveService;
        private readonly ILogger<ExplosiveController> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ExplosiveController(
            IExplosiveService explosiveService,
            ILogger<ExplosiveController> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _explosiveService = explosiveService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            
            // Set EPPlus license context
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.View", "Permissions.Explosive.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _explosiveService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpPost("Paginated")]
        [ProducesResponseType(typeof(APIOperationResponse<PaginatedList<ExplosiveDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.View", "Permissions.Explosive.Page")]
        public async Task<IActionResult> GetAllPaginated([FromBody] PagedListRequest request)
        {
            var result = await _explosiveService.GetAllPaginatedAsync(request);
            return ProcessResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.View")]
        public async Task<IActionResult> GetById(long id, [FromQuery] bool includeDeleted = false)
        {
            var result = await _explosiveService.GetByIdAsync(id, includeDeleted);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Explosive.Create")]
        public async Task<IActionResult> Create([FromForm] CreateUpdateExplosiveDto input, [FromForm] List<IFormFile> files)
        {
            var result = await _explosiveService.CreateAsync(input, files);
            return ProcessResponse(result);
        }

        [HttpPost("Import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.Create")]
        public async Task<IActionResult> Import(IFormFile file, [FromQuery] string language = "en")
        {
            var result = await _explosiveService.ImportAsync(file, language);
            return ProcessResponse(result);
        }

        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromQuery] string language = "en")
        {
            var result = await _explosiveService.ImportPreviewAsync(file, language);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Generate explosive import template with Excel data validation (dropdowns for lookups)
        /// </summary>
        /// <param name="language">Language for template headers (en/ar), defaults to 'en'</param>
        /// <returns>Excel file with data validation dropdowns and all fields from web form</returns>
        [HttpGet("template")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [CheckAuthorize("Permissions.Explosive.Create")]
        public async Task<IActionResult> GenerateImportTemplate([FromQuery] string language = "en")
        {
            try
            {
                _logger.LogInformation("Generating explosive import template. Language: {Language}", language);

                var templateResult = await _explosiveService.GenerateImportTemplateAsync(language);
                
                if (!templateResult.Succeeded || templateResult.Data == null)
                {
                    _logger.LogWarning("Failed to generate explosive import template");
                    return StatusCode(500, new { message = "Failed to generate template", errors = templateResult.Errors });
                }

                var fileName = $"Explosive_Import_Template_{_dateTimeProvider.Now:yyyyMMdd}.xlsx";
                
                _logger.LogInformation("Explosive import template generated successfully. Language: {Language}, FileSize: {FileSize} bytes", 
                    language, templateResult.Data.Length);

                return File(
                    templateResult.Data,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating explosive import template");
                return StatusCode(500, new { message = "An error occurred while generating the template", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateExplosiveDto input)
        {
            var result = await _explosiveService.UpdateAsync(id, input);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _explosiveService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        [HttpPost("{id}/restore")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.Edit")]
        public async Task<IActionResult> Restore(long id)
        {
            var result = await _explosiveService.RestoreAsync(id);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}/permanent")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Explosive.Delete")]
        public async Task<IActionResult> PermanentDelete(long id)
        {
            var result = await _explosiveService.PermanentDeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

