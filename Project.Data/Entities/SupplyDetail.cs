using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities   
{
    public class SupplyDetail : FullAuditEntity<long>
    {
        public long SupplyId { get; set; }
        public long ItemId { get; set; }
        public string Lot { get; set; } = string.Empty;
        public long Quantity { get; set; }
        public string? Notes { get; set; }

        #region Navigation Properties
        public Supply Supply { get; set; }
        public BaseItem Item { get; set; }
        #endregion
    }
}
