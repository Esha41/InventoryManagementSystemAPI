using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Interface;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Unit : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public ItemType? ItemType { get; set; } // Discriminator: Ammunition, Weapon, or Explosive
        public bool IsDeleted { get; set; }
    }
}
