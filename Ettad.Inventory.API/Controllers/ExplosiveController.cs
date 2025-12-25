using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Explosives;
using Ettad.Inventory.Service.Explosives.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExplosiveController : ApiControllerBase
    {
        private readonly IExplosiveService _explosiveService;
        private readonly ILogger<ExplosiveController> _logger;

        public ExplosiveController(
            IExplosiveService explosiveService,
            ILogger<ExplosiveController> logger)
        {
            _explosiveService = explosiveService;
            _logger = logger;
            
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

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.View")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _explosiveService.GetByIdAsync(id);
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
        public async Task<IActionResult> Import(IFormFile file)
        {
            var result = await _explosiveService.ImportAsync(file);
            return ProcessResponse(result);
        }

        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file)
        {
            var result = await _explosiveService.ImportPreviewAsync(file);
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

                var fileName = $"Explosive_Import_Template_{DateTime.UtcNow:yyyyMMdd}.xlsx";
                
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
    }
}

