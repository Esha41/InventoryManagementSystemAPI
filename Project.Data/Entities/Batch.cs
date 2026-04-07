using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    public class Batch : FullAuditEntity<long>
    {
        public string BatchNumber { get; set; } = string.Empty;

        public long DepotId { get; set; }

        #region Navigation Properties

        public Depot Depot { get; set; }
        public ICollection<Asset> Assets { get; set; }

        #endregion
    }
}
