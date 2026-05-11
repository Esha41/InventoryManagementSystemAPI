using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Assets.Dtos
{
    /// <summary>
    /// Minimal row for catalog items that have tracked assets (one row per item, paged).
    /// </summary>
    public class AssetItemCatalogSummaryDto
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public string Nsn { get; set; }
        public string PartNo { get; set; }
        public ItemType ItemType { get; set; }
        /// <summary>Count of non-deleted assets for this catalog item in the requested depot scope.</summary>
        public long TotalAssets { get; set; }
    }
}
