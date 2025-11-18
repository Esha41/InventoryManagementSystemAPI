using System.ComponentModel.DataAnnotations;

namespace Ettad.Workflows.Service.DTO
{
    public class ApproveRejectWorkflowApprovalDto
    {
        [Required]
        public int WorkflowApprovalStepId { get; set; }

        [Required]
        public bool IsApproved { get; set; } // true for approve, false for reject

        public string? Comments { get; set; }
        
    }
}

