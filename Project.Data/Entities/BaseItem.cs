using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public abstract class BaseItem : FullAuditEntity<long> // Base  for Ammunation , Explosive , Weapon , Accessory
    {
        public string Name { get; set; }

        public string? NameAr { get; set; }

        public string? ItemNo { get; set; }
       
        public ItemType ItemType { get; set; }

        public string? Nsn { get; set; }
        
        public string? PartNo { get; set; }

        public decimal? Price { get; set; }

        public long? MinimumQuantity { get; set; }

        public long? CriticalQuantity { get; set; }

        public long? MaximumStock { get; set; }

        public string? Distribution { get; set; }

        public string? ReferenceNo { get; set; }

        public string? UNNumber { get; set; }

        public string? Notes { get; set; }

        public long? ClassificationId { get; set; }

        public long? TypeId { get; set; }

        #region Navigation Properties

        public Classification Classification { get; set; }

        public ItemTypeLookup Type { get; set; }

        public ICollection<BaseItemPrimaryPurpos> BaseItemPrimaryPurposes { get; set; }

        #endregion
    }
}
