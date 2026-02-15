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
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

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
        private readonly JwtOptions _jwtOptions;
        #endregion

        #region ctor
        public AccountController(IAccountServices authenticationService, IHelpureService helpureService, ICaptchaService captchaService, IOptions<JwtOptions> jwtOptions)
        {
            _authenticationService = authenticationService;
            _helpureService = helpureService;
            _captchaService = captchaService;
            _jwtOptions = jwtOptions?.Value ?? new JwtOptions();
        }
        #endregion




        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInformation request)
        {
            var result = await _authenticationService.Login(request);

            if (result.Succeeded && result.Data != null && !string.IsNullOrEmpty(result.Data.RefreshToken))
            {
                SetRefreshTokenCookie(Response, result.Data.RefreshToken);
                result.Data.RefreshToken = string.Empty;
            }

            return ProcessResponse(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authenticationService.RefreshTokenFromCookieAsync();
            if (!result.Succeeded || result.Data == null)
                return ProcessResponse(result);

            SetRefreshTokenCookie(Response, result.Data.RefreshToken);
            result.Data.RefreshToken = string.Empty;
            return ProcessResponse(result);
        }

        private void SetRefreshTokenCookie(HttpResponse response, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                MaxAge = TimeSpan.FromMinutes(_jwtOptions.RefreshTokenExpireInMinutes)
            };
            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
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
