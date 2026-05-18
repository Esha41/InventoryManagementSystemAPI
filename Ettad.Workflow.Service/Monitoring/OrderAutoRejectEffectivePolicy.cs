namespace Ettad.Workflows.Service.Monitoring;

/// <summary>
/// Resolved organization-wide auto-reject policy (typed table and/or legacy Settings fallback).
/// Timing, reminders, and notifications only; trigger anchor is per-workflow.
/// </summary>
public sealed record OrderAutoRejectEffectivePolicy(
    int ThresholdDays,
    bool IsEnabled,
    bool NotifyRequester,
    IReadOnlyList<int> ReminderLeadDays,
    IReadOnlyList<string> NotifyRoleIds);
