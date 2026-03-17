using System;
using System.Threading;
using System.Threading.Tasks;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SettingsEntity = Ettad.Data.Entities.Settings.Settings;

namespace Ettad.User.Services.Implementation;

/// <summary>
/// Manages maintenance mode via the Settings table.
/// </summary>
public class MaintenanceService : IMaintenanceService
{
    private const string MaintenanceGroup = "MAINTENANCE";
    private const string IsEnabledKey = "IsEnabled";

    private readonly ApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MaintenanceService(
        ApplicationDbContext dbContext,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    /// <inheritdoc />
    public async Task<APIOperationResponse<MaintenanceStatusDto>> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var setting = await _dbContext.Settings
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    s => s.Group == MaintenanceGroup && s.Key == IsEnabledKey,
                    cancellationToken);

            var isEnabled = false;
            if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
            {
                bool.TryParse(setting.Value, out isEnabled);
            }

            return APIOperationResponse<MaintenanceStatusDto>.Success(new MaintenanceStatusDto { IsEnabled = isEnabled });
        }
        catch (Exception ex)
        {
            return APIOperationResponse<MaintenanceStatusDto>.Fail(
                ResponseType.InternalServerError,
                "Failed to retrieve maintenance status: " + ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<APIOperationResponse<bool>> SetEnabledAsync(bool isEnabled, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = _currentUserService.UserId ?? "System";

            var setting = await _dbContext.Settings
                .FirstOrDefaultAsync(
                    s => s.Group == MaintenanceGroup && s.Key == IsEnabledKey,
                    cancellationToken);

            if (setting != null)
            {
                setting.Value = isEnabled.ToString();
                setting.ModificationDate = _dateTimeProvider.Now;
                setting.ModifiedBy = userId;
                _dbContext.Settings.Update(setting);
            }
            else
            {
                var newSetting = new SettingsEntity
                {
                    Key = IsEnabledKey,
                    Value = isEnabled.ToString(),
                    Group = MaintenanceGroup,
                    CreatedBy = userId
                };
                await _dbContext.Settings.AddAsync(newSetting, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return APIOperationResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return APIOperationResponse<bool>.Fail(
                ResponseType.InternalServerError,
                "Failed to update maintenance mode: " + ex.Message);
        }
    }
}
