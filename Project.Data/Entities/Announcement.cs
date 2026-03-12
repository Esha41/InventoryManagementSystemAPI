using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Announcement : FullAuditEntity<long>
    {
        public string Message { get; set; }
        public RequestPriority Priority { get; set; }
        public AnnouncementDeliveryType DeliveryType { get; set; } = AnnouncementDeliveryType.Banner;
        public bool IsDismissable { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TargetRoles { get; set; } // JSON array of role IDs
        public bool IsActive { get; set; }

        #region Navigation Properties

        public ICollection<AnnouncementDismissal> Dismissals { get; set; } = new List<AnnouncementDismissal>();

        #endregion
    }
}
