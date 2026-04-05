namespace Ettad.Inventory.Service.Inventories.Dtos
{
    public class CreateInventoryDto
    {
        public long DepoId { get; set; }

        public string? InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? RecievedDate { get; set; }

        public string? ContractNumber { get; set; }

        public string? Notes { get; set; }

        public List<CreateInventoryDetailDto> InventoryDetails { get; set; } = new();
    }

    public class CreateInventoryDetailDto
    {
        public long ItemId { get; set; }

        public string Lot { get; set; } = string.Empty;

        public long? SupplierId { get; set; }

        public long? ManufacturerId { get; set; }

        public long? CountryId { get; set; }

        public string? BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int? YearOfManufacture { get; set; }

        public bool ReadyForIssue { get; set; } = true;

        public long OriginalQuantity { get; set; }

        public long? PrimaryPurposId { get; set; }
    }
}
