using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Dtos;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CheckAuthorize("Permissions.ItemDepartmentAssignment.Page")]
    public class ItemDepartmentAssignmentController : ApiControllerBase
    {
        private readonly IItemDepartmentAssignmentService _assignmentService;

        public ItemDepartmentAssignmentController(IItemDepartmentAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.View")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _assignmentService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.View")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _assignmentService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpGet("summary")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.View")]
        public async Task<IActionResult> GetDepartmentSummaries()
        {
            var result = await _assignmentService.GetDepartmentSummariesAsync();
            return ProcessResponse(result);
        }

        [HttpGet("department/{departmentId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.View")]
        public async Task<IActionResult> GetByDepartment(long departmentId)
        {
            var result = await _assignmentService.GetByDepartmentIdAsync(departmentId);
            return ProcessResponse(result);
        }

        [HttpGet("item/{itemId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.View")]
        public async Task<IActionResult> GetByItem(long itemId)
        {
            var result = await _assignmentService.GetByItemIdAsync(itemId);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.Create")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateItemDepartmentAssignmentDto dto)
        {
            var result = await _assignmentService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        [HttpPost("bulk")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.Create")]
        public async Task<IActionResult> BulkAssign([FromBody] List<CreateUpdateItemDepartmentAssignmentDto> assignments)
        {
            var result = await _assignmentService.BulkAssignAsync(assignments);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateItemDepartmentAssignmentDto dto)
        {
            var result = await _assignmentService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.ItemDepartmentAssignment.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _assignmentService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}
