using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Ettad.Application.Common.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ettad.Data.Enums;
using SettingsEntity = Ettad.Data.Entities.Settings.Settings;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.User.Services.Services;

public class SettingsProvider : ISettingsProvider
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private const string EmailGroup = "EMAIL";

    public SettingsProvider(ApplicationDbContext dbContext, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task<EmailConfiguration> getEmailSettings(CancellationToken cancellationToken = default)
    {
        var settings = await _dbContext.Settings
            .AsNoTracking()
            .Where(s => s.Group == EmailGroup)
            .ToListAsync(cancellationToken);

        if (settings.Count == 0)
        {
            return new EmailConfiguration();
        }

        var map = settings
            .Where(s => !string.IsNullOrWhiteSpace(s.Key))
            .GroupBy(s => s.Key!, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToDictionary(s => s.Key!, s => s.Value ?? string.Empty, StringComparer.OrdinalIgnoreCase);

        return new EmailConfiguration
        {
            HostIp = TryGetString(map, "EmailHost", string.Empty),
            Port = TryGetInt(map, "EmailPort", 587),
            SSL = TryGetBool(map, "EmailSSL", false),
            Username = TryGetString(map, "EmailUsername", string.Empty),
            Password = TryGetString(map, "EmailPassword", string.Empty),
            DisplayName = TryGetString(map, "EmailSenderName", string.Empty),
            DisableAuthentication = !TryGetBool(map, "EmailEnabled", false)
        };
    }

    private static int TryGetInt(IReadOnlyDictionary<string, string> map, string key, int fallback)
    {
        if (map.TryGetValue(key, out var value) && int.TryParse(value, out var parsed))
        {
            return parsed;
        }

        return fallback;
    }

    public async Task<bool> SaveEmailSettings(EmailSettingsDto emailSettings, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = _currentUserService.UserId ?? "System";
            var existingSettings = await _dbContext.Settings
                .Where(s => s.Group == EmailGroup)
                .ToListAsync(cancellationToken);

            var settingsToUpdate = new Dictionary<string, SettingsEntity>();
            foreach (var setting in existingSettings)
            {
                if (!string.IsNullOrWhiteSpace(setting.Key))
                {
                    settingsToUpdate[setting.Key] = setting;
                }
            }

            var emailSettingKeys = new Dictionary<string, string>
            {
                { "EmailEnabled", emailSettings.EnableEmailNotifications.ToString() },
                { "EmailHost", emailSettings.Host },
                { "EmailPort", emailSettings.Port.ToString() },
                { "EmailSSL", emailSettings.EnableSSL.ToString() },
                { "EmailSenderName", emailSettings.SenderName },
                { "EmailUsername", emailSettings.AccountUsername }
            };

            if (!string.IsNullOrWhiteSpace(emailSettings.AccountPassword))
            {
                emailSettingKeys["EmailPassword"] = emailSettings.AccountPassword;
            }

            foreach (var kvp in emailSettingKeys)
            {
                if (settingsToUpdate.TryGetValue(kvp.Key, out var existingSetting))
                {
                    // Update existing setting
                    existingSetting.Value = kvp.Value;
                    existingSetting.ModificationDate = _dateTimeProvider.Now;
                    existingSetting.ModifiedBy = userId;
                    _dbContext.Settings.Update(existingSetting);
                }
                else
                {
                    // Create new setting
                    var newSetting = new SettingsEntity
                    {
                        Key = kvp.Key,
                        Value = kvp.Value,
                        Group = EmailGroup,
                        CreatedBy = userId
                        // CreationDate will be set automatically by AuditEntity
                    };
                    await _dbContext.Settings.AddAsync(newSetting, cancellationToken);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static string TryGetString(IReadOnlyDictionary<string, string> map, string key, string fallback)
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
