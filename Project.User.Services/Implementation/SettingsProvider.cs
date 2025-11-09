using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ettad.Application.Common.Models;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ettad.User.Services.Implementation;

public class SettingsProvider : ISettingsProvider
{
    private readonly ApplicationDbContext _dbContext;
    private readonly LdapOptions _fallbackLdapOptions;

    public SettingsProvider(ApplicationDbContext dbContext, IOptions<LdapOptions>? fallbackOptions = null)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _fallbackLdapOptions = fallbackOptions?.Value ?? new LdapOptions();
    }

    public async Task<LdapOptions> GetLdapSettings(CancellationToken cancellationToken = default)
    {
        var settings = await _dbContext.Settings
            .AsNoTracking()
            .Where(s => s.Group != null && s.Group == "LDAP")
            .ToListAsync(cancellationToken);

        if (settings.Count == 0)
        {
            return CloneLdapOptions(_fallbackLdapOptions);
        }

        var map = settings
            .Where(s => !string.IsNullOrWhiteSpace(s.Key))
            .GroupBy(s => s.Key!, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToDictionary(s => s.Key!, s => s.Value ?? string.Empty, StringComparer.OrdinalIgnoreCase);

        var ldapOptions = new LdapOptions
        {
            IsActive = TryGetBool(map, "LdapIsActive", _fallbackLdapOptions.IsActive),
            LdapServer = TryGetString(map, "LdapServer", _fallbackLdapOptions.LdapServer),
            LdapDomain = TryGetString(map, "LdapDomain", _fallbackLdapOptions.LdapDomain),
            LdapUsername = TryGetString(map, "LdapUsername", _fallbackLdapOptions.LdapUsername),
            LdapPassword = TryGetString(map, "LdapPassword", _fallbackLdapOptions.LdapPassword),
            LdapEmpAttr = TryGetString(map, "LdapEmpAttr", _fallbackLdapOptions.LdapEmpAttr ?? "sAMAccountName") ?? "sAMAccountName"
        };

        if (!map.ContainsKey("LdapIsActive") && !string.IsNullOrWhiteSpace(ldapOptions.LdapServer))
        {
            ldapOptions.IsActive = true;
        }

        return ldapOptions;
    }

    public Task<EmailConfiguration> getEmailSettings()
    {
        return Task.FromResult(new EmailConfiguration());
    }

    private static LdapOptions CloneLdapOptions(LdapOptions options)
    {
        return new LdapOptions
        {
            IsActive = options.IsActive,
            LdapServer = options.LdapServer,
            LdapDomain = options.LdapDomain,
            LdapUsername = options.LdapUsername,
            LdapPassword = options.LdapPassword,
            LdapEmpAttr = options.LdapEmpAttr
        };
    }

    private static string? TryGetString(IReadOnlyDictionary<string, string> map, string key, string? fallback)
    {
        return map.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : fallback;
    }

    private static bool TryGetBool(IReadOnlyDictionary<string, string> map, string key, bool fallback)
    {
        if (map.TryGetValue(key, out var value) && bool.TryParse(value, out var parsed))
        {
            return parsed;
        }

        return fallback;
    }
}
