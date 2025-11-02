using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Interfaces;

namespace Ettad.Data.Entities
{
    public class Supplier : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
