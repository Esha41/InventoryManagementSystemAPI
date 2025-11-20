using Ettad.Data.Enums;
using System;
using System.Collections.Generic;

namespace Ettad.Workflows.Service.DTO
{
    public class BaseRequestDto
    {
        public long Id { get; set; }
        public string RequestNo { get; set; }
        public RequestType RequestType { get; set; }
        public string Reason { get; set; }
        public RequestPriority Priority { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public List<ApprovalHistoryDto> ApprovalHistory { get; set; } = new List<ApprovalHistoryDto>();
    }

    public class ApprovalHistoryDto
    {
        public int Id { get; set; }
        public int WorkflowApprovalStepId { get; set; }
        public int? WorkflowStepId { get; set; }
        public RequestStatus OldRequestStatus { get; set; }
        public RequestStatus NewRequestStatus { get; set; }
        public string? Comments { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public int? StepOrder { get; set; }
        public string? ApplicationRoleId { get; set; }
    }
}

