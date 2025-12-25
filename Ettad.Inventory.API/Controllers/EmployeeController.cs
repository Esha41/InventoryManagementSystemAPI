using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Assets;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
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
    }
}

