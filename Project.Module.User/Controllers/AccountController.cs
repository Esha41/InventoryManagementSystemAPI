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
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        #region fields
        private readonly IAccountServices _authenticationService;
        private readonly IHelpureService _helpureService;
        private readonly ICaptchaService _captchaService;
        #endregion

        #region ctor
        public AccountController(IAccountServices authenticationService, IHelpureService helpureService, ICaptchaService captchaService)
        {
            _authenticationService = authenticationService;
            _helpureService = helpureService;
            _captchaService = captchaService;
        }
        #endregion




        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
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

        [HttpGet("generate-captcha")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult GenerateCaptcha()
        {
            var (captchaId, captchaCode) = _captchaService.GenerateCaptcha();
            
            // Return only the CAPTCHA ID to the client, not the code
            // The client will need to display the code and send it back with the login request
            return Ok(new { captchaId, captchaCode });
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            var result = await _authenticationService.LogoutAsync();
            return ProcessResponse(result);
        }

    }
}
