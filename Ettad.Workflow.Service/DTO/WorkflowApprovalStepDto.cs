using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.DTO
{
    public class WorkflowApprovalStepDto
    {
        public int Id { get; set; }

        public int WorkflowStepId { get; set; }         // link to WorkflowStep
        public int TargetRequestId { get; set; }

        public WorkflowType RequestType { get; set; }

        public string? ApproverRoleId { get; set; }     // instead of ApproverEmployeeId
        public int IsDelegation { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public RequestStatus Status { get; set; }

        public string? Comments { get; set; }

        public bool IsCurrent { get; set; }

        // Optional: For history tracking
        public RequestStatus? OldRequestStatus { get; set; }
        public RequestStatus? NewRequestStatus { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
    }



}
