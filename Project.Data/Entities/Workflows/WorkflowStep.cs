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
    public class WorkflowStep : AuditEntity<int>
    {
     

        [Required]
        [ForeignKey("Workflow")]
        public int WorkflowId { get; set; }

        [Required]
        public int StepOrder { get; set; }

        public ApproverType? ApproverType { get; set; } 

        public int? ApproverEmployeeId { get; set; }

        [Required]
        public bool MustApprove { get; set; } = true;

     

        // Navigation properties
        public virtual Workflow Workflow { get; set; }
        public virtual ICollection<WorkflowApprovalHistory> ApprovalHistories { get; set; }
    }
}
