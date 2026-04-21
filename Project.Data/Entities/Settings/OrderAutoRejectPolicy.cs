using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Idenitity;

namespace Ettad.Data.Entities.Settings;

/// <summary>
/// Global singleton policy for order workflow auto-rejection (one row expected).
/// </summary>
public class OrderAutoRejectPolicy : AuditEntity<int>
{
    [Required]
    [ForeignKey(nameof(TriggerRole))]
    public string TriggerRoleId { get; set; }

    public virtual ApplicationRole TriggerRole { get; set; }

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
