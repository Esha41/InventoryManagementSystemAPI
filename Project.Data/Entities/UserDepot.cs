using Ettad.Comman.Idenitity;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Junction entity for per-user depot access assignment.
    /// Users with entries here can see and access the assigned depots.
    /// </summary>
    public class UserDepot
    {
        public string UserId { get; set; } = string.Empty;
        public long DepotId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Depot Depot { get; set; } = null!;
    }
}
