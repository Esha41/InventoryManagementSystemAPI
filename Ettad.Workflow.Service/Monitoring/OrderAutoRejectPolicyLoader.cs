using Ettad.Data.Constants;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Monitoring;

public static class OrderAutoRejectPolicyLoader
{
    public static async Task<OrderAutoRejectEffectivePolicy?> LoadAsync(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger? logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var policy = await context.OrderAutoRejectPolicies
                .AsNoTracking()
                .Include(p => p.NotifyRoles)
                .Include(p => p.ReminderLeadDays)
                .FirstOrDefaultAsync(cancellationToken);

            if (policy != null)
            {
                var leads = policy.ReminderLeadDays.Select(x => x.LeadDays).Distinct().OrderBy(x => x).ToList();
                var notifyRoles = policy.NotifyRoles.Select(x => x.RoleId).Distinct().ToList();
                return new OrderAutoRejectEffectivePolicy(
                    policy.TriggerRoleId,
                    policy.ThresholdDays,
                    policy.IsEnabled,
                    policy.NotifyRequester,
                    leads,
                    notifyRoles);
            }
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "Reading OrderAutoRejectPolicies failed; falling back to legacy Settings.");
        }

        var triggerRoleId = await GetLegacyTriggerRoleIdAsync(context, cancellationToken);
        if (string.IsNullOrEmpty(triggerRoleId))
            return null;

        var threshold = await GetLegacyThresholdDaysAsync(context, configuration, cancellationToken);
        var defaultLead = Math.Min(7, Math.Max(0, threshold - 1));
        var leadsFallback = defaultLead >= 1 ? new List<int> { defaultLead } : new List<int>();

        return new OrderAutoRejectEffectivePolicy(
            triggerRoleId,
            threshold,
            IsEnabled: true,
            NotifyRequester: true,
            leadsFallback,
            Array.Empty<string>());
    }

    private static async Task<int> GetLegacyThresholdDaysAsync(
        ApplicationDbContext context,
        IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        var row = await context.Settings.AsNoTracking()
            .FirstOrDefaultAsync(
                s => s.Key == OrderAutoRejectConstants.ThresholdDaysKey && s.Group == OrderAutoRejectConstants.Group,
                cancellationToken);

        if (row != null && int.TryParse(row.Value, out var parsed) && parsed > 0)
            return parsed;

        var configVal = configuration.GetValue<int?>("BackgroundJobs:OrderAutoReject:ThresholdDays");
        if (configVal is > 0)
            return configVal.Value;

        return OrderAutoRejectConstants.DefaultThresholdDays;
    }

    private static async Task<string?> GetLegacyTriggerRoleIdAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var row = await context.Settings.AsNoTracking()
            .FirstOrDefaultAsync(
                s => s.Key == OrderAutoRejectConstants.TriggerRoleIdKey && s.Group == OrderAutoRejectConstants.Group,
                cancellationToken);

        if (string.IsNullOrWhiteSpace(row?.Value))
            return null;

        return row.Value.Trim();
    }
}
