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
        public int TargetRequestId { get; set; }

        [Required]
        public WorkflowType RequestType { get; set; } // Using enum

        public string? ApproverUserId { get; set; }

        public int IsDelegation { get; set; }

        public DateTime? ApprovedDate { get; set; } = DateTime.Now;

        [Required]
        public RequestStatus Status { get; set; } // Using enum

        public string Comments { get; set; }

        public bool IsCurrent { get; set; }

        // Navigation properties
        public virtual WorkflowStep WorkflowStep { get; set; }

    }
}
