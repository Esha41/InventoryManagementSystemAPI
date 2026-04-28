using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Interface;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Caliber : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        /// <summary>Ammunition or Weapon only (<see cref="ItemType"/>).</summary>
        public ItemType ItemType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
