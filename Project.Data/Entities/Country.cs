using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Interfaces;

namespace Ettad.Data.Entities
{
    public class Country : BaseEntity<int>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
    }
}
