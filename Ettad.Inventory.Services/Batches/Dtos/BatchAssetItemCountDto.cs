namespace Ettad.Inventory.Service.Batches.Dtos
{
    /// <summary>
    /// Per-item-type totals within a batch for the same filter set as <see cref="BatchDto.Assets"/>,
    /// excluding any single-item drill-down used only for paging the asset list.
    /// </summary>
    public class BatchAssetItemCountDto
    {
        public long ItemId { get; set; }

        public string ItemName { get; set; } = string.Empty;

        public string? ItemNameAr { get; set; }

        public string? ItemNo { get; set; }

        public string? Nsn { get; set; }

        public int Count { get; set; }
    }
}
