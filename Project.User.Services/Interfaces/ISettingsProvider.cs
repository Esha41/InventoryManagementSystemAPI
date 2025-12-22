using Ettad.Application.Common.Models;
using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    /// <summary>
    /// Provider for non-LDAP application settings such as email configuration.
    /// LDAP settings are handled by ILdapSettingsService in the Ettad.LdapSettings.Services module.
    /// </summary>
    public interface ISettingsProvider
    {
        Task<EmailConfiguration> getEmailSettings(CancellationToken cancellationToken = default);
        Task<bool> SaveEmailSettings(EmailSettingsDto emailSettings, CancellationToken cancellationToken = default);
    }
}
