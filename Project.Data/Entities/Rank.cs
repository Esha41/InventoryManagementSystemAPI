using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Rank : AuditEntity<long>, ILookup
    {
        public string NameEn { get ; set; }
        public string NameAr { get ; set; }
        public bool IsDeleted { get ; set; }
    }
}
