using Ettad.Application.Common.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Common.Security;
using Ettad.Modules.EmailSystem.API.Models;
using Ettad.Modules.EmailSystem.API.Services;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Modules.EmailSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailSettingsController : ApiControllerBase
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailDispatchService _emailDispatch;

        public EmailSettingsController(
            ISettingsProvider settingsProvider,
            ICurrentUserService currentUserService,
            IEmailDispatchService emailDispatch)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _emailDispatch = emailDispatch ?? throw new ArgumentNullException(nameof(emailDispatch));
        }

        /// <summary>
        /// Save email settings configuration
        /// </summary>
        /// <param name="emailSettings">Email settings data</param>
        /// <returns>Success result</returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.EmailSettings.Create", "Permissions.EmailSettings.Edit")]
        public async Task<IActionResult> SaveEmailSettings([FromBody] EmailSettingsDto emailSettings)
        {
            // For now, allow any authenticated user (can be restricted later with permissions)
            // TODO: Add proper permission check: Permissions.EmailSettings.Edit or require admin role
            
            if (!ModelState.IsValid)
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.BadRequest,
                    "Invalid model state"));
            }

            var result = await _settingsProvider.SaveEmailSettings(emailSettings);
            
            if (result)
            {
                return ProcessResponse(APIOperationResponse<bool>.Success(true, "Email settings saved successfully"));
            }
            else
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.InternalServerError,
                    "Failed to save email settings"));
            }
        }

        /// <summary>
        /// Check if email notifications are enabled (any authenticated user).
        /// Used by notification service to decide whether to send email on new notifications.
        /// </summary>
        [HttpGet("IsEnabled")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> IsEmailNotificationsEnabled()
        {
            var config = await _settingsProvider.getEmailSettings();
            var enabled = !config.DisableAuthentication;
            return ProcessResponse(APIOperationResponse<bool>.Success(enabled));
        }

        /// <summary>
        /// Get email settings configuration (admin only)
        /// </summary>
        /// <returns>Email settings</returns>
        [HttpGet]
        [ProducesResponseType(typeof(APIOperationResponse<EmailConfiguration>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.EmailSettings.View", "Permissions.EmailSettings.Page")]
        public async Task<IActionResult> GetEmailSettings()
        {
            // For now, allow any authenticated user (can be restricted later with permissions)
            // TODO: Add proper permission check: Permissions.EmailSettings.View
            var result = await _settingsProvider.getEmailSettings();
            return ProcessResponse(APIOperationResponse<EmailConfiguration>.Success(result));
        }

      
        [HttpPost("/api/Email/send")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendNotificationEmail([FromBody] SendEmailRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    ResponseType.BadRequest,
                    "Invalid request. Check recipient email, subject, and body."));
            }

            var result = await _emailDispatch.SendAsync(request, HttpContext?.RequestAborted ?? default);
            return ProcessResponse(result);
        }

    }
}
