using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Settings;

/// <summary>
/// Global singleton policy for order workflow auto-rejection (one row expected).
/// Threshold, reminders, notifications, and scan schedule; trigger anchor is per-workflow.
/// </summary>
public class OrderAutoRejectPolicy : AuditEntity<int>
{
    [Required]
    public int ThresholdDays { get; set; }

    [Required]
    public string ScanCron { get; set; }

    public bool IsEnabled { get; set; } = true;

    /// <summary>When true, the original requester is included in reminder and rejection notifications.</summary>
    public bool NotifyRequester { get; set; } = true;

    public virtual ICollection<OrderAutoRejectPolicyNotifyRole> NotifyRoles { get; set; } = new List<OrderAutoRejectPolicyNotifyRole>();

    public virtual ICollection<OrderAutoRejectPolicyReminderDay> ReminderLeadDays { get; set; } = new List<OrderAutoRejectPolicyReminderDay>();
}
