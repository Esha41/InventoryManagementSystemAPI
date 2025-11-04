using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Requests.Dtos
{
    public class CreateUpdateRequestDto
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
        public long AnnualDiscard { get; set; }
        public string UsageLocation { get; set; }
        public int NumberOfOfficer { get; set; }
        public int NumberOfOtherRank { get; set; }
        public string RequesterName { get; set; }
        public long RequesterRankId { get; set; }
        public string RequesterIdNo { get; set; }
        public string Comment { get; set; }

        public List<CreateUpdateRequestDetailDto> RequestDetails { get; set; } = new();
    }
}
