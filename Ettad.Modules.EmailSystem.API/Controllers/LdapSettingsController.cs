using Ettad.Application.Common.Models;
using Ettad.Application.Common.Interfaces;
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
    [AllowAnonymous] // Temporarily allowing anonymous access for testing
    public class LdapSettingsController : ApiControllerBase
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly ICurrentUserService _currentUserService;

        public LdapSettingsController(ISettingsProvider settingsProvider, ICurrentUserService currentUserService)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        /// <summary>
        /// Save LDAP settings configuration
        /// </summary>
        /// <param name="ldapSettings">LDAP settings data</param>
        /// <returns>Success result</returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateLdapSettings([FromBody] LdapOptions ldapSettings)
        {
            // For now, allow any authenticated user (can be restricted later with permissions)
            // TODO: Add proper permission check: Permissions.LdapSettings.Edit or require admin role

            if (!ModelState.IsValid)
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.BadRequest,
                    "Invalid model state"));
            }

            var result = await _settingsProvider.SaveLdapSettings(ldapSettings);

            if (result)
            {
                return ProcessResponse(APIOperationResponse<bool>.Success(true, "LDAP settings saved successfully"));
            }
            else
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.InternalServerError,
                    "Failed to save LDAP settings"));
            }
        }

        /// <summary>
        /// Update LDAP settings configuration
        /// </summary>
        /// <param name="ldapSettings">LDAP settings data</param>
        /// <returns>Success result</returns>
        [HttpPut]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateLdapSettings([FromBody] LdapOptions ldapSettings)
        {
            // Uses the same upsert logic as POST
            if (!ModelState.IsValid)
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.BadRequest,
                    "Invalid model state"));
            }

            var result = await _settingsProvider.SaveLdapSettings(ldapSettings);

            if (result)
            {
                return ProcessResponse(APIOperationResponse<bool>.Success(true, "LDAP settings updated successfully"));
            }
            else
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.InternalServerError,
                    "Failed to update LDAP settings"));
            }
        }

        /// <summary>
        /// Get LDAP settings configuration
        /// </summary>
        /// <returns>LDAP settings</returns>
        [HttpGet]
        [ProducesResponseType(typeof(APIOperationResponse<LdapOptions>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLdapSettings()
        {
            // For now, allow any authenticated user (can be restricted later with permissions)
            // TODO: Add proper permission check: Permissions.LdapSettings.View
            var result = await _settingsProvider.GetLdapSettings();
            return ProcessResponse(APIOperationResponse<LdapOptions>.Success(result));
        }

        /// <summary>
        /// Delete all LDAP settings
        /// </summary>
        /// <returns>Success result</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteLdapSettings()
        {
            var result = await _settingsProvider.DeleteLdapSettings();

            if (result)
            {
                return ProcessResponse(APIOperationResponse<bool>.Success(true, "LDAP settings deleted successfully"));
            }
            else
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.InternalServerError,
                    "Failed to delete LDAP settings"));
            }
        }
    }
}


