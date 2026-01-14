using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Workflows
{
    public class WorkflowApprovalHistory : AuditEntity<int>
    {
       

        [Required]
        [ForeignKey("WorkflowStep")]
        public int WorkflowStepId { get; set; }

        [Required]
        public int TargetRequestId { get; set; }

        [Required]
        public WorkflowType RequestType { get; set; } // Using enum

        public int? ApproverEmployeeId { get; set; }

        public int IsDelagation { get; set; } // Using enum

        public DateTime? ApprovedDate { get; set; }

        [Required]
        public ApprovalStatus Status { get; set; } // Using enum

        public string Comments { get; set; }

      

        // Navigation properties
        public virtual WorkflowStep WorkflowStep { get; set; }
    }
}
