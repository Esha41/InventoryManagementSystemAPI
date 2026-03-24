namespace Ettad.Inventory.Service.Inventories.Dtos
{
    /// <summary>
    /// DTO for importing inventory from Excel
    /// Maps Excel row to inventory import data
    /// </summary>
    public class InventoryImportRowDto
    {
        public int RowNumber { get; set; }
        public string? ItemName { get; set; } // "ItemName (ItemNo)" format from dropdown
        public string ItemNo { get; set; } = string.Empty;
        public long? ItemId { get; set; }
        public string Lot { get; set; } = string.Empty;
        
        // Changed from IDs to names for user-friendly Excel import
        public string? Supplier { get; set; }
        public string? Manufacturer { get; set; }
        public string? Country { get; set; }
        
        // Internal IDs (populated during import processing)
        public long? SupplierId { get; set; }
        public long? ManufacturerId { get; set; }
        public long? CountryId { get; set; }
        
        public long OriginalQuantity { get; set; }
        public string? BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool ReadyForIssue { get; set; } = true;
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? Notes { get; set; }
    }
}

