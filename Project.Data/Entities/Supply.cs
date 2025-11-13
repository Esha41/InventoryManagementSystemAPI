using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities   
{
    public class Supply : FullAuditEntity<long>
    {
        public long OrderId { get; set; }
        public DateTime SupplyDate { get; set; }
        public string RecieverName { get; set; }
        public long ReceiverRankId { get; set; }
        public string RecieverMilitaryId { get; set; }
        public string? Notes { get; set; }

        #region Navigation Properties

        public Order Order { get; set; }
        public Rank ReceiverRank { get; set; }
        public ICollection<SupplyDetails> SupplyDetails { get; set; }

        #endregion
    }
}
