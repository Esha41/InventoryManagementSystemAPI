using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Accessories.Dtos;
using Ettad.Inventory.Service.Accessories.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class AccessoryController : ApiControllerBase
    {
        private readonly IAccessoryService _accessoryService;
        private readonly ILogger<AccessoryController> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AccessoryController(
            IAccessoryService accessoryService,
            ILogger<AccessoryController> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _accessoryService = accessoryService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;

            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.View", "Permissions.Accessory.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accessoryService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpPost("Paginated")]
        [ProducesResponseType(typeof(APIOperationResponse<PaginatedList<AccessoryDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.View", "Permissions.Accessory.Page")]
        public async Task<IActionResult> GetAllPaginated([FromBody] PagedListRequest request)
        {
            var result = await _accessoryService.GetAllPaginatedAsync(request);
            return ProcessResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.View")]
        public async Task<IActionResult> GetById(long id, [FromQuery] bool includeDeleted = false)
        {
            var result = await _accessoryService.GetByIdAsync(id, includeDeleted);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Accessory.Create")]
        public async Task<IActionResult> Create([FromForm] CreateUpdateAccessoryDto input, [FromForm] List<IFormFile> files)
        {
            var result = await _accessoryService.CreateAsync(input, files);
            return ProcessResponse(result);
        }

        [HttpPost("Import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.Create")]
        public async Task<IActionResult> Import(IFormFile file, [FromQuery] string language = "en")
        {
            var result = await _accessoryService.ImportAsync(file, language);
            return ProcessResponse(result);
        }

        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromQuery] string language = "en")
        {
            var result = await _accessoryService.ImportPreviewAsync(file, language);
            return ProcessResponse(result);
        }

        [HttpGet("template")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [CheckAuthorize("Permissions.Accessory.Create")]
        public async Task<IActionResult> GenerateImportTemplate([FromQuery] string language = "en")
        {
            try
            {
                _logger.LogInformation("Generating accessory import template. Language: {Language}", language);

                var templateResult = await _accessoryService.GenerateImportTemplateAsync(language);

                if (!templateResult.Succeeded || templateResult.Data == null)
                {
                    _logger.LogWarning("Failed to generate accessory import template");
                    return StatusCode(500, new { message = "Failed to generate template", errors = templateResult.Errors });
                }

                var fileName = $"Accessory_Import_Template_{_dateTimeProvider.Now:yyyyMMdd}.xlsx";

                return File(
                    templateResult.Data,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating accessory import template");
                return StatusCode(500, new { message = "An error occurred while generating the template", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateAccessoryDto input)
        {
            var result = await _accessoryService.UpdateAsync(id, input);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _accessoryService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        [HttpPost("{id}/restore")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.Edit")]
        public async Task<IActionResult> Restore(long id)
        {
            var result = await _accessoryService.RestoreAsync(id);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}/permanent")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Accessory.Delete")]
        public async Task<IActionResult> PermanentDelete(long id)
        {
            var result = await _accessoryService.PermanentDeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}
