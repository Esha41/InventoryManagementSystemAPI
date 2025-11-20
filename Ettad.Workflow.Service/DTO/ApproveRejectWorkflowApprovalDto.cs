using Ettad.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ettad.Workflows.Service.DTO
{
    public class ApproveRejectWorkflowApprovalDto
    {
        [Required]
        public int BaseRequestID { get; set; }

        [Required]
        public bool IsApproved { get; set; } // true for approve, false for reject

        public string? Comments { get; set; }
        public bool SendToHigherApproval { get; set; }
        public RequestStatus Action { get; set; }


    }
}

