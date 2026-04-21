namespace Ettad.Workflows.Service.Monitoring;

/// <summary>
/// Resolved global auto-reject policy (typed table and/or legacy Settings fallback).
/// </summary>
public sealed record OrderAutoRejectEffectivePolicy(
    string TriggerRoleId,
    int ThresholdDays,
    bool IsEnabled,
    bool NotifyRequester,
    IReadOnlyList<int> ReminderLeadDays,
    IReadOnlyList<string> NotifyRoleIds);
