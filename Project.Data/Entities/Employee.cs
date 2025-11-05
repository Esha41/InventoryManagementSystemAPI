using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Employee : FullAuditEntity<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public long? RankId { get; set; }

        #region Navigation Properties
        public Rank Rank { get; set; }
        #endregion
    }
}
