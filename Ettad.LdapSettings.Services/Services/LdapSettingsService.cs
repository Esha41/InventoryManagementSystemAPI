using Ettad.Application.Common.Interfaces;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.LdapSettings.Services.Interfaces;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SettingsEntity = Ettad.Data.Entities.Settings.Settings;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Constants;
using Ettad.LdapSettings.Services.Dtos;
using FluentValidation;

namespace Ettad.LdapSettings.Services.Services
{
    /// <summary>
    /// Persists and retrieves LDAP settings from the shared Settings table.
    /// </summary>
    public class LdapSettingsService : ILdapSettingsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly LdapOptions _fallbackLdapOptions;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IValidator<LdapOptions> _ldapOptionsValidator;

        public LdapSettingsService(
            ApplicationDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            IValidator<LdapOptions> ldapOptionsValidator,
            IOptions<LdapOptions>? fallbackOptions = null)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _ldapOptionsValidator = ldapOptionsValidator ?? throw new ArgumentNullException(nameof(ldapOptionsValidator));
            _fallbackLdapOptions = fallbackOptions?.Value ?? new LdapOptions();
        }

        public async Task<APIOperationResponse<bool>> SaveLdapSettings(LdapOptions ldapSettings, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = await _ldapOptionsValidator.ValidateAsync(ldapSettings, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var userId = _currentUserService.UserId ?? "System";

                var existingSettings = await _dbContext.Settings
                    .Where(s => s.Group == General.Group)
                    .ToListAsync(cancellationToken);

                var settingsToUpdate = new Dictionary<string, SettingsEntity>(StringComparer.OrdinalIgnoreCase);
                foreach (var setting in existingSettings)
                {
                    if (!string.IsNullOrWhiteSpace(setting.Key))
                    {
                        settingsToUpdate[setting.Key] = setting;
                    }
                }

                var ldapSettingKeys = new Dictionary<string, string?>
                {
                    { "LdapServer", ldapSettings.LdapServer },
                    { "LdapDomain", ldapSettings.LdapDomain },
                    { "LdapEmpAttr", ldapSettings.LdapEmpAttr ?? "sAMAccountName" },
                    { "LdapUsername", ldapSettings.LdapUsername },
                    { "LdapPassword", ldapSettings.LdapPassword },
                    { "LdapIsActive", ldapSettings.IsActive.ToString() }
                };

                foreach (var kvp in ldapSettingKeys)
                {
                    var value = kvp.Value ?? string.Empty;

                    if (settingsToUpdate.TryGetValue(kvp.Key, out var existingSetting))
                    {
                        existingSetting.Value = value;
                        existingSetting.ModificationDate = _dateTimeProvider.Now;
                        existingSetting.ModifiedBy = userId;
                        _dbContext.Settings.Update(existingSetting);
                    }
                    else
                    {
                        var newSetting = new SettingsEntity
                        {
                            Key = kvp.Key,
                            Value = value,
                            Group = General.Group,
                            CreatedBy = userId
                        };
                        await _dbContext.Settings.AddAsync(newSetting, cancellationToken);
                    }
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    "Failed to save LDAP settings: " + ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteLdapSettings(CancellationToken cancellationToken = default)
        {
            try
            {
                var ldapSettings = await _dbContext.Settings
                    .Where(s => s.Group == General.Group)
                    .ToListAsync(cancellationToken);

                if (ldapSettings.Count == 0)
                {
                    return APIOperationResponse<bool>.Success(true);
                }

                _dbContext.Settings.RemoveRange(ldapSettings);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    "Failed to delete LDAP settings: " + ex.Message);
            }
        }

        public async Task<APIOperationResponse<LdapOptions>> GetLdapSettings(CancellationToken cancellationToken = default)
        {
            try
            {
                var settings = await _dbContext.Settings
                    .AsNoTracking()
                    .Where(s => s.Group != null && s.Group == General.Group)
                    .ToListAsync(cancellationToken);

                LdapOptions ldapOptions;

                if (settings.Count == 0)
                {
                    ldapOptions = CloneLdapOptions(_fallbackLdapOptions);
                }
                else
                {
                    var map = settings
                        .Where(s => !string.IsNullOrWhiteSpace(s.Key))
                        .GroupBy(s => s.Key!, StringComparer.OrdinalIgnoreCase)
                        .Select(g => g.First())
                        .ToDictionary(s => s.Key!, s => s.Value ?? string.Empty, StringComparer.OrdinalIgnoreCase);

                    ldapOptions = new LdapOptions
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
                }

                return APIOperationResponse<LdapOptions>.Success(ldapOptions);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<LdapOptions>.Fail(
                    ResponseType.InternalServerError,
                    "Failed to retrieve LDAP settings: " + ex.Message);
            }
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
}
