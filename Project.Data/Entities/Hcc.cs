using Ettad.Data.Interfaces;
using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Hcc : AuditEntity<int>, INameable
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
