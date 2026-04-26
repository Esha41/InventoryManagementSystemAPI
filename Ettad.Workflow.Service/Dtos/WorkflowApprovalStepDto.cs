using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Dtos
{
    public class WorkflowApprovalStepDto
    {
        public int Id { get; set; }

        public int WorkflowStepId { get; set; }         // link to WorkflowStep
        public int TargetRequestId { get; set; }

        public WorkflowType RequestType { get; set; }

        public string? ApproverUserId { get; set; }
        public string? ApproverRoleId { get; set; }
        public int IsDelegation { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public RequestStatus Status { get; set; }

        public string? Comments { get; set; }

        public bool IsCurrent { get; set; }

        // When a step is returned for review, this tracks which step to return to after approval
        public int? ReturnToStepId { get; set; }

        // Optional: For history tracking
        public RequestStatus? OldRequestStatus { get; set; }
        public RequestStatus? NewRequestStatus { get; set; }
        public string? ChangedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
    }

    public class CreateWorkflowApprovalStepDto
    {
        [Required]
        public int WorkflowStepId { get; set; }

        [Required]
        public int TargetRequestId { get; set; }

        [Required]
        public WorkflowType RequestType { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        public string? ApproverUserId { get; set; }

        public int IsDelegation { get; set; } = 0;

        public string? Comments { get; set; }

        public bool IsCurrent { get; set; } = false;

        public string? CreatedBy { get; set; }
    }

    public class UpdateWorkflowApprovalStepDto
    {
        [Required]
        public int WorkflowStepId { get; set; }

        [Required]
        public int TargetRequestId { get; set; }

        [Required]
        public WorkflowType RequestType { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        public string? ApproverUserId { get; set; }

        public int IsDelegation { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string? Comments { get; set; }

        public bool IsCurrent { get; set; }

        public string? ChangedBy { get; set; }
    }

}
