using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.CrossCutting.Common.Security;

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
        [CheckAuthorize("Permissions.UserDelegations.Delete", "DelegationManagement")]
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

        // Admin endpoints (require DelegationManagement — not only View/Page used for profile delegations)
        [HttpGet("admin/all")]
        [CheckAuthorize("DelegationManagement")]
        public async Task<IActionResult> GetAllDelegations()
        {
            var response = await _userDelegationService.GetAllDelegationsAsync();
            return ProcessResponse(response);
        }

        [HttpGet("admin/history")]
        [CheckAuthorize("DelegationManagement")]
        public async Task<IActionResult> GetDelegationHistory()
        {
            var response = await _userDelegationService.GetDelegationHistoryAsync();
            return ProcessResponse(response);
        }

        [HttpGet("settings/cross-department")]
        [CheckAuthorize("DelegationManagement")]
        public async Task<IActionResult> GetCrossDepartmentDelegationSetting()
        {
            var response = await _userDelegationService.GetAllowCrossDepartmentDelegationAsync();
            return ProcessResponse(response);
        }

        [HttpPut("settings/cross-department")]
        [CheckAuthorize("DelegationManagement")]
        public async Task<IActionResult> UpdateCrossDepartmentDelegationSetting([FromBody] bool allow)
        {
            var response = await _userDelegationService.UpdateAllowCrossDepartmentDelegationAsync(allow);
            return ProcessResponse(response);
        }

        [HttpGet("settings/delegator-action")]
        [CheckAuthorize("DelegationManagement")]
        public async Task<IActionResult> GetDelegatorActionSetting()
        {
            var response = await _userDelegationService.GetAllowDelegatorActionAsync();
            return ProcessResponse(response);
        }

        [HttpPut("settings/delegator-action")]
        [CheckAuthorize("DelegationManagement")]
        public async Task<IActionResult> UpdateDelegatorActionSetting([FromBody] bool allow)
        {
            var response = await _userDelegationService.UpdateAllowDelegatorActionAsync(allow);
            return ProcessResponse(response);
        }

        [HttpGet("is-restricted")]
        public async Task<IActionResult> IsRestricted()
        {
            // We need current user ID, assuming BaseController provides it or retrieve from service
             // The check logic is nicely encapsulated in service, but service method takes userId.
             // We need to inject ICurrentUserService to get ID here or use User.Identity
             // ApiControllerBase might have CurrentUser property? 
             // Let's assume HttpContext.User
            
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var isRestricted = await _userDelegationService.IsUserRestrictedByDelegationAsync(userId);
            return Ok(new APIOperationResponse<bool> { Succeeded = true, Data = isRestricted });
        }
    }
}
