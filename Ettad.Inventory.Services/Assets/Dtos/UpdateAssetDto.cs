using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Assets.Dtos
{
    public class UpdateAssetDto
    {
        public long ItemId { get; set; }

        public string? SerialNumber { get; set; }

        public string? RFID { get; set; }

        public AssetStatus? Status { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiryDate { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string? DeliveryReceipt { get; set; }

        public string? Notes { get; set; }
    }
}

