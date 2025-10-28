using Ettad.Data.Enums;
using Moujam.Casiher.Comman.Base;

namespace Ettad.Data.Entities
{
    public abstract class BaseItem : AuditEntity<long> //Base  for Ammunation , Explosive , Weapon , Accessory
    {
        public int ItemNo { get; set; }
       
        public ItemType ItemType { get; set; }
       
        public int Lot { get; set; }
        
        public string BatchNo { get; set; }
        
        public int HccId  { get; set; }
        
        public int SupplierId { get; set; }
        
        public int CountryId { get; set; }
        
        public int PartNo { get; set; }

        public string Depot { get; set; }

        public int? ManufacturerId { get; set; }

        #region Navigation Properties

        public Hcc Hcc { get; set; }
        public Supplier Supplier { get; set; }
        public Country Country { get; set; }
        public Manufacturer Manufacturer { get; set; }

        #endregion
    }
}
