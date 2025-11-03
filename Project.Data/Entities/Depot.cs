using Ettad.CrossCutting.Comman;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Depot : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Location { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsDeleted { get; set; }
    }
}
