using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Interfaces;

namespace Ettad.Data.Entities
{
    public class Country : BaseEntity<int>, INameable
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
    }
}
