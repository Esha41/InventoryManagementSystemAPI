using Ettad.CrossCutting.Comman.Base;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;

namespace Ettad.Data.Entities
{
    public class NotificationReceiver : BaseEntity<long>
    {
        public long NotificationId { get; set; }
        public string? UserId { get; set; } // ApplicationUser.Id
        public string? RoleId { get; set; } // ApplicationRole.Id
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }

        #region Navigation Properties
        public Notification Notification { get; set; }
        public ApplicationUser? User { get; set; }
        public ApplicationRole? Role { get; set; }
        #endregion
    }
}

