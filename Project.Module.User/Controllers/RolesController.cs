using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.Application.Common.Interfaces;

namespace Ettad.User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ApiControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ICurrentUserService _currentUserService;

        public RolesController(IRoleService roleService, ICurrentUserService currentUserService)
        {
            _roleService = roleService;
            _currentUserService = currentUserService;
        }

        [HttpPost("GetRolesWithPagination")]
        [CheckAuthorize(
             "Permissions.Roles.Page",
             "Permissions.Roles.View"

         )]
        public async Task<IActionResult> GetRoles(PagedListRequest request)
        {
            var response = await _roleService.GetRolesAsync(request);
            return Ok(response);
        }

        [CheckAuthorize(
             "Permissions.Roles.Page",
             "Permissions.Roles.View")
        ]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var response = await _roleService.GetRoleByIdAsync(id);
            if (!response.Succeeded) return NotFound(response);
            return Ok(response);
        }

        [CheckAuthorize(
             "Permissions.Roles.Page",
             "Permissions.Roles.View")
        ]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _roleService.GetAllRolesAsync();
            if (!response.Succeeded) return BadRequest(response);

            return Ok(response);
        }


        [HttpPost]
        [CheckAuthorize(
             "Permissions.Roles.Create"
         )]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            // Prevent non-super admins from creating super admin roles
            if (createRoleDto.IsSuperAdmin && !_currentUserService.IsSuperAdmin)
            {
                return Forbid();
            }

            var response = await _roleService.CreateRoleAsync(createRoleDto);
            if (!response.Succeeded) return BadRequest(response);
            return CreatedAtAction(nameof(GetRoleById), new { id = response.Data.Id }, response);
        }

        [CheckAuthorize(
         "Permissions.Roles.Edit"
        )]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            // Prevent non-super admins from setting or modifying super admin flag
            if (updateRoleDto.IsSuperAdmin && !_currentUserService.IsSuperAdmin)
            {
                return Forbid();
            }

            var response = await _roleService.UpdateRoleAsync(id, updateRoleDto);
            if (!response.Succeeded)
            {
                if (response.StatusCode == 404) return NotFound(response);
                return BadRequest(response);
            }
            return Ok(response);
        }


        [CheckAuthorize("Permissions.Roles.Delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var response = await _roleService.DeleteRoleAsync(id);
            if (!response.Succeeded)
            {
                if (response.StatusCode == 404) return NotFound(response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [CheckAuthorize("Permissions.Roles.Page","Permissions.Roles.View")]
        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetPermitionByRoleId(string id)
        {
            var response = await _roleService.GetPlainPermissionsForRoleAsync(id);
            if (!response.Succeeded)
            {
                if (response.StatusCode == 404) return NotFound(response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [CheckAuthorize("Permissions.Roles.Page","Permissions.Roles.View")]
        [HttpGet("{id}/crud/permissions")]
        public async Task<IActionResult> GetCrudPermissionsForRole(string id)
        {
            var response = await _roleService.GetCrudPermissionsForRole(id);
            if (!response.Succeeded)
            {
                if (response.StatusCode == 404) return NotFound(response);
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("permissions")]
        [CheckAuthorize("Permissions.Roles.Edit")]
        public async Task<ActionResult> AssignPermissionsToRole(AssignPermissionsDto assignPermissions)
        {
            var response = await _roleService.AssignPermissionsToRoleAsync(assignPermissions);
            if (!response.Succeeded)
            {
                if (response.StatusCode == 404) return NotFound(response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}/users")] 
        [CheckAuthorize("Permissions.Roles.ViewUsers")]
        public async Task<IActionResult> GetUsersInRole(string id)
        {
            var response = await _roleService.GetUsersInRoleAsync(id);
            return ProcessResponse(response);
        }

        [HttpDelete("{id}/users")]
        [CheckAuthorize("Permissions.Roles.RemoveUsers")]
        public async Task<IActionResult> RemoveUsersFromRole(string id, [FromBody] RemoveUsersFromRoleDto dto)
        {
            var response = await _roleService.RemoveUsersFromRoleAsync(id, dto);
            return ProcessResponse(response);
        }

        [CheckAuthorize("Permissions.Roles.Page", "Permissions.Roles.View")]
        [HttpGet("entities")]
        public async Task<ActionResult> GetAllAppicationEntities()
        {
            // Calls role service to retrieve all application entities
            var result = await _roleService.GetAllApplicationEntitiesAsync();

            // Returns standardized API response using base controller helper
            return ProcessResponse(result);
        }

        [CheckAuthorize("Permissions.Roles.Page", "Permissions.Roles.View")]
        [HttpGet("getApplicationentities/{roleId}")]
        public async Task<ActionResult<List<RoleApplicationEntityDto>>> GetByRole(string roleId)
        {
            var result = await _roleService.GetApplicationEntitiesByRoleAsync(roleId);
            if (result == null || result.Count == 0)
                return NotFound($"No application entities found for role {roleId}");

            return Ok(result);
        }
    }
}