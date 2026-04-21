using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.FileUpload;
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
        public string RequestPurposeNotes { get; set; } = string.Empty;
        public long DepartmentId { get; set; }
        public string? RequesterId { get; set; }
        public long RequestPurposeId { get; set; }

        #region Navigation Properties
        public Department Department { get; set; }
        public ApplicationUser Requester { get; set; }
        public RequestPurpose RequestPurpose { get; set; }
        public ICollection<RequestItem> RequestItems { get; set; }
        #endregion
    }
}
