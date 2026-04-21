using Cronos;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities.Settings;
using SettingsEntity = Ettad.Data.Entities.Settings.Settings;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Workflows.Service.Monitoring;
using Ettad.Workflows.Service.Settings.Dtos;
using FluentValidation;
using FluentValidation.Results;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Workflows.Service.Settings;

public class OrderAutoRejectSettingsService : IOrderAutoRejectSettingsService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRecurringJobManager _recurringJobManager;

    public OrderAutoRejectSettingsService(
        ApplicationDbContext context,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IRecurringJobManager recurringJobManager)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        _recurringJobManager = recurringJobManager;
    }

    public async Task<OrderAutoRejectSettingsDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var policy = await _context.OrderAutoRejectPolicies
            .AsNoTracking()
            .Include(p => p.NotifyRoles)
            .Include(p => p.ReminderLeadDays)
            .Include(p => p.TriggerRole)
            .FirstOrDefaultAsync(cancellationToken);

        if (policy == null)
        {
            await TryMaterializePolicyFromLegacyAsync(cancellationToken);
            policy = await _context.OrderAutoRejectPolicies
                .AsNoTracking()
                .Include(p => p.NotifyRoles)
                .Include(p => p.ReminderLeadDays)
                .Include(p => p.TriggerRole)
                .FirstOrDefaultAsync(cancellationToken);
        }

        if (policy == null)
            return await BuildDtoFromLegacySettingsOnlyAsync(cancellationToken);

        return await MapToDtoAsync(policy, cancellationToken);
    }

    public async Task UpdateAsync(UpdateOrderAutoRejectSettingsDto dto, CancellationToken cancellationToken = default)
    {
        ValidateCron(dto.ScanCron);
        await ValidateUpdateAsync(dto, cancellationToken);

        var cron = dto.ScanCron.Trim();
        var now = _dateTimeProvider.Now;
        var userId = _currentUserService.UserId;

        var policy = await _context.OrderAutoRejectPolicies
            .Include(p => p.NotifyRoles)
            .Include(p => p.ReminderLeadDays)
            .FirstOrDefaultAsync(cancellationToken);

        if (policy == null)
        {
            policy = new OrderAutoRejectPolicy
            {
                CreationDate = now,
                CreatedBy = userId
            };
            _context.OrderAutoRejectPolicies.Add(policy);
        }
        else
        {
            _context.OrderAutoRejectPolicyNotifyRoles.RemoveRange(policy.NotifyRoles.ToList());
            policy.NotifyRoles.Clear();
            _context.OrderAutoRejectPolicyReminderDays.RemoveRange(policy.ReminderLeadDays.ToList());
            policy.ReminderLeadDays.Clear();
        }

        policy.TriggerRoleId = dto.TriggerRoleId!.Trim();
        policy.ThresholdDays = dto.ThresholdDays;
        policy.ScanCron = cron;
        policy.IsEnabled = dto.IsEnabled;
        policy.NotifyRequester = dto.NotifyRequester;
        policy.ModificationDate = now;
        policy.ModifiedBy = userId;

        var distinctReminders = dto.ReminderLeadDays.Distinct().OrderByDescending(x => x).ToList();
        foreach (var lead in distinctReminders)
        {
            policy.ReminderLeadDays.Add(new OrderAutoRejectPolicyReminderDay
            {
                LeadDays = lead,
                CreationDate = now,
                CreatedBy = userId
            });
        }

        var distinctNotifyRoles = dto.NotifyRoleIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();

        foreach (var roleId in distinctNotifyRoles)
        {
            policy.NotifyRoles.Add(new OrderAutoRejectPolicyNotifyRole
            {
                RoleId = roleId
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        SyncRecurringJob(cron, dto.IsEnabled);
    }

    private async Task ValidateUpdateAsync(UpdateOrderAutoRejectSettingsDto dto, CancellationToken cancellationToken)
    {
        var failures = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(dto.TriggerRoleId))
            failures.Add(new ValidationFailure(nameof(dto.TriggerRoleId), "A trigger role must be selected."));
        else if (!await _context.Roles.AnyAsync(r => r.Id == dto.TriggerRoleId.Trim(), cancellationToken))
            failures.Add(new ValidationFailure(nameof(dto.TriggerRoleId), "Unknown trigger role id."));

        if (dto.ThresholdDays < 1 || dto.ThresholdDays > 365)
            failures.Add(new ValidationFailure(nameof(dto.ThresholdDays), "Threshold must be between 1 and 365 inclusive."));

        var reminderDistinct = dto.ReminderLeadDays.Distinct().ToList();
        foreach (var lead in reminderDistinct)
        {
            if (lead < 1)
                failures.Add(new ValidationFailure(nameof(dto.ReminderLeadDays), "Each reminder lead must be at least 1 day."));
            else if (lead >= dto.ThresholdDays)
                failures.Add(new ValidationFailure(nameof(dto.ReminderLeadDays), $"Reminder lead {lead} must be less than threshold ({dto.ThresholdDays})."));
        }

        var notifyIds = dto.NotifyRoleIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();

        foreach (var roleId in notifyIds)
        {
            if (!await _context.Roles.AnyAsync(r => r.Id == roleId, cancellationToken))
                failures.Add(new ValidationFailure(nameof(dto.NotifyRoleIds), $"Unknown notify role id: {roleId}."));
        }

        if (failures.Count > 0)
            throw new ValidationException(failures);
    }

    private void SyncRecurringJob(string cron, bool isEnabled)
    {
        if (isEnabled)
        {
            var job = Job.FromExpression<OrderAutoRejectHangfireJob>(j => j.ExecuteAsync());
            _recurringJobManager.AddOrUpdate(
                OrderAutoRejectConstants.JobId,
                job,
                cron.Trim(),
                new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
        }
        else
            _recurringJobManager.RemoveIfExists(OrderAutoRejectConstants.JobId);
    }

    private async Task<OrderAutoRejectSettingsDto> MapToDtoAsync(OrderAutoRejectPolicy policy, CancellationToken cancellationToken)
    {
        var notifyRoleIds = policy.NotifyRoles.Select(n => n.RoleId).ToList();
        var nameByRoleId = await _context.Roles.AsNoTracking()
            .Where(r => notifyRoleIds.Contains(r.Id))
            .ToDictionaryAsync(r => r.Id, r => r.Name, cancellationToken);

        return new OrderAutoRejectSettingsDto
        {
            TriggerRoleId = policy.TriggerRoleId,
            TriggerRoleName = policy.TriggerRole?.Name,
            ThresholdDays = policy.ThresholdDays,
            ScanCron = policy.ScanCron,
            IsEnabled = policy.IsEnabled,
            NotifyRequester = policy.NotifyRequester,
            ReminderLeadDays = policy.ReminderLeadDays.OrderByDescending(x => x.LeadDays).Select(x => x.LeadDays).ToList(),
            NotifyRoles = notifyRoleIds
                .Select(id => new RoleRefDto { Id = id, Name = nameByRoleId.GetValueOrDefault(id) })
                .ToList()
        };
    }

    private async Task<OrderAutoRejectSettingsDto> BuildDtoFromLegacySettingsOnlyAsync(CancellationToken cancellationToken)
    {
        var triggerIdRaw = await GetStoredValueAsync(OrderAutoRejectConstants.TriggerRoleIdKey, cancellationToken);
        var thresholdStr = await GetStoredValueAsync(OrderAutoRejectConstants.ThresholdDaysKey, cancellationToken);
        var cron = await GetStoredValueAsync(OrderAutoRejectConstants.ScanCronKey, cancellationToken);

        string? triggerId = string.IsNullOrWhiteSpace(triggerIdRaw) ? null : triggerIdRaw.Trim();
        var threshold = int.TryParse(thresholdStr, out var td) ? td : OrderAutoRejectConstants.DefaultThresholdDays;
        if (string.IsNullOrWhiteSpace(cron))
            cron = OrderAutoRejectConstants.DefaultCronExpression;

        var roleName = string.IsNullOrEmpty(triggerId)
            ? null
            : (await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == triggerId, cancellationToken))?.Name;

        return new OrderAutoRejectSettingsDto
        {
            TriggerRoleId = triggerId,
            TriggerRoleName = roleName,
            ThresholdDays = threshold,
            ScanCron = cron ?? OrderAutoRejectConstants.DefaultCronExpression,
            IsEnabled = !string.IsNullOrEmpty(triggerId),
            NotifyRequester = true,
            ReminderLeadDays = new List<int> { 7 },
            NotifyRoles = new List<RoleRefDto>()
        };
    }

    /// <summary>
    /// One-time import from legacy Settings key/value rows when typed tables are empty.
    /// </summary>
    private async Task TryMaterializePolicyFromLegacyAsync(CancellationToken cancellationToken)
    {
        var triggerIdRaw = await GetStoredValueAsync(OrderAutoRejectConstants.TriggerRoleIdKey, cancellationToken);
        var triggerId = string.IsNullOrWhiteSpace(triggerIdRaw) ? null : triggerIdRaw.Trim();
        if (string.IsNullOrEmpty(triggerId))
            return;

        if (!await _context.Roles.AnyAsync(r => r.Id == triggerId, cancellationToken))
            return;

        if (await _context.OrderAutoRejectPolicies.AnyAsync(cancellationToken))
            return;

        var thresholdStr = await GetStoredValueAsync(OrderAutoRejectConstants.ThresholdDaysKey, cancellationToken);
        var threshold = int.TryParse(thresholdStr, out var td) && td > 0 ? td : OrderAutoRejectConstants.DefaultThresholdDays;
        var cronRaw = await GetStoredValueAsync(OrderAutoRejectConstants.ScanCronKey, cancellationToken);
        var cron = string.IsNullOrWhiteSpace(cronRaw) ? OrderAutoRejectConstants.DefaultCronExpression : cronRaw.Trim();

        var now = _dateTimeProvider.Now;
        var userId = _currentUserService.UserId;

        var policy = new OrderAutoRejectPolicy
        {
            TriggerRoleId = triggerId,
            ThresholdDays = threshold,
            ScanCron = cron,
            IsEnabled = true,
            NotifyRequester = true,
            CreationDate = now,
            CreatedBy = userId
        };

        var defaultLead = Math.Min(7, threshold - 1);
        if (defaultLead >= 1)
        {
            policy.ReminderLeadDays.Add(new OrderAutoRejectPolicyReminderDay
            {
                LeadDays = defaultLead,
                CreationDate = now,
                CreatedBy = userId
            });
        }

        _context.OrderAutoRejectPolicies.Add(policy);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<string?> GetStoredValueAsync(string key, CancellationToken cancellationToken)
    {
        var row = await _context.Settings.AsNoTracking()
            .FirstOrDefaultAsync(
                s => s.Key == key && s.Group == OrderAutoRejectConstants.Group,
                cancellationToken);
        return row?.Value;
    }

    private static void ValidateCron(string cron)
    {
        if (string.IsNullOrWhiteSpace(cron)
            || !CronExpression.TryParse(cron.Trim(), CronFormat.Standard, out _))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(UpdateOrderAutoRejectSettingsDto.ScanCron), "Invalid cron expression.")
            });
        }
    }
}
