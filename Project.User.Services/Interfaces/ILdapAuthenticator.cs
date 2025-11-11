using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    public interface ILdapAuthenticator
    {
        /// <summary>
        /// Validates credentials against LDAP. If <paramref name="loginWithoutPassword"/> is true and <paramref name="password"/> is null,
        /// this method will only check that an account with the given username exists in LDAP.
        /// </summary>
        //  Task<bool> ValidateAsync(string username, string? password, bool loginWithoutPassword, CancellationToken cancellationToken = default);
        Task<bool> ValidateAsync(
                string username,
                string? password,
                bool loginWithoutPassword,
                LdapOptions ldapOptions,        // 👈 new parameter (settings from DB)
                CancellationToken cancellationToken = default);
    }

}
