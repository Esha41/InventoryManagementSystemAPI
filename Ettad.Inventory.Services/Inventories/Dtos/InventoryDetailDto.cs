using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Inventories.Dtos
{
    public class InventoryDetailDto
    {
        public long Id { get; set; }

        public long ItemId { get; set; }

        public string Lot { get; set; } = string.Empty;

        public long InventoryId { get; set; }

        public long? SupplierId { get; set; }

        public long? ManufacturerId { get; set; }

        public long? CountryId { get; set; }

        public long OriginalQuantity { get; set; } // Original Quantity

        public string? BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int? YearOfManufacture { get; set; }

        public bool ReadyForIssue { get; set; }

        public long UsedQuantity { get; set; }
        public long ReservedQuantityByOrdersOnProcessing { get; set; }
        public long RemainingQuantity { get; set; }

        public long CurrentQuantity { get; set; }

        public bool IsLotEmpty { get; set; }

        // Invoice Information (from parent Inventory)
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? RecievedDate { get; set; }
        public string? ContractNumber { get; set; }
        public string? Notes { get; set; }

        #region Navigation Properties

        public BaseItemDto Item { get; set; }
        public SupplierDto Supplier { get; set; }
        public ManufacturerDto Manufacturer { get; set; }
        public CountryDto Country { get; set; }

        #endregion
    }
}
