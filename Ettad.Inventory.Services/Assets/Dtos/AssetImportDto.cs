namespace Ettad.Inventory.Service.Assets.Dtos
{
    public class AssetImportDto
    {
        public int RowNumber { get; set; }
        // Item identification (by name or ItemNo)
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public long? ItemId { get; set; }

        public string? BatchNumber { get; set; }

        // Asset identification
        public string? SerialNumber { get; set; }
        public string? RFID { get; set; }

        // Asset details
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string? Notes { get; set; }
    }
}

