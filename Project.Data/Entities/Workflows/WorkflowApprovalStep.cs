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
    public class WorkflowApprovalStep: AuditEntity<int>    
    {
        [Required]
        [ForeignKey("WorkflowStep")]
        public int WorkflowStepId { get; set; }

        [Required]
        public long TargetRequestId { get; set; }

        [Required]
        public WorkflowType RequestType { get; set; } // Using enum

        public string? ApproverUserId { get; set; }

        /// <summary>AspNetRoles.Id for the role the user was acting under when this step was completed.</summary>
        public string? ApproverRoleId { get; set; }

        public bool IsDelegation { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [Required]
        public RequestStatus Status { get; set; } // Using enum

        public string Comments { get; set; }

        public bool IsCurrent { get; set; }

        /// <summary>
        /// When set, a pre-expiry warning was already sent for this approval step (order auto-reject monitor).
        /// </summary>
        public DateTime? ExpirationWarningSentAt { get; set; }

        // When a step is returned for review, this tracks which step to return to after approval
        public int? ReturnToStepId { get; set; }

        // Navigation properties
        public virtual WorkflowStep WorkflowStep { get; set; }

        public virtual ICollection<WorkflowApprovalStepReminder> Reminders { get; set; } = new List<WorkflowApprovalStepReminder>();
    }
}
