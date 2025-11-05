using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.RequestManagement.Service.Common.Dtos
{
    public class RequestItemDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public long RequestId { get; set; }
        public string Notes { get; set; }

        #region Navigation Properties

        public BaseItemDto Item { get; set; }

        #endregion
    }
}
