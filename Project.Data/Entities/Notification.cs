using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Notification : FullAuditEntity<long>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? EntityType { get; set; } // Entity name (e.g., "Order", "Ammunition") - null for system notifications
        public long? EntityId { get; set; } // ID of the related entity row - null for system/broadcast notifications

        #region Navigation Properties
        public ICollection<NotificationReceiver> Receivers { get; set; } = new List<NotificationReceiver>();
        #endregion
    }
}

