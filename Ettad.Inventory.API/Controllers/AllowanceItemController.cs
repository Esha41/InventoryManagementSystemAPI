using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.AllowanceItems;
using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AllowanceItemController : ApiControllerBase
    {
        private readonly IAllowanceItemService _allowanceItemService;

        public AllowanceItemController(IAllowanceItemService allowanceItemService)
        {
            _allowanceItemService = allowanceItemService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AllowanceItem.View", "Permissions.AllowanceItem.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _allowanceItemService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AllowanceItem.View", "Permissions.AllowanceItem.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _allowanceItemService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpGet("department/{departmentId}/year/{year}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AllowanceItem.View", "Permissions.AllowanceItem.Page")]
        public async Task<IActionResult> GetByDepartmentAndYear(long departmentId, int year)
        {
            var result = await _allowanceItemService.GetByDepartmentAndYearAsync(departmentId, year);
            return ProcessResponse(result);
        }

        [HttpGet("department/{departmentId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AllowanceItem.View", "Permissions.AllowanceItem.Page")]
        public async Task<IActionResult> GetByDepartment(long departmentId)
        {
            var result = await _allowanceItemService.GetByDepartmentAsync(departmentId);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.AllowanceItem.Create")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateAllowanceItemDto dto)
        {
            var result = await _allowanceItemService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        [HttpPost("bulk")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.AllowanceItem.Create")]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateAllowanceItemDto dto)
        {
            var result = await _allowanceItemService.BulkCreateAsync(dto);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.AllowanceItem.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateAllowanceItemDto dto)
        {
            var result = await _allowanceItemService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.AllowanceItem.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _allowanceItemService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

