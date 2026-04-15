using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Tracks one return processing outcome (weapon → asset or ammo → inventory detail) for audit and attachments.
    /// </summary>
    public class ReturnTrackingLine : FullAuditEntity<long>
    {
        public long ReturnId { get; set; }

        public long RequestId { get; set; }

        public long DepotId { get; set; }

        public long? RequestItemId { get; set; }

        public long? ReturnedQuantity { get; set; }

        public long? ReceivedQuantity { get; set; }

        public string? Lot { get; set; }

        public string? BatchNumber { get; set; }

        public string? SerialNumber { get; set; }

        public string? Notes { get; set; }

        public long? AssetId { get; set; }

        public long? InventoryDetailId { get; set; }

        #region Navigation Properties

        public Return Return { get; set; }

        public BaseRequest Request { get; set; }

        public Depot Depot { get; set; }

        public RequestItem RequestItem { get; set; }

        public Asset Asset { get; set; }

        public InventoryDetail InventoryDetail { get; set; }

        #endregion
    }
}
