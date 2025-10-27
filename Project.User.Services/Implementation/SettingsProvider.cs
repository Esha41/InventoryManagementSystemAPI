using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ettad.Application.Common.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;
namespace Ettad.User.Services.Implementation
{
 
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IOptions<LdapOptions> _ldapOptions;

        public SettingsProvider(IOptions<LdapOptions> ldapOptions)
        {
            _ldapOptions = ldapOptions;
        }

        public Task<LdapOptions> GetLdapSettings(CancellationToken cancellationToken = default)
        {
            // Return a copy to avoid accidental external updates
            var copy = new LdapOptions
            {
                IsActive = _ldapOptions.Value.IsActive,
                LdapServer = _ldapOptions.Value.LdapServer,
                LdapDomain = _ldapOptions.Value.LdapDomain,
                LdapUsername = _ldapOptions.Value.LdapUsername,
                LdapPassword = _ldapOptions.Value.LdapPassword,
                LdapEmpAttr = _ldapOptions.Value.LdapEmpAttr
            };

            return Task.FromResult(copy);
        }
        public async Task<EmailConfiguration> getEmailSettings()
        {
            //GetSettingsByTypeQueryHandler objHandler = new GetSettingsByTypeQueryHandler(_context);
            return (new EmailConfiguration());
        }
    }

}
