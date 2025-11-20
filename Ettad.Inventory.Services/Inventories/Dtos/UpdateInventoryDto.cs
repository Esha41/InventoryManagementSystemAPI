namespace Ettad.Inventory.Service.Inventories.Dtos
{
    public class UpdateInventoryDto
    {
        public long DepoId { get; set; }

        public string? InvoiceNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? RecievedDate { get; set; }

        public string? Notes { get; set; }

        public List<UpdateInventoryDetailDto> InventoryDetails { get; set; } = new();
    }

    public class UpdateInventoryDetailDto
    {
        public long? Id { get; set; } // Nullable for new details being added during update

        public long ItemId { get; set; }

        public int Lot { get; set; }

        public long? SupplierId { get; set; }

        public long? ManufacturerId { get; set; }

        public long? CountryId { get; set; }

        public long ItemQuantity { get; set; }
    }
}

