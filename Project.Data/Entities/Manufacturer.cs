using Ettad.Data.Interfaces;
using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities
{
    //  public Organization Organization { get; set; }
    public class Manufacturer : AuditEntity<int>, INameable
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
