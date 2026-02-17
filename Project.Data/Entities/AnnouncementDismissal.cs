using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class AnnouncementDismissal : BaseEntity<long>
    {
        public long AnnouncementId { get; set; }
        public string UserId { get; set; }
        public DateTime DismissedAt { get; set; }

        #region Navigation Properties

        public Announcement Announcement { get; set; }
        public ApplicationUser User { get; set; }

        #endregion
    }
}
