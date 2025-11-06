namespace Ettad.RequestManagement.Service.Common.Dtos
{
    public class BaseRequestDto
    {
        public long Id { get; set; }
        public string RequestNo { get; set; }
        public string RequestType { get; set; }
        public string? Reason { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public long DepartmentId { get; set; }
        public long? RequesterId { get; set; }
        public long? RecieverId { get; set; }
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
