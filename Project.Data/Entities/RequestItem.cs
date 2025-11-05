using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class RequestItem : FullAuditEntity<long>
    {
        public long ItemId { get; set; }

        public long Quantity { get; set; }

        public long RequestId { get; set; }

        public string Notes { get; set; }

        #region Navigation Properties

        public BaseItem Item { get; set; }
        public BaseRequest Request { get; set; }

        #endregion
    }
}
