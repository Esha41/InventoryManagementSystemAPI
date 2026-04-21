using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Settings;

public class OrderAutoRejectPolicyReminderDay : AuditEntity<int>
{
    [Required]
    public int PolicyId { get; set; }

    public virtual OrderAutoRejectPolicy Policy { get; set; }

    /// <summary>Send reminder when days remaining is less than or equal to this value (before threshold elapses).</summary>
    [Required]
    public int LeadDays { get; set; }
}
