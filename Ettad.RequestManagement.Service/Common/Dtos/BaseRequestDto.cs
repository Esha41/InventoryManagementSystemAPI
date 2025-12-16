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
    }
}
