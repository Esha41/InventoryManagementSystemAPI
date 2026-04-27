using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Enums;
using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Dtos
{
    public class BaseRequestDto
    {
        public long Id { get; set; }
        public string RequestNo { get; set; }
        public RequestType RequestType { get; set; }
        public string? Reason { get; set; }
        public RequestPriority Priority { get; set; }
        public RequestStatus Status { get; set; }
        public string? Notes { get; set; }

        public long DepartmentId { get; set; }
        public string RequesterId { get; set; }
        public long RequestPurposeId { get; set; }
        public DateTime RequestDate { get; set; }

        // Additional info
        public string? DepartmentName { get; set; }
        public string? DepartmentNameAr { get; set; }
        public string? DepartmentNameEn { get; set; }
        public string? RequesterName { get; set; }
        public string? RequesterNameEn { get; set; }
        public string? RequesterNameAr { get; set; }
        public string? RequesterUserName { get; set; }
        public string? RequestPurposeName { get; set; }
        public string? RequestPurposeNameAr { get; set; }
        public string? RequestPurposeNameEn { get; set; }


        // Lists
        public List<object> RequestItems { get; set; } = new List<object>();
        public List<ApprovalHistoryDto> ApprovalHistory { get; set; } = new List<ApprovalHistoryDto>();
        
        // Files attached to the request (Order/Return/Discard)
        public List<FileUploadDto> Files { get; set; } = new List<FileUploadDto>();
        
        // Usage related fields
        public string? UsageLocation { get; set; }
        public string? UsagePurpose { get; set; }
        public DateTime? UsageDateFrom { get; set; }
        public string? UsageTimeFrom { get; set; }
        public DateTime? UsageDateTo { get; set; }
        public string? UsageTimeTo { get; set; }
        public int? NumberOfOfficer { get; set; }
        public int? NumberOfOtherRank { get; set; }

        /// <summary>Pickup/supply date for orders (from Order.SupplyDate).</summary>
        public DateTime? SupplyDate { get; set; }
      
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
        public string? ApproverNameEn { get; set; }
        public string? ApproverNameAr { get; set; }
        public DateTime ChangedAt { get; set; }
        public int? StepOrder { get; set; }
        public string? ApplicationRoleId { get; set; }
        public string? ApplicationRoleName { get; set; }
        public string? ApplicationRoleNameAr { get; set; }
        public bool RequireHigherApproval { get; set; }
        public string? HigherApprovalRoleId { get; set; }
        public bool IsPending { get; set; } // True if this step hasn't been completed yet
        public bool IsCurrentUserApprover { get; set; } // True if current user can approve this step
        public bool CanReturn { get; set; }
        public bool IsDelegation { get; set; }

        /// <summary>Role id the user acted under (when completed).</summary>
        public string? ChangedByRoleId { get; set; }
        public string? ChangedByRoleName { get; set; }
        public string? ChangedByRoleNameAr { get; set; }

        /// <summary>Structured additional parallel approver roles for pending/future steps.</summary>
        public List<WorkflowStepParallelRoleDto> EligibleParallelRoles { get; set; } = new();

        /// <summary>Display of additional parallel approver roles for pending steps (EN).</summary>
        public string? EligibleParallelRoleNamesEn { get; set; }
        /// <summary>Display of additional parallel approver roles for pending steps (AR).</summary>
        public string? EligibleParallelRoleNamesAr { get; set; }

        public List<FileUploadDto> Files { get; set; } = new List<FileUploadDto>();
        public List<WorkflowStepTransitionDto> Transitions { get; set; } = new List<WorkflowStepTransitionDto>();
    }
}
