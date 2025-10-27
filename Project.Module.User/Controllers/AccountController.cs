using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Ettad.ResponseHandler.Models;
using Ettad.Services.DataTransferObject.AuthenticationDto;
using Ettad.Services.Helpers;
using Ettad.Services.Interfaces;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Implementation;
using Ettad.User.Services.Interfaces;
using System.Net;

namespace Ettad.User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        #region fields
        private readonly IAccountServices _authenticationService;
        private readonly IHelpureService _helpureService;
        #endregion

        #region ctor
        public AccountController(IAccountServices authenticationService, IHelpureService helpureService)
        {
            _authenticationService = authenticationService;
            _helpureService = helpureService;
        }
        #endregion




        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginInformation request)
        {

            var result = await _authenticationService.Login(request);

            return ProcessResponse(result);

        }

        [Authorize]
        [HttpGet("user-claims")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUserClaims()
        {
            var result = await _authenticationService.GetRoleClaimsOnlyAsync();


            return ProcessResponse(result);
        }
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var result = await _authenticationService.ForgotPasswordAsync(request);
            return ProcessResponse(result);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request);
            return ProcessResponse(result);
        }

    }
}
