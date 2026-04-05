using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Assets.Dtos
{
    public class CreateAssetDto
    {
        public long ItemId { get; set; }

        public string BatchNumber { get; set; } = string.Empty;

        public string? SerialNumber { get; set; }

        public string? RFID { get; set; }

        public long DepotId { get; set; }

        public string? AssetTag { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiryDate { get; set; }

        public string? Condition { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string? DeliveryReceipt { get; set; }

        public string? Notes { get; set; }
    }
}

