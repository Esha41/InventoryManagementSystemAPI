using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Workflows
{
    /// <summary>
    /// When a configured workflow step is approved, the requester may be notified (see workflow handler).
    /// Rows are grouped by <see cref="WorkflowId"/>; saving configuration replaces all rows for that workflow only.
    /// </summary>
    public class WorkflowStepRequesterQuantityNotification : AuditEntity<long>
    {
        public long WorkflowId { get; set; }

        public long WorkflowStepId { get; set; }

        public virtual Workflow Workflow { get; set; }

        public virtual WorkflowStep WorkflowStep { get; set; }
    }
}
