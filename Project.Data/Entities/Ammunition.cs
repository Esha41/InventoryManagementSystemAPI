using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Ammunition : BaseItem
    {
        public decimal BulletDiameter { get; set; }

        public long BulletDiameterUnitId { get; set; }

        public decimal CaseLength {  get; set; }

        public long CaseLengthUnitId { get; set; }

        public bool IsLinked { get; set; }
       
        public string Primer { get; set; }
      
        public decimal TotalWeight { get; set; }

        public long? NatureOptionId { get; set; }

        public string Nsn { get; set; }

        public long? PrimaryPurposId { get; set; }

        public long? ProjectileColorId { get; set; }

        public long? ProjectailMaterialId { get; set; }

        public long CaseTypeId { get; set; }

        public long PropellantId { get; set; }

        public long CompatibilityId { get; set; }

        public long HazardDivisionId { get; set; }

        #region Navigation Properties

        public Unit BulletDiameterUnit { get; set; }
        public Unit CaseLengthUnit { get; set; }
        public NatureOption NatureOption { get; set; }
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
