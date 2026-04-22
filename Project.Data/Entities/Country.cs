using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Interface;

namespace Ettad.Data.Entities
{
    public class Country : BaseEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
    }
}
