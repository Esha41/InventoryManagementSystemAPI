using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Interface;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Lookup table for item types (WeaponType, AmmunitionType, ExplosiveType)
    /// The ItemType property distinguishes which category this type belongs to
    /// </summary>
    public class ItemTypeLookup : AuditEntity<long>, ILookup
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public ItemType ItemType { get; set; } // Discriminator: Weapon, Explosive, or Ammunition
        public bool IsDeleted { get; set; }
    }
}

