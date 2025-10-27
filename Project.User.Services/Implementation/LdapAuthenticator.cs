using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Ettad.CrossCutting.Comman.Exception;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
namespace Ettad.User.Services.Implementation
{
    public class LdapAuthenticator : ILdapAuthenticator
    {
        private readonly LdapOptions _options;

        public LdapAuthenticator(IOptions<LdapOptions> options)
        {
            _options = options.Value;
        }

        public Task<bool> ValidateAsync(string username, string? password, bool loginWithoutPassword, CancellationToken cancellationToken = default)
        {
            if (!_options.IsActive) return Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(_options.LdapServer) && string.IsNullOrWhiteSpace(_options.LdapDomain))
                throw new ApiException("server.invalidLdapSettings");

            try
            {
                if (loginWithoutPassword)
                {
                    using var directory = new DirectoryEntry(_options.LdapServer);
                    using var searcher = new DirectorySearcher(directory)
                    {
                        Filter = $"(&(objectClass=user)({_options.LdapEmpAttr}={username}))"
                    };
                    var found = searcher.FindOne();
                    return Task.FromResult(found != null);
                }

                // validate credentials using PrincipalContext
                using var context = string.IsNullOrWhiteSpace(_options.LdapUsername) || string.IsNullOrWhiteSpace(_options.LdapPassword)
                    ? new PrincipalContext(ContextType.Domain, _options.LdapDomain)
                    : new PrincipalContext(ContextType.Domain, _options.LdapDomain, _options.LdapUsername, _options.LdapPassword);

                var isValid = context.ValidateCredentials(username, password);
                return Task.FromResult(isValid);
            }
            catch (Exception ex)
            {
                // LDAP-specific failure should be mapped to a friendly ApiException so callers don't leak implementation details
                throw new ApiException("server.invalidLdapSettings: " + ex.Message);
            }
        }
    }
}
