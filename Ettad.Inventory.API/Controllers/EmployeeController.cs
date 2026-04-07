using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Assets;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ApiControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Employee.View", "Permissions.Employee.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Employee.View", "Permissions.Employee.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _employeeService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Employee.Create")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateEmployeeDto dto)
        {
            var result = await _employeeService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Employee.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateEmployeeDto dto)
        {
            var result = await _employeeService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Employee.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _employeeService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet("template")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Employee.View", "Permissions.Employee.Page")]
        public async Task<IActionResult> GenerateImportTemplate([FromQuery] string language = "en")
        {
            var result = await _employeeService.GenerateImportTemplateAsync(language);
            if (!result.Succeeded || result.Data == null || result.Data.Length == 0)
                return ProcessResponse(result);

            return File(result.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Employee_Import_Template.xlsx");
        }

        [HttpGet("export")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Employee.View", "Permissions.Employee.Page")]
        public async Task<IActionResult> Export([FromQuery] string language = "en")
        {
            var result = await _employeeService.ExportAsync(language);
            if (!result.Succeeded || result.Data == null || result.Data.Length == 0)
                return ProcessResponse(result);

            return File(result.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Employees_{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }

        [HttpPost("import-preview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Employee.Create", "Permissions.Employee.Edit")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromQuery] string language = "en")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is required" });

            var result = await _employeeService.ImportPreviewAsync(file, language);
            return ProcessResponse(result);
        }

        [HttpPost("import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Employee.Create", "Permissions.Employee.Edit")]
        public async Task<IActionResult> Import(IFormFile file, [FromQuery] string language = "en")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is required" });

            var result = await _employeeService.ImportAsync(file, language);
            return ProcessResponse(result);
        }
    }
}

