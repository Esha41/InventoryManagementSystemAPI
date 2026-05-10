using System.ComponentModel.DataAnnotations;

using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Workflows;

/// <summary>Junction: trigger → workflow steps (N:M).</summary>
public class WorkflowAutoRejectTriggerStep : AuditEntity<long>
{
    [Required]
    public long WorkflowAutoRejectTriggerId { get; set; }

    [Required]
    public long WorkflowStepId { get; set; }

    public virtual WorkflowAutoRejectTrigger Trigger { get; set; } = null!;
    public virtual WorkflowStep WorkflowStep { get; set; } = null!;
}
