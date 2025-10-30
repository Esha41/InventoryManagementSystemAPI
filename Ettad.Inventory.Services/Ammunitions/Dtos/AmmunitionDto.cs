using Ettad.Data.Entities;

namespace Ettad.Inventory.Services.Ammunitions.Dtos
{
    public class AmmunitionDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ItemNo { get; set; }

        public int Lot { get; set; }

        public string BatchNo { get; set; }

        public long HccId { get; set; }

        public long? SupplierId { get; set; }

        public long? CountryId { get; set; }

        public string PartNo { get; set; }

        public long? ManufacturerId { get; set; }

        public bool ReadyForIssue { get; set; } = true;

        public DateTime? ExpiryDate { get; set; }

        public decimal BulletDiameter { get; set; }

        public long BulletDiameterUnitId { get; set; }

        public decimal CaseLength { get; set; }

        public long CaseLengthUnitId { get; set; }

        public bool IsLinked { get; set; }

        public string Primer { get; set; }

        public decimal TotalWeight { get; set; }

        public long? NatureOptionId { get; set; }

        public long NsnId { get; set; }

        public long? PrimaryPurposId { get; set; }

        public long? ProjectileColorId { get; set; }

        public long? ProjectailMaterialId { get; set; }

        public long CaseTypeId { get; set; }

        public long PropellantId { get; set; }

        public long CompatibilityId { get; set; }

        public long HazardDivisionId { get; set; }

        #region Navigation Properties

        public Hcc Hcc { get; set; }
        public Supplier Supplier { get; set; }
        public Country Country { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Unit BulletDiameterUnit { get; set; }
        public Unit CaseLengthUnit { get; set; }
        public NatureOption NatureOption { get; set; }
        public Nsn Nsn { get; set; }
        public PrimaryPurpos PrimaryPurpos { get; set; }
        public Color ProjectileColor { get; set; }
        public ProjectailMaterial ProjectailMaterial { get; set; }
        public CaseType CaseType { get; set; }
        public Propellant Propellant { get; set; }
        public Compatibility Compatibility { get; set; }
        public HazardDivision HazardDivision { get; set; }

        #endregion
    }
}
