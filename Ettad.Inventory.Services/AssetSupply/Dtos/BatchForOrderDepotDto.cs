using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// Item info for a batch - which requested item(s) this batch contains.
    /// </summary>
    public class BatchItemDto
    {
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// DTO for a batch that contains requested items and is in one of the selected depots.
    /// Used in weapon supply review to let users select batches before viewing assets.
    /// </summary>
    public class BatchForOrderDepotDto
    {
        public long Id { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public long DepotId { get; set; }
        public string? DepotName { get; set; }
        /// <summary>
        /// Full depot DTO for localization (nameEn, nameAr, code, etc.)
        /// </summary>
        public DepotDto? Depot { get; set; }
        /// <summary>
        /// Which requested items this batch contains (with available asset count per item).
        /// </summary>
        public List<BatchItemDto> Items { get; set; } = new();
    }
}
