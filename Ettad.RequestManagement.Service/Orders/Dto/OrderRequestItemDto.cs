using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Orders.Dto
{
    public class OrderRequestItemDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public string Notes { get; set; }

        #region Item Navigation Properties
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public ItemType ItemType { get; set; }
        #endregion
    }
}

