using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities.Workflows;

public class WorkflowAutoRejectTrigger : AuditEntity<long>
{
  
    [Required]
    public long WorkflowId { get; set; }

    public AutoRejectTriggerMode Mode { get; set; }

    public bool ResetOnReApproval { get; set; } = true;

    public virtual Workflow Workflow { get; set; } = null!;
    public virtual ICollection<WorkflowAutoRejectTriggerRole> TriggerRoles { get; set; } = new List<WorkflowAutoRejectTriggerRole>();
    public virtual ICollection<WorkflowAutoRejectTriggerStep> TriggerSteps { get; set; } = new List<WorkflowAutoRejectTriggerStep>();
}
