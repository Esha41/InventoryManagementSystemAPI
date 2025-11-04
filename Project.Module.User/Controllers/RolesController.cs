using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RolesController : ApiControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        [HttpPost("GetRolesWithPagination")]


 //       [CheckAuthorize(
 //    "Permissions.Roles.Page",
 //    "Permissions.Roles.View"
   
 //)]
        public async Task<IActionResult> GetRoles(PagedListRequest request)
        {
            var response = await _roleService.GetRolesAsync(request);
            return Ok(response);
        }

        //       [CheckAuthorize(
        //"Permissions.Roles.Page",
        //"Permissions.Roles.View")]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var response = await _roleService.GetRoleByIdAsync(id);
            if (!response.Succeeded) return NotFound(response);
            return Ok(response);
        }
        [AllowAnonymous]
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
        //[CheckAuthorize("Permissions.Roles.Page","Permissions.Roles.View")]
        [AllowAnonymous]
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

        //[CheckAuthorize("Permissions.Roles.Page","Permissions.Roles.View")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        //[Authorize("Permissions.Roles.Edit")]
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
        //[CheckAuthorize("Permissions.Roles.ViewUsers")]
        public async Task<IActionResult> GetUsersInRole(string id)
        {
            var response = await _roleService.GetUsersInRoleAsync(id);
            return ProcessResponse(response);
        }

        [HttpDelete("{id}/users")]
        //[CheckAuthorize("Permissions.Roles.RemoveUsers")]
        public async Task<IActionResult> RemoveUsersFromRole(string id, [FromBody] RemoveUsersFromRoleDto dto)
        {
            var response = await _roleService.RemoveUsersFromRoleAsync(id, dto);
            return ProcessResponse(response);
        }

        [HttpGet("entities")]
        public async Task<IActionResult> GetAllAppicationEntities()
        {
            var result = await _roleService.GetAllApplicationEntitiesAsync();

            if (!result.Succeeded)
            {
                return BadRequest(result); // or use appropriate status code
            }

            return Ok(result);
        }
        [HttpGet("getApplicationentities{roleId}")]
        public async Task<ActionResult<List<RoleApplicationEntityDto>>> GetByRole(string roleId)
        {
            var result = await _roleService.GetApplicationEntitiesByRoleAsync(roleId);
            if (result == null || result.Count == 0)
                return NotFound($"No application entities found for role {roleId}");

            return Ok(result);
        }
    }
}