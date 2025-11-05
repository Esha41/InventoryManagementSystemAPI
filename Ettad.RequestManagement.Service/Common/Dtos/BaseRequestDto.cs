using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;

namespace Ettad.RequestManagement.Service.Common.Dtos
{
    public class BaseRequestDto
    {
        public long Id { get; set; }
        public string RequestNo { get; set; }
        public string RequestType { get; set; }
        public string Reason { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public long DepartmentId { get; set; }
        public long? RequesterId { get; set; }
        public long? RecieverId { get; set; }
        public long? DepotId { get; set; }
        public long RequestPurposeId { get; set; }

        #region Navigation Properties
        public DepartmentDto Department { get; set; }
        public EmployeeDto Requester { get; set; }
        public EmployeeDto Reciever { get; set; }
        public DepotDto Depot { get; set; }
        public RequestPurposeDto RequestPurpose { get; set; }
        public ICollection<RequestItemDto> RequestItems { get; set; }
        #endregion
    }
}
