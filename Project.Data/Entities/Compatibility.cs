using Ettad.CrossCutting.Comman;
using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Compatibility : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
