using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class RequestDetail : BaseEntity<long>
    {
        public long ItemId { get; set; }

        public long ItemQuantity { get; set; }

        public long RequestId { get; set; }

        #region Navigation Properties

        public BaseItem Item { get; set; }
        public Request Request { get; set; }


        #endregion
    }
}
