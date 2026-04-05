using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Ammunitions.Dtos
{
    public class AmmunitionDto : BaseItemDto
    {
        public AmmunitionType AmmunitionType { get; set; }

        public decimal? BulletDiameter { get; set; }

        public long? BulletDiameterUnitId { get; set; }

        public string? ArmNumber { get; set; }

        public bool IsLinked { get; set; }

        public string? Primer { get; set; }

        public decimal? TotalWeight { get; set; }

        public long? NatureOptionId { get; set; }

        public long? ProjectileColorId { get; set; }

        public long? ProjectailMaterialId { get; set; }

        public long? CaseTypeId { get; set; }

        public long? PropellantId { get; set; }

        public long? CompatibilityId { get; set; }

        public long? HazardDivisionId { get; set; }
        
        #region Navigation Properties

        public UnitDto BulletDiameterUnit { get; set; }
        public NatureOptionDto NatureOption { get; set; }
        public ColorDto ProjectileColor { get; set; }
        public ProjectailMaterialDto ProjectailMaterial { get; set; }
        public CaseTypeDto CaseType { get; set; }
        public PropellantDto Propellant { get; set; }
        public CompatibilityDto Compatibility { get; set; }
        public HazardDivisionDto HazardDivision { get; set; }

        #endregion
    }
}
