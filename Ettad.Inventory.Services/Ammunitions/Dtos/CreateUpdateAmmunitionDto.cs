using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Ammunitions.Dtos
{
    public class CreateUpdateAmmunitionDto : CreateUpdateBaseItemDto
    {
        public string? ArmNumber { get; set; }

        public decimal? BulletDiameter { get; set; }

        public long? BulletDiameterUnitId { get; set; }

        public bool? IsLinked { get; set; }

        public string? Primer { get; set; }

        public decimal? TotalWeight { get; set; }

        public long? NatureOptionId { get; set; }

        public long? ProjectileColorId { get; set; }

        public long? ProjectailMaterialId { get; set; }

        public long? CaseTypeId { get; set; }

        public long? PropellantId { get; set; }

        public long? CompatibilityId { get; set; }

        public long? HazardDivisionId { get; set; }
    }
}
