using Ettad.LdapSettings.Services.DTO;
using Ettad.ResponseHandler.Models;
using System.Threading;
using System.Threading.Tasks;

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
