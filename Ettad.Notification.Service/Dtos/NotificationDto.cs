namespace Ettad.Notification.Service.Dtos
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string? EntityType { get; set; } // Optional: Entity name (e.g., "Order", "Ammunition")
        public long? EntityId { get; set; } // Optional: ID of the related entity row
        public DateTime CreationDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public List<NotificationReceiverDto> Receivers { get; set; } = new List<NotificationReceiverDto>();
    }
}

