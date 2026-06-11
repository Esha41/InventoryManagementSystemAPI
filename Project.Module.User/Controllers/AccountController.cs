using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using System.Net;
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
        private readonly ICaptchaService _captchaService;
        private readonly IOnboardingService _onboardingService;
        #endregion

        #region ctor
        public AccountController(
            IAccountServices authenticationService,
            ICaptchaService captchaService,
            IOnboardingService onboardingService)
        {
            _authenticationService = authenticationService;
            _captchaService = captchaService;
            _onboardingService = onboardingService;
        }
        #endregion

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInformation request)
        {
            var result = await _authenticationService.Login(request);

            if (result.Succeeded && result.Data != null && !result.Data.RequiresRoleSelection && !string.IsNullOrEmpty(result.Data.RefreshToken))
            {
                SetRefreshTokenCookie(Response, result.Data.RefreshToken);
                result.Data.RefreshToken = string.Empty;
            }

            return ProcessResponse(result);
        }

        [HttpPost("select-role")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SelectRole([FromBody] SelectRoleDto request)
        {
            var result = await _authenticationService.SelectRoleAsync(request);
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
            // Session cookie (no MaxAge/Expires): the browser discards it when closed,
            // so a closed browser cannot silently restore the session. Server-side
            // expiry (sliding window + absolute cap) bounds it independently.
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
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

        // AllowAnonymous: the idle-timeout logout fires with an already-expired access token,
        // and the interceptor deliberately never refresh-retries logout calls. The service
        // resolves the session from the httpOnly refresh cookie instead; a bearer token, when
        // present and valid, is still used for jti blacklisting.
        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            var result = await _authenticationService.LogoutAsync();
            return ProcessResponse(result);
        }

        [HttpGet("onboarding-status")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOnboardingStatus()
        {
            var result = await _onboardingService.GetStatusAsync();
            return ProcessResponse(result);
        }

        [HttpPost("complete-onboarding")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CompleteOnboarding()
        {
            var result = await _onboardingService.CompleteAsync();
            return ProcessResponse(result);
        }
    }
}
