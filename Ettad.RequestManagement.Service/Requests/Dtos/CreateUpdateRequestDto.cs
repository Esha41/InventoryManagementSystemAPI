using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.RequestManagement.Service.Requests.Dtos
{
    public class CreateUpdateRequestDto
    {
        public string RequestNo { get; set; }
        public string RequestDate { get; set; }
        public string RequestStatus { get; set; }
        public long DepotId { get; set; }
        public string RequestPriority { get; set; }
        public string RequestKind { get; set; }
        public long DepartmentId { get; set; }
        public long RequestReciverId { get; set; }
        public bool IsFromReserved { get; set; }
        public string UsageDate { get; set; }
        public string UsageTime { get; set; }
        public string UsePurpose { get; set; }
        public bool AnnualDiscard { get; set; }
        public string UsageLocation { get; set; }
        public int NumberOfOfficer { get; set; }
        public int NumberOfOtherRank { get; set; }
        public string RequesterName { get; set; }
        public string RequesterRank { get; set; }
        public string RequesterIdNo { get; set; }
        public string Comment { get; set; }
    }
}
