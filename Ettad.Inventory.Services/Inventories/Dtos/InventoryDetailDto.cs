using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Inventories.Dtos
{
    public class InventoryDetailDto
    {
        public long Id { get; set; }

        public long ItemId { get; set; }

        public int Lot { get; set; }

        public long InventoryId { get; set; }

        public long? SupplierId { get; set; }

        public long? ManufacturerId { get; set; }

        public long? CountryId { get; set; }

        public long OriginalQuantity { get; set; } // Original Quantity

        public long UsedQuantity { get; set; }
        public long ReservedQuantityByOrdersOnProcessing { get; set; }
        public long RemainingQuantity { get; set; }

        public long CurrentQuantity { get; set; }

        public bool IsLotEmpty { get; set; }

        #region Navigation Properties

        public BaseItemDto Item { get; set; }
        public SupplierDto Supplier { get; set; }
        public ManufacturerDto Manufacturer { get; set; }
        public CountryDto Country { get; set; }

        #endregion
    }
}
