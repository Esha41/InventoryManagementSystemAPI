
using Ettad.LdapSettings.Services.Dtos;

namespace Ettad.User.Services.Interfaces
{
    public interface ILdapAuthenticator
    {
        /// <summary>
        /// Validates credentials against LDAP using username and password.
        /// </summary>
        Task<bool> ValidateAsync(
                string username,
                string password,
                LdapOptions ldapOptions,
                CancellationToken cancellationToken = default);
    }

}
