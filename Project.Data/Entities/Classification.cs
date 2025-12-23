using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Classification : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

