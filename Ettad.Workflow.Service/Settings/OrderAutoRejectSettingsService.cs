using Cronos;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Constants;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Interfaces.Repositories;
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
    private static readonly string[] PolicyDetailIncludes =
    {
        nameof(OrderAutoRejectPolicy.NotifyRoles),
        nameof(OrderAutoRejectPolicy.ReminderLeadDays),
        nameof(OrderAutoRejectPolicy.TriggerRole),
    };

    private static readonly string[] PolicyEditIncludes =
    {
        nameof(OrderAutoRejectPolicy.NotifyRoles),
        nameof(OrderAutoRejectPolicy.ReminderLeadDays),
    };

    private readonly ICrossCuttingRepository<OrderAutoRejectPolicy> _policyRepository;
    private readonly ICrossCuttingRepository<OrderAutoRejectPolicyNotifyRole> _policyNotifyRoleRepository;
    private readonly ICrossCuttingRepository<OrderAutoRejectPolicyReminderDay> _policyReminderDayRepository;
    private readonly ICrossCuttingRepository<ApplicationRole> _roleRepository;
    private readonly ICrossCuttingRepository<Ettad.Data.Entities.Settings.Settings> _settingsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRecurringJobManager _recurringJobManager;

    public OrderAutoRejectSettingsService(
        ICrossCuttingRepository<OrderAutoRejectPolicy> policyRepository,
        ICrossCuttingRepository<OrderAutoRejectPolicyNotifyRole> policyNotifyRoleRepository,
        ICrossCuttingRepository<OrderAutoRejectPolicyReminderDay> policyReminderDayRepository,
        ICrossCuttingRepository<ApplicationRole> roleRepository,
        ICrossCuttingRepository<Ettad.Data.Entities.Settings.Settings> settingsRepository,
        ITransactionManager transactionManager,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider,
        IRecurringJobManager recurringJobManager)
    {
        _policyRepository = policyRepository;
        _policyNotifyRoleRepository = policyNotifyRoleRepository;
        _policyReminderDayRepository = policyReminderDayRepository;
        _roleRepository = roleRepository;
        _settingsRepository = settingsRepository;
        _transactionManager = transactionManager;
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        _recurringJobManager = recurringJobManager;
    }

    public async Task<OrderAutoRejectSettingsDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var policy = await LoadPolicyForDisplayAsync(cancellationToken);

        if (policy == null)
        {
            await TryMaterializePolicyFromLegacyAsync(cancellationToken);
            policy = await LoadPolicyForDisplayAsync(cancellationToken);
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

        var distinctReminders = dto.ReminderLeadDays.Distinct().OrderByDescending(x => x).ToList();
        var distinctNotifyRoles = dto.NotifyRoleIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();

        await using var transaction = await _transactionManager.BeginAsync(cancellationToken);
        try
        {
            var policy = await _policyRepository
                .Find(_ => true, false, PolicyEditIncludes)
                .OrderBy(p => p.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (policy == null)
            {
                policy = new OrderAutoRejectPolicy
                {
                    CreationDate = now,
                    CreatedBy = userId,
                    TriggerRoleId = dto.TriggerRoleId!.Trim(),
                    ThresholdDays = dto.ThresholdDays,
                    ScanCron = cron,
                    IsEnabled = dto.IsEnabled,
                    NotifyRequester = dto.NotifyRequester,
                };

                foreach (var lead in distinctReminders)
                {
                    policy.ReminderLeadDays.Add(new OrderAutoRejectPolicyReminderDay
                    {
                        LeadDays = lead,
                        CreationDate = now,
                        CreatedBy = userId
                    });
                }

                foreach (var roleId in distinctNotifyRoles)
                {
                    policy.NotifyRoles.Add(new OrderAutoRejectPolicyNotifyRole { RoleId = roleId });
                }

                await _policyRepository.AddAsync(policy);
            }
            else
            {
                var pid = policy.Id;

                foreach (var n in (await _policyNotifyRoleRepository.FindAsync(x => x.PolicyId == pid)).ToList())
                    await _policyNotifyRoleRepository.DeleteAsync(n);
                foreach (var r in (await _policyReminderDayRepository.FindAsync(x => x.PolicyId == pid)).ToList())
                    await _policyReminderDayRepository.DeleteAsync(r);

                policy.NotifyRoles.Clear();
                policy.ReminderLeadDays.Clear();

                policy.TriggerRoleId = dto.TriggerRoleId!.Trim();
                policy.ThresholdDays = dto.ThresholdDays;
                policy.ScanCron = cron;
                policy.IsEnabled = dto.IsEnabled;
                policy.NotifyRequester = dto.NotifyRequester;
                policy.ModificationDate = now;
                policy.ModifiedBy = userId;

                await _policyRepository.UpdateAsync(policy);

                foreach (var lead in distinctReminders)
                {
                    await _policyReminderDayRepository.AddAsync(new OrderAutoRejectPolicyReminderDay
                    {
                        PolicyId = pid,
                        LeadDays = lead,
                        CreationDate = now,
                        CreatedBy = userId
                    });
                }

                foreach (var roleId in distinctNotifyRoles)
                {
                    await _policyNotifyRoleRepository.AddAsync(new OrderAutoRejectPolicyNotifyRole
                    {
                        PolicyId = pid,
                        RoleId = roleId
                    });
                }
            }

            await _transactionManager.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transactionManager.RollbackAsync(cancellationToken);
            throw;
        }

        SyncRecurringJob(cron, dto.IsEnabled);
    }

    private async Task<OrderAutoRejectPolicy?> LoadPolicyForDisplayAsync(CancellationToken cancellationToken)
    {
        return await _policyRepository
            .Find(_ => true, false, PolicyDetailIncludes)
            .OrderBy(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task ValidateUpdateAsync(UpdateOrderAutoRejectSettingsDto dto, CancellationToken cancellationToken)
    {
        var failures = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(dto.TriggerRoleId))
            failures.Add(new ValidationFailure(nameof(dto.TriggerRoleId), "A trigger role must be selected."));
        else if (!await _roleRepository.Find(r => r.Id == dto.TriggerRoleId.Trim()).AnyAsync(cancellationToken))
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
            if (!await _roleRepository.Find(r => r.Id == roleId).AnyAsync(cancellationToken))
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
        var nameByRoleId = await _roleRepository
            .Find(r => notifyRoleIds.Contains(r.Id))
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
            : await _roleRepository.Find(r => r.Id == triggerId).Select(r => r.Name).FirstOrDefaultAsync(cancellationToken);

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

        if (!await _roleRepository.Find(r => r.Id == triggerId).AnyAsync(cancellationToken))
            return;

        if (await _policyRepository.Find(_ => true).AnyAsync(cancellationToken))
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

        await _policyRepository.AddAsync(policy);
    }

    private async Task<string?> GetStoredValueAsync(string key, CancellationToken _)
    {
        var row = await _settingsRepository.FindOneAsync(
            s => s.Key == key && s.Group == OrderAutoRejectConstants.Group);
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
