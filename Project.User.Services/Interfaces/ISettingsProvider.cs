using Ettad.Application.Common.Models;
using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    public interface ISettingsProvider
    {
        Task<LdapOptions> GetLdapSettings(CancellationToken cancellationToken = default);
        Task<EmailConfiguration> getEmailSettings();

    }
}
