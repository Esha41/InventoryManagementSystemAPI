using Ettad.Data.Interfaces;
using Moujam.Casiher.Comman.Base;

namespace BrzanData.Models
{
    public class Department : AuditEntity<int>, INameable
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
