using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class RequestPurpose : FullAuditEntity<long>
    {
        public string NameEn { get ; set; }
        public string NameAr { get ; set; }
        public RequestType RequestType { get ; set; }
        public RequestPurposeAllowanceContext AllowanceContext { get; set; }
    }
}
