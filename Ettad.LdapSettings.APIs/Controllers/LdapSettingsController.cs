using Ettad.LdapSettings.Services.Interfaces;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Common.Security;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ettad.LdapSettings.Services.Dtos;

namespace Ettad.LdapSettings.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CheckAuthorize("Permissions.LdapSettings.Page", "Permissions.LdapSettings.View")]
    public class LdapSettingsController : ApiControllerBase
    {
        private readonly ILdapSettingsService _ldapSettingsService;

        public LdapSettingsController(ILdapSettingsService ldapSettingsService)
        {
            _ldapSettingsService = ldapSettingsService ?? throw new ArgumentNullException(nameof(ldapSettingsService));
        }

        /// <summary>
        /// Save LDAP settings configuration
        /// </summary>
        /// <param name="ldapSettings">LDAP settings data</param>
        /// <returns>Success result</returns>
        [HttpPost]
        [CheckAuthorize("Permissions.LdapSettings.Create", "Permissions.LdapSettings.Edit")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateLdapSettings([FromBody] LdapOptions ldapSettings)
        {
            var result = await _ldapSettingsService.SaveLdapSettings(ldapSettings);
            return ProcessResponse<bool>(result);
        }

        /// <summary>
        /// Update LDAP settings configuration
        /// </summary>
        /// <param name="ldapSettings">LDAP settings data</param>
        /// <returns>Success result</returns>
        [HttpPut]
        [CheckAuthorize("Permissions.LdapSettings.Edit")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateLdapSettings([FromBody] LdapOptions ldapSettings)
        {
            var result = await _ldapSettingsService.SaveLdapSettings(ldapSettings);
            return ProcessResponse<bool>(result);
        }

        /// <summary>
        /// Get LDAP settings configuration
        /// </summary>
        /// <returns>LDAP settings</returns>
        [HttpGet]
        [CheckAuthorize("Permissions.LdapSettings.View")]
        [ProducesResponseType(typeof(APIOperationResponse<LdapOptions>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLdapSettings()
        {
            var result = await _ldapSettingsService.GetLdapSettings();
            return ProcessResponse<LdapOptions>(result);
        }

        /// <summary>
        /// Delete all LDAP settings
        /// </summary>
        /// <returns>Success result</returns>
        [HttpDelete]
        [CheckAuthorize("Permissions.LdapSettings.Delete")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteLdapSettings()
        {
            var result = await _ldapSettingsService.DeleteLdapSettings();
            return ProcessResponse<bool>(result);
        }
    }
}


