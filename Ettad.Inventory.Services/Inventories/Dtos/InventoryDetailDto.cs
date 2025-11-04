using Ettad.Data.Entities;

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

        public long ItemQuantity { get; set; }

        public long CurrentQuantity { get; set; }

        #region Navigation Properties

        public BaseItem Item { get; set; }
        public Supplier Supplier { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Country Country { get; set; }

        #endregion
    }
}

