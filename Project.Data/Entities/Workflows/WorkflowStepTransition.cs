using Ettad.CrossCutting.Comman.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ettad.Data.Entities.Workflows
{
    public class WorkflowStepTransition : AuditEntity<int>
    {
        [Required]
        [ForeignKey("SourceWorkflowStep")]
        public int SourceWorkflowStepId { get; set; }
        public WorkflowStep SourceWorkflowStep { get; set; }

        [Required]
        [ForeignKey("TargetWorkflowStep")]
        public int TargetWorkflowStepId { get; set; }
        public WorkflowStep TargetWorkflowStep { get; set; }
    }
}

