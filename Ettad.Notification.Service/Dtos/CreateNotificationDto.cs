namespace Ettad.Notification.Service.Dtos
{
    public class CreateNotificationDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? EntityType { get; set; } // Optional: Entity name (e.g., "Order", "Ammunition")
        public long? EntityId { get; set; } // Optional: ID of the related entity row
        public List<string>? UserIds { get; set; }
        public List<string>? RoleIds { get; set; }
        public string? SenderId { get; set; }
        public bool IncludeSuperAdmins { get; set; }
        /// <summary>When true and no UserIds/RoleIds, notify all users (e.g. broadcast announcements).</summary>
        public bool IncludeAllUsers { get; set; }
    }
}

