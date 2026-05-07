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
    public class WorkflowStepApprovalLog : AuditEntity<long>
    {
       

        [Required]
        [ForeignKey("WorkflowApprovalStep")]
        public long WorkflowApprovalStepId { get; set; }

        [ForeignKey("WorkflowStep")]
        public long? WorkflowStepId { get; set; }

        public RequestStatus OldRequestStatus { get; set; }

        public RequestStatus NewRequestStatus { get; set; }

        public string? Comments { get; set; }

        /// <summary>User id (AspNetUsers.Id) who performed the action.</summary>
        public string? ChangedBy { get; set; }

        /// <summary>AspNetRoles.Id for the role used when the action was taken.</summary>
        public string? ChangedByRoleId { get; set; }

        public DateTime ChangedAt { get; set; }

        // Navigation properties
        public virtual WorkflowStep WorkflowStep { get; set; }

    }
}
