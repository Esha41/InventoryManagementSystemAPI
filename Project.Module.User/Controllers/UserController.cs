using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.Services.DataTransferObject.AuthenticationDto;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [CheckAuthorize("Permissions.SystemUsers.View", "Permissions.SystemUsers.page")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _userService.GetByIdAsync(id);
            return ProcessResponse(response);
        }

        [HttpPost("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var response = await _userService.GetCurrentUserAsync();
            return ProcessResponse(response);
        }

        [HttpGet]
        [CheckAuthorize("Permissions.SystemUsers.View", "Permissions.SystemUsers.page")]
        public async Task<IActionResult> GetAll([FromQuery] Ettad.CrossCutting.Comman.Models.PagedListRequest request)
        {
            var response = await _userService.GetAllAsync(request);
            return ProcessResponse(response);
        }
        //[HttpGet]
        //[CheckAuthorize("Permissions.SystemUsers.View", "Permissions.SystemUsers.page")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var response = await _userService.GetAllUserAsync();
        //    return ProcessResponse(response);
        //}


        [CheckAuthorize("Permissions.SystemUsers.Create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var response = await _userService.CreateAsync(dto);
            return ProcessResponse(response);
        }

        [CheckAuthorize("Permissions.SystemUsers.Edit")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            dto.Id = id;
            var response = await _userService.UpdateAsync(dto);
            return ProcessResponse(response);
        }
       
        [CheckAuthorize("Permissions.SystemUsers.Delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _userService.DeleteAsync(id);
            return ProcessResponse(response);
        }

        [CheckAuthorize("Permissions.SystemUsers.Edit")]
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> Restore(string id)
        {
            var response = await _userService.RestoreAsync(id);
            return ProcessResponse(response);
        }

        [HttpGet("{id}/roles")]
        [CheckAuthorize("Permissions.SystemUsers.View")] 
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var response = await _userService.GetUserRolesAsync(id);
            return ProcessResponse(response);
        }

        [HttpPut("{id}/roles")]
        [CheckAuthorize("Permissions.SystemUsers.Edit")] 
        public async Task<IActionResult> UpdateUserRoles(string id, [FromBody] UpdateUserRolesDto dto)
        {
            var response = await _userService.UpdateUserRolesAsync(id, dto);
            return ProcessResponse(response);
        }

        [HttpPut("{id}/toggle-status")]
        [CheckAuthorize("Permissions.SystemUsers.Edit")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var response = await _userService.ToggleUserStatusAsync(id);
            return ProcessResponse(response);
        }
        [HttpGet("Summary")]
        [CheckAuthorize("Permissions.SystemUsers.View", "Permissions.SystemUsers.page")]
        public async Task<IActionResult> GetSummary()
        {
            var response = await _userService.GetUsersSummaryAsync();
            return ProcessResponse(response);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var response = await _userService.ChangePasswordAsync(dto);
            return ProcessResponse(response);
        }
    }

}
