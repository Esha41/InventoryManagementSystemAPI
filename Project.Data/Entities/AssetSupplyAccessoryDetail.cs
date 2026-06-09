using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Accessory quantities supplied with a specific weapon asset line.
    /// </summary>
    public class AssetSupplyAccessoryDetail : FullAuditEntity<long>
    {
        public long AssetSupplyDetailId { get; set; }
        public long AccessoryId { get; set; }
        public long DefaultQuantity { get; set; }
        public long SuppliedQuantity { get; set; }

        #region Navigation Properties

        public AssetSupplyDetail AssetSupplyDetail { get; set; }
        public BaseItem Accessory { get; set; }

        #endregion
    }
}
