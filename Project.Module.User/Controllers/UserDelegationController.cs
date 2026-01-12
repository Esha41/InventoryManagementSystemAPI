using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
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
        public async Task<IActionResult> GetAvailableUsers()
        {
            var response = await _userDelegationService.GetAvailableUsersAsync();
            return ProcessResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDelegationDto dto)
        {
            var response = await _userDelegationService.CreateDelegationAsync(dto);
            return ProcessResponse(response);
        }

        [HttpGet("my-delegations")]
        public async Task<IActionResult> GetMyDelegations()
        {
            var response = await _userDelegationService.GetMyDelegationsAsync();
            return ProcessResponse(response);
        }

        [HttpPut("{id}/revoke")]
        public async Task<IActionResult> Revoke(int id)
        {
            var response = await _userDelegationService.RevokeDelegationAsync(id);
            return ProcessResponse(response);
        }
    }
}
