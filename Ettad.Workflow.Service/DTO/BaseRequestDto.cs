using Ettad.Data.Enums;
using Ettad.CrossCutting.Comman.FileUpload;
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
        public string? Notes { get; set; }
        public long DepartmentId { get; set; }
        public string? RequesterId { get; set; }
        public long RequestPurposeId { get; set; }
        public string? DepartmentName { get; set; }
        public string? RequesterName { get; set; }
        public string? RequesterUserName { get; set; }
        public string? RequestPurposeName { get; set; }
        public List<FileUploadDto> Files { get; set; } = new List<FileUploadDto>();
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
        public string? ApplicationRoleName { get; set; }
        public bool RequireHigherApproval { get; set; }
        public string? HigherApprovalRoleId { get; set; }
        public bool IsPending { get; set; } // True if this step hasn't been completed yet
        public List<FileUploadDto> Files { get; set; } = new List<FileUploadDto>();
    }
}

