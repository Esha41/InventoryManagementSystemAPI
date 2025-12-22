using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Workflows
{
    public class WorkflowStepTransition : AuditEntity<int>
    {
        public int SourceWorkflowStepId { get; set; }
        public int TargetWorkflowStepId { get; set; }

        public WorkflowStep SourceWorkflowStep { get; set; }
        public WorkflowStep TargetWorkflowStep { get; set; }
    }
}
