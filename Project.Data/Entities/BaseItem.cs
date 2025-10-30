using Ettad.Data.Enums;
using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities
{
    public abstract class BaseItem : AuditEntity<long> // Base  for Ammunation , Explosive , Weapon , Accessory
    {
        public string Name { get; set; }

        public string ItemNo { get; set; }
       
        public ItemType ItemType { get; set; }
       
        public int Lot { get; set; }
        
        public string BatchNo { get; set; }
        
        public long HccId  { get; set; }
        
        public long? SupplierId { get; set; }
        
        public long? CountryId { get; set; }
        
        public string PartNo { get; set; }

        public long? ManufacturerId { get; set; }

        public bool ReadyForIssue { get; set; } = true;

        public DateTime? ExpiryDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        #region Navigation Properties

        public Hcc Hcc { get; set; }
        public Supplier Supplier { get; set; }
        public Country Country { get; set; }
        public Manufacturer Manufacturer { get; set; }

        #endregion
    }
}
