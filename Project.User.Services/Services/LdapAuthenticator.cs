using Ettad.CrossCutting.Comman.Exception;
using Ettad.LdapSettings.Services.DTO;
using Ettad.User.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using Microsoft.Extensions.Logging;

namespace Ettad.User.Services.Services
{
    public class LdapAuthenticator : ILdapAuthenticator
    {
        private readonly ILogger<AccountServices> _logger;
        public LdapAuthenticator(ILogger<AccountServices> logger) { _logger = logger; }

        public Task<bool> ValidateAsync(
            string username,
            string password,
            LdapOptions ldapOptions,
            CancellationToken cancellationToken = default)
        {
            if (!ldapOptions.IsActive)
            {
                _logger.LogWarning("LDAP validation skipped: LDAP is inactive. Username: {Username}", username);
                return Task.FromResult(false);
            }

            if (string.IsNullOrWhiteSpace(ldapOptions.LdapServer) ||
                string.IsNullOrWhiteSpace(ldapOptions.LdapDomain))
            {
                _logger.LogError("LDAP validation failed: Invalid LDAP settings. Username: {Username}", username);
                throw new ApiException("server.invalidLdapSettings");
            }

            try
            {
                // Format username with domain if not already specified
                var principalUsername = username.Trim();
                if (!principalUsername.Contains("\\") && !principalUsername.Contains("@"))
                {
                    principalUsername = $"{ldapOptions.LdapDomain}\\{principalUsername}";
                }

                // Use explicit credentials for PrincipalContext
                using var context = new PrincipalContext(
                    ContextType.Domain,
                    ldapOptions.LdapDomain,
                    ldapOptions.LdapUsername,
                    ldapOptions.LdapPassword
                );

                // Validate credentials
                var isValid = context.ValidateCredentials(username, password, ContextOptions.Negotiate);

                if (isValid)
                    _logger.LogInformation("LDAP validation successful. Username: {Username}", username);
                else
                    _logger.LogWarning("LDAP validation failed: Invalid password. Username: {Username}", username);

                return Task.FromResult(isValid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LDAP validation exception. Username: {Username}", username);
                throw new ApiException("server.invalidLdapSettings: " + ex.Message);
            }
        }

    }
}
