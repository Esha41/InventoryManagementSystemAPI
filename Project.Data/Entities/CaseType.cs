using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ettad.Data.Interfaces;
using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities
{
    public class CaseType : AuditEntity<int>, INameable
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
