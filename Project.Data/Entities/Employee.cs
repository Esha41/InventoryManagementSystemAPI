using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Employee : FullAuditEntity<long>
    {
        public string? NameAr { get; set; }

        public string? NameEn { get; set; }

        public string? MilitaryId { get; set; }

        public long? DepartmentId { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Notes { get; set; }

        public long? RankId { get; set; }

        public string? UserId { get; set; }

        #region Navigation Properties

        public Department Department { get; set; }
        public Rank Rank { get; set; }
        public ApplicationUser User { get; set; }

        #endregion
    }
}

