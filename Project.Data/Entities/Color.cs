using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Interfaces;

namespace Ettad.Data.Entities
{
    public class Color : BaseEntity<int>, INameable
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
