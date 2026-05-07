using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Workflows
{
    public class WorkflowStepTransition : AuditEntity<long>
    {
        public long SourceWorkflowStepId { get; set; }
        public long TargetWorkflowStepId { get; set; }

        public WorkflowStep SourceWorkflowStep { get; set; }
        public WorkflowStep TargetWorkflowStep { get; set; }
    }
}
