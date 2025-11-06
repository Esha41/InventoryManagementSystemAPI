namespace Ettad.RequestManagement.Service.Common.Dtos
{
    public class RequestItemDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public long RequestId { get; set; }
        public string? Notes { get; set; }

        #region Navigation Properties (Simplified)
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        #endregion
    }
}
