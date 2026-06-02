using Ettad.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ettad.Workflows.Service.Dtos
{
    public class ApproveRejectWorkflowApprovalDto
    {
        [Required]
        public long BaseRequestID { get; set; }

        [Required]
        public bool IsApproved { get; set; } // true for approve, false for reject

        public string? Comments { get; set; }
        public bool SendToHigherApproval { get; set; }
        public RequestStatus Action { get; set; }
        public long? NextStepId { get; set; }
        public long? ReturnToWorkflowStepId { get; set; }

        /// <summary>
        /// Optimistic-concurrency token: the WorkflowApprovalStep.Id the client was viewing when it
        /// submitted. When supplied and that step is no longer the current step (another approver
        /// already actioned it), the request is rejected with 409 Conflict instead of silently
        /// acting on whatever step is now current. Optional for backward compatibility.
        /// </summary>
        public long? ExpectedWorkflowApprovalStepId { get; set; }
    }
}

