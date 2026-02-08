using Ettad.LdapSettings.Services.DTO;
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
        /// Validates credentials against LDAP using username and password.
        /// </summary>
        Task<bool> ValidateAsync(
                string username,
                string password,
                LdapOptions ldapOptions,
                CancellationToken cancellationToken = default);
    }

}
