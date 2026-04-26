using Ettad.LdapSettings.Services.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.LdapSettings.Services.Interfaces
{
    /// <summary>
    /// Abstraction for managing LDAP settings.
    /// </summary>
    public interface ILdapSettingsService
    {
        Task<APIOperationResponse<LdapOptions>> GetLdapSettings(CancellationToken cancellationToken = default);
        Task<APIOperationResponse<bool>> SaveLdapSettings(LdapOptions ldapSettings, CancellationToken cancellationToken = default);
        Task<APIOperationResponse<bool>> DeleteLdapSettings(CancellationToken cancellationToken = default);
    }
}
