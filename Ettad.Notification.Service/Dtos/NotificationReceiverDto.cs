namespace Ettad.Notification.Service.Dtos
{
    public class NotificationReceiverDto
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}

