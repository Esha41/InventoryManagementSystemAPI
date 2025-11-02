using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public abstract class BaseItem : AuditEntity<long> // Base  for Ammunation , Explosive , Weapon , Accessory
    {
        public string Name { get; set; }

        public string ItemNo { get; set; }
       
        public ItemType ItemType { get; set; }
       
        public string BatchNo { get; set; }
        
        public long HccId  { get; set; }
        
        public string PartNo { get; set; }

        public bool ReadyForIssue { get; set; } = true;

        public DateTime? ExpiryDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        #region Navigation Properties

        public Hcc Hcc { get; set; }

        #endregion
    }
}
