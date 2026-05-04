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
        public long Id { get; set; }

        public long WorkflowStepId { get; set; }         // link to WorkflowStep
        public long TargetRequestId { get; set; }

        public WorkflowType RequestType { get; set; }

        public string? ApproverUserId { get; set; }
        public string? ApproverRoleId { get; set; }
        public bool IsDelegation { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public RequestStatus Status { get; set; }

        public string? Comments { get; set; }

        public bool IsCurrent { get; set; }

        // When a step is returned for review, this tracks which step to return to after approval
        public long? ReturnToStepId { get; set; }

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
        public long WorkflowStepId { get; set; }

        [Required]
        public long TargetRequestId { get; set; }

        [Required]
        public WorkflowType RequestType { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        public string? ApproverUserId { get; set; }

        public bool IsDelegation { get; set; } = false;

        public string? Comments { get; set; }

        public bool IsCurrent { get; set; } = false;

        public string? CreatedBy { get; set; }
    }

    public class UpdateWorkflowApprovalStepDto
    {
        [Required]
        public long WorkflowStepId { get; set; }

        [Required]
        public long TargetRequestId { get; set; }

        [Required]
        public WorkflowType RequestType { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        public string? ApproverUserId { get; set; }

        public bool IsDelegation { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string? Comments { get; set; }

        public bool IsCurrent { get; set; }

        public string? ChangedBy { get; set; }
    }

}
