using Ettad.Application.Common.Interfaces;
using Ettad.Application.Common.Models;
using Ettad.LdapSettings.Services.DTO;
using Ettad.LdapSettings.Services.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.LdapSettings.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Temporarily allowing anonymous access for testing
    public class LdapSettingsController : ApiControllerBase
    {
        private readonly ILdapSettingsService _ldapSettingsService;
        private readonly ICurrentUserService _currentUserService;

        public LdapSettingsController(
            ILdapSettingsService ldapSettingsService,
            ICurrentUserService currentUserService)
        {
            _ldapSettingsService = ldapSettingsService ?? throw new ArgumentNullException(nameof(ldapSettingsService));
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
            if (!ModelState.IsValid)
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.BadRequest,
                    "Invalid model state"));
            }

            var result = await _ldapSettingsService.SaveLdapSettings(ldapSettings);

            if (result)
            {
                return ProcessResponse(APIOperationResponse<bool>.Success(true, "LDAP settings saved successfully"));
            }

            return ProcessResponse(APIOperationResponse<bool>.Fail(
                Ettad.ResponseHandler.Consts.ResponseType.InternalServerError,
                "Failed to save LDAP settings"));
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
            if (!ModelState.IsValid)
            {
                return ProcessResponse(APIOperationResponse<bool>.Fail(
                    Ettad.ResponseHandler.Consts.ResponseType.BadRequest,
                    "Invalid model state"));
            }

            var result = await _ldapSettingsService.SaveLdapSettings(ldapSettings);

            if (result)
            {
                return ProcessResponse(APIOperationResponse<bool>.Success(true, "LDAP settings updated successfully"));
            }

            return ProcessResponse(APIOperationResponse<bool>.Fail(
                Ettad.ResponseHandler.Consts.ResponseType.InternalServerError,
                "Failed to update LDAP settings"));
        }

        /// <summary>
        /// Get LDAP settings configuration
        /// </summary>
        /// <returns>LDAP settings</returns>
        [HttpGet]
        [ProducesResponseType(typeof(APIOperationResponse<LdapOptions>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLdapSettings()
        {
            var result = await _ldapSettingsService.GetLdapSettings();
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
            var result = await _ldapSettingsService.DeleteLdapSettings();

            if (result)
            {
                return ProcessResponse(APIOperationResponse<bool>.Success(true, "LDAP settings deleted successfully"));
            }

            return ProcessResponse(APIOperationResponse<bool>.Fail(
                Ettad.ResponseHandler.Consts.ResponseType.InternalServerError,
                "Failed to delete LDAP settings"));
        }
    }
}


