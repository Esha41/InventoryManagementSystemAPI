using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Asset : FullAuditEntity<long>
    {
        public long ItemId { get; set; }

        public string? SerialNumber { get; set; }

        public string? RFID { get; set; }

        public long DepotId { get; set; }

        public long? DepartmentId { get; set; }

        public long? CustodianId { get; set; }

        public string? Location { get; set; }

        // Enhanced fields
        public AssetStatus? Status { get; set; }

        public string? AssetTag { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiryDate { get; set; }

        public string? Condition { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string? Notes { get; set; }

        #region Navigation Properties

        public BaseItem Item { get; set; }

        public Depot Depot { get; set; }

        public Department Department { get; set; }

        public Employee Custodian { get; set; }

        #endregion
    }
}

