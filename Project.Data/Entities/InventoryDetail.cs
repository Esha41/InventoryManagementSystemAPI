using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities   
{
    public class InventoryDetail : BaseEntity<long>
    {
        public long ItemId { get; set; }

        public int Lot { get; set; }

        public long InventoryId { get; set; }

        public long? SupplierId { get; set; }

        public long? ManufacturerId { get; set; }

        public long? CountryId { get; set; }

        public long ItemQuantity { get; set; }

        public string? BatchNo { get; set; }

        public bool ReadyForIssue { get; set; } = true;

        public int? YearOfManufacture { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool IsLotEmpty { get; set; } = false;

        #region Navigation Properties

        public BaseItem Item { get; set; }
        public Inventory Inventory { get; set; }
        public Supplier Supplier { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Country Country { get; set; }


        #endregion
    }
}
