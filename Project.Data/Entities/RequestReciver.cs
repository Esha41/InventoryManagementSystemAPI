using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class RequestReciver : AuditEntity<long>
    {
        public string ReciverIdNo { get; set; }
        public string ReciverName { get; set; }
        public long ReciverRankId { get; set; }

        #region Navigation Properties
        public Rank Rank { get; set; }

        #endregion

    }
}
