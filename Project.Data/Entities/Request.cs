using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Request : AuditEntity<long>
    {
        public string RequestNo { get; set; } 
        public string RequestDate { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public long DepotId { get; set; }
        public RequestPriority RequestPriority { get; set; }
        public RequestType RequestType { get; set; }
        public RequestPurpose RequestPurpose { get; set; }
        public long DepartmentId { get; set; }
        public long RequestReciverId { get; set; }
        public bool IsFromReserved { get; set; }
        public DateTime UsageDate { get; set; }
        public TimeOnly UsageTime { get; set; }
        public string UsePurpose { get; set; }
        public long AnnualDiscard { get; set; } //
        public string UsageLocation { get; set; }
        public int NumberOfOfficer { get; set; }
        public int NumberOfOtherRank { get; set; }
        public string RequesterName { get; set; }
        public long RequesterRankId { get; set; }
        public string RequesterIdNo { get; set; }
        public string Comment { get; set; }
        

        #region Navigation Properties
        public Depot Depo { get; set; }
        public Department Department { get; set; }
        public RequestReciver RequestReciver { get; set; }
        public Rank RequesterRank { get; set; }
        public ICollection<RequestDetail> ResquestDetails { get; set; }

        #endregion

    }
}

