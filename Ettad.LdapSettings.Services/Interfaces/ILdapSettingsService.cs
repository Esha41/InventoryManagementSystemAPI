using Ettad.Application.Common.Models;
using Ettad.LdapSettings.Services.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.LdapSettings.Services.Interfaces
{
    /// <summary>
    /// Abstraction for managing LDAP settings.
    /// </summary>
    public interface ILdapSettingsService
    {
        Task<LdapOptions> GetLdapSettings(CancellationToken cancellationToken = default);
        Task<bool> SaveLdapSettings(LdapOptions ldapSettings, CancellationToken cancellationToken = default);
        Task<bool> DeleteLdapSettings(CancellationToken cancellationToken = default);
    }
}


