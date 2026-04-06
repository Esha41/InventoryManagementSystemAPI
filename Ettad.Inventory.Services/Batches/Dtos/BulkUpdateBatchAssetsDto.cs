using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Batches.Dtos
{
    public class BulkUpdateBatchAssetsDto
    {
        public List<BatchAssetUpdateItem> Items { get; set; } = new();
    }

    public class BatchAssetUpdateItem
    {
        public long AssetId { get; set; }

        public long ItemId { get; set; }

        public string? SerialNumber { get; set; }

        public string? RFID { get; set; }

        public AssetStatus? Status { get; set; }

        public string? AssetTag { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiryDate { get; set; }

        public string? Condition { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string? DeliveryReceipt { get; set; }

        public string? Notes { get; set; }
    }
}
