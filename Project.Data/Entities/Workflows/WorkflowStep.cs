using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Entities.Workflows
{
    public class WorkflowStep : AuditEntity<long>
    {
        [Required]
        [ForeignKey("Workflow")]
        public long WorkflowId { get; set; }

        [Required]
        public int StepOrder { get; set; }

      
        [Required]
        public string ApplicationRoleId { get; set; }           // Role responsible for this step
        public ApplicationRole ApplicationRole { get; set; }    // Navigation property

        [Required]
        [ForeignKey("ApplicationEntity")]
        public long ApplicationEntityId { get; set; }                       // Reference to the entity being approved

        [Required]
        public bool MustApprove { get; set; } = true;

        [Required]
        public bool RequireHigherApproval { get; set; } = false;

        public string? HigherApprovalRoleId { get; set; }       // Optional higher approval
       
        public ApplicationRole? HigherApprovalRole { get; set; }

        public long? HigherApplicationEntityId { get; set; }
        public ApplicationEntity HigherApplicationEntity { get; set; }

        [Required]
        public bool ReserveQty { get; set; } = false;

        public bool CanSkip { get; set; } = false;
        public bool CanReturn { get; set; } = false;

        // Navigation properties
        public virtual Workflow Workflow { get; set; }       
        public virtual ICollection<WorkflowApprovalStep> ApprovalSteps { get; set; } = new List<WorkflowApprovalStep>();
        public virtual ICollection<WorkflowStepNotifier> Notifiers { get; set; } = new List<WorkflowStepNotifier>();
        public virtual ICollection<WorkflowStepTransition> Transitions { get; set; } = new List<WorkflowStepTransition>();
        /// <summary>Additional approver roles (any-of) alongside <see cref="ApplicationRoleId"/>.</summary>
        public virtual ICollection<WorkflowStepParallelRole> ParallelRoles { get; set; } = new List<WorkflowStepParallelRole>();
    }

}
