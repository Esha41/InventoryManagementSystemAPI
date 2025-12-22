using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Ettad.CrossCutting.Comman.Exception;
using Ettad.LdapSettings.Services.DTO;
using Ettad.User.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using Microsoft.Extensions.Logging;

namespace Ettad.User.Services.Implementation
{
    public class LdapAuthenticator : ILdapAuthenticator
    {
        private readonly ILogger<AccountServices> _logger;
        public LdapAuthenticator(ILogger<AccountServices> logger) { _logger = logger; }
        //public Task<bool> ValidateAsync(
        //    string username,
        //    string? password,
        //    bool loginWithoutPassword,
        //    LdapOptions ldapOptions,
        //    CancellationToken cancellationToken = default)
        //{
        //    if (!ldapOptions.IsActive)
        //        return Task.FromResult(false);

        //    if (string.IsNullOrWhiteSpace(ldapOptions.LdapServer) &&
        //        string.IsNullOrWhiteSpace(ldapOptions.LdapDomain))
        //        throw new ApiException("server.invalidLdapSettings");

        //    try
        //    {
        //        if (loginWithoutPassword)
        //        {
        //            using var directory = new DirectoryEntry(ldapOptions.LdapServer);
        //            using var searcher = new DirectorySearcher(directory)
        //            {
        //                Filter = $"(&(objectClass=user)({ldapOptions.LdapEmpAttr}={username}))"
        //            };
        //            var found = searcher.FindOne();
        //            return Task.FromResult(found != null);
        //        }

        //        using var context = string.IsNullOrWhiteSpace(ldapOptions.LdapUsername) ||
        //                            string.IsNullOrWhiteSpace(ldapOptions.LdapPassword)
        //            ? new PrincipalContext(ContextType.Domain, ldapOptions.LdapDomain)
        //            : new PrincipalContext(ContextType.Domain, ldapOptions.LdapDomain, ldapOptions.LdapUsername, ldapOptions.LdapPassword);

        //        var isValid = context.ValidateCredentials(username, password);
        //        return Task.FromResult(isValid);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApiException("server.invalidLdapSettings: " + ex.Message);
        //    }
        //}
        public Task<bool> ValidateAsync(
     string username,
     string? password,
     bool loginWithoutPassword,
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
                if (loginWithoutPassword)
                {
                    // Search for user without password
                    using var directory = new DirectoryEntry(
                        $"LDAP://{ldapOptions.LdapServer}",
                        ldapOptions.LdapUsername,
                        ldapOptions.LdapPassword
                    );

                    using var searcher = new DirectorySearcher(directory)
                    {
                        Filter = $"(&(objectClass=user)({ldapOptions.LdapEmpAttr}={username}))"
                    };

                    var found = searcher.FindOne();

                    if (found != null)
                        _logger.LogInformation("LDAP validation successful (no password). Username: {Username}", username);
                    else
                        _logger.LogWarning("LDAP validation failed (user not found, no password). Username: {Username}", username);

                    return Task.FromResult(found != null);
                }

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
