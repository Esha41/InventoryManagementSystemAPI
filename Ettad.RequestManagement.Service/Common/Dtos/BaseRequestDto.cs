using Ettad.Data.Enums;

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
        public string? RecieverId { get; set; }
        public long? DepotId { get; set; }
        public long RequestPurposeId { get; set; }

        #region Navigation Properties (Simplified - just names/what makes sense)
        public string DepartmentName { get; set; }
        public string? RequesterName { get; set; }
        public string? RecieverName { get; set; }
        public string? DepotName { get; set; }
        public string RequestPurposeName { get; set; }
        public ICollection<RequestItemDto> RequestItems { get; set; }
        #endregion
    }
}
