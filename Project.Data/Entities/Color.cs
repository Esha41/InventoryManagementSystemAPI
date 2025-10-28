using Ettad.Data.Interfaces;
using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Color : BaseEntity<int>, INameable
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
