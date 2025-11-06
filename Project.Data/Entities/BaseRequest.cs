using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class BaseRequest : FullAuditEntity<long>
    {
        public string RequestNo { get; set; }
        public RequestType RequestType { get; set; }
        public string Reason { get; set; }
        public RequestPriority Priority { get; set; }
        public RequestStatus Status { get; set; }
        public string Notes { get; set; }
        public long DepartmentId { get; set; }
        public long? RequesterId { get; set; }
        public long? RecieverId { get; set; }
        public long? DepotId { get; set; }
        public long RequestPurposeId { get; set; }

        #region Navigation Properties
        public Department Department { get; set; }
        public Employee Requester { get; set; }
        public Employee Reciever { get; set; }
        public Depot Depot { get; set; }
        public RequestPurpose RequestPurpose { get; set; }
        public ICollection<RequestItem> RequestItems { get; set; }
        #endregion
    }
}
