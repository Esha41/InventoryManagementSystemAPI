using Ettad.Application.Common.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Common.Security;
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

        public EmailSettingsController(ISettingsProvider settingsProvider, ICurrentUserService currentUserService)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
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
        /// Get email settings configuration
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
    }
}
