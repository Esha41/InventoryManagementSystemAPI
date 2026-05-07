using Ettad.Data.Enums;
using System;

namespace Ettad.Workflows.Service.Dtos
{
    public class WorkflowApprovalWithOrderDto
    {
        // WorkflowStep properties (wfs)
        public long WorkflowId { get; set; }
        public int StepOrder { get; set; }
        public string ApplicationRoleId { get; set; }
        public long ApplicationEntityId { get; set; }
        public bool RequireHigherApproval { get; set; }
        public string? HigherApprovalRoleId { get; set; }
        public long? HigherApplicationEntityId { get; set; }
        public bool ReserveQty { get; set; }

        // WorkflowApprovalStep properties (ws)
        public long WorkflowStepId { get; set; }
        public long TargetRequestId { get; set; }
        public WorkflowType RequestType { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public RequestStatus Status { get; set; }
        public string? Comments { get; set; }
        public bool IsCurrent { get; set; }
        public string? ApproverUserId { get; set; }
        public bool IsDelegation { get; set; }

        // BaseRequest properties (br)
        public long BaseRequestId { get; set; }
        public string RequestNo { get; set; }
        public RequestType BaseRequestType { get; set; }
        public string Reason { get; set; }
        public RequestPriority Priority { get; set; }
        public RequestStatus BaseRequestStatus { get; set; }
        public string Notes { get; set; }
        public long DepartmentId { get; set; }
        public string? RequesterId { get; set; }
        public long RequestPurposeId { get; set; }
        public long WorkflowApprovalStepId { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
