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
    public class WorkflowStepApprovalLog : AuditEntity<int>
    {
       

        [Required]
        [ForeignKey("WorkflowApprovalStep")]
        public int WorkflowApprovalStepId { get; set; }


        public RequestStatus OldRequestStatus { get; set; }

        public RequestStatus NewRequestStatus { get; set; }

        public string? Comments { get; set; }

        public string? ChangedBy { get; set; }

        public DateTime ChangedAt { get; set; }

        // Navigation properties
        public virtual WorkflowStep WorkflowStep { get; set; }

    }
}
