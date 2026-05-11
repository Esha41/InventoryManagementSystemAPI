using System.ComponentModel.DataAnnotations;

using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Idenitity;

namespace Ettad.Data.Entities.Workflows;

/// <summary>Junction: trigger → AspNetRoles (N:M).</summary>
public class WorkflowAutoRejectTriggerRole : AuditEntity<long>
{
    [Required]
    public long WorkflowAutoRejectTriggerId { get; set; }

    [Required]
    public string RoleId { get; set; } = null!;

    public virtual WorkflowAutoRejectTrigger Trigger { get; set; } = null!;
    /// <summary>Optional navigation — queries may omit Include.</summary>
    public virtual ApplicationRole? Role { get; set; }
}
