using Ettad.Data.Enums;
using Ettad.Module.lookup.Dtos;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;

namespace Ettad.RequestManagement.Service.Common.Dtos
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
        public string? RequesterId { get; set; }
        public long RequestPurposeId { get; set; }

        public DepartmentDto Department { get; set; }
        public RequesterDto Requester { get; set; }
        public RequestPurposeDto RequestPurpose { get; set; }

        public ICollection<RequestItemDto> RequestItems { get; set; }
        
        // Audit fields
        public DateTime CreationDate { get; set; }

        // Order-specific fields (nullable for Return/Discard requests)
        public DateTime? UsageDateFrom { get; set; }
        public DateTime? UsageDateTo { get; set; }
        public TimeOnly? UsageTimeFrom { get; set; }
        public TimeOnly? UsageTimeTo { get; set; }
        public string? UsagePurpose { get; set; }
        public string? UsageLocation { get; set; }
        public bool? IsFromAllowance { get; set; }
        public bool? AnnualDiscard { get; set; }
        public int? NumberOfOfficer { get; set; }
        public int? NumberOfOtherRank { get; set; }
        public long? DepotId { get; set; }
        public string? DepotNameAr { get; set; }
        public string? DepotNameEn { get; set; }
        public string? ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
    }
}
