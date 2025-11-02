using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class AllowanceItem : FullAuditEntity<long>
    {
        public long ItemId { get; set; }
        public long DepartmentId { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
        public ItemType ItemType { get; set; }

        #region Navigation Properties
        public BaseItem Item { get; set; }
        public Department Department { get; set; }
        #endregion
    }
}
