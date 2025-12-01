using Ettad.Data.Enums;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Ammunitions.Dtos
{
    public class AmmunitionDto
    {
        public long Id { get; set; }

        public AmmunitionType AmmunitionType { get; set; }

        public string Name { get; set; }

        public string ItemNo { get; set; }

        public long HccId { get; set; }

        public string PartNo { get; set; }

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

        public string Nsn { get; set; }

        public long? PrimaryPurposId { get; set; }

        public long? ProjectileColorId { get; set; }

        public long? ProjectailMaterialId { get; set; }

        public long CaseTypeId { get; set; }

        public long PropellantId { get; set; }

        public long CompatibilityId { get; set; }

        public long HazardDivisionId { get; set; }
        
        #region Navigation Properties

        public HccDto Hcc { get; set; }
        public UnitDto BulletDiameterUnit { get; set; }
        public UnitDto CaseLengthUnit { get; set; }
        public NatureOptionDto NatureOption { get; set; }
        public PrimaryPurposDto PrimaryPurpos { get; set; }
        public ColorDto ProjectileColor { get; set; }
        public ProjectailMaterialDto ProjectailMaterial { get; set; }
        public CaseTypeDto CaseType { get; set; }
        public PropellantDto Propellant { get; set; }
        public CompatibilityDto Compatibility { get; set; }
        public HazardDivisionDto HazardDivision { get; set; }

        #endregion
    }
}
