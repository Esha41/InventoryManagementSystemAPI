using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.CrossCutting.Common.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ettad.User.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserDelegationController : ApiControllerBase
    {
        private readonly IUserDelegationService _userDelegationService;

        public UserDelegationController(IUserDelegationService userDelegationService)
        {
            _userDelegationService = userDelegationService;
        }

        [HttpGet("available-users")]
        [CheckAuthorize("Permissions.UserDelegations.View", "Permissions.UserDelegations.Page")]
        public async Task<IActionResult> GetAvailableUsers()
        {
            var response = await _userDelegationService.GetAvailableUsersAsync();
            return ProcessResponse(response);
        }

        [HttpPost]
        [CheckAuthorize("Permissions.UserDelegations.Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDelegationDto dto)
        {
            var response = await _userDelegationService.CreateDelegationAsync(dto);
            return ProcessResponse(response);
        }

        [HttpGet("my-delegations")]
        [CheckAuthorize("Permissions.UserDelegations.View", "Permissions.UserDelegations.Page")]
        public async Task<IActionResult> GetMyDelegations()
        {
            var response = await _userDelegationService.GetMyDelegationsAsync();
            return ProcessResponse(response);
        }

        [HttpPut("{id}/revoke")]
        [CheckAuthorize("Permissions.UserDelegations.Delete")]
        public async Task<IActionResult> Revoke(int id)
        {
            var response = await _userDelegationService.RevokeDelegationAsync(id);
            return ProcessResponse(response);
        }

        [HttpPut("{id}/approve")]
        [CheckAuthorize("Permissions.UserDelegations.View", "Permissions.UserDelegations.Page")]
        public async Task<IActionResult> Approve(int id)
        {
            var response = await _userDelegationService.ApproveDelegationAsync(id);
            return ProcessResponse(response);
        }

        [HttpPut("{id}/reject")]
        [CheckAuthorize("Permissions.UserDelegations.View", "Permissions.UserDelegations.Page")]
        public async Task<IActionResult> Reject(int id)
        {
            var response = await _userDelegationService.RejectDelegationAsync(id);
            return ProcessResponse(response);
        }

        [HttpGet("pending-delegations")]
        [CheckAuthorize("Permissions.UserDelegations.View", "Permissions.UserDelegations.Page")]
        public async Task<IActionResult> GetPendingDelegations()
        {
            var response = await _userDelegationService.GetPendingDelegationsAsync();
            return ProcessResponse(response);
        }

        // Admin endpoints
        [HttpGet("admin/all")]
        [CheckAuthorize("Permissions.UserDelegations.View", "Permissions.UserDelegations.Page")]
        public async Task<IActionResult> GetAllDelegations()
        {
            var response = await _userDelegationService.GetAllDelegationsAsync();
            return ProcessResponse(response);
        }

        [HttpGet("admin/history")]
        [CheckAuthorize("Permissions.UserDelegations.View", "Permissions.UserDelegations.Page")]
        public async Task<IActionResult> GetDelegationHistory()
        {
            var response = await _userDelegationService.GetDelegationHistoryAsync();
            return ProcessResponse(response);
        }
    }
}
