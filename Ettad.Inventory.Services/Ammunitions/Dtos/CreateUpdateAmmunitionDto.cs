namespace Ettad.Inventory.Service.Ammunitions.Dtos
{
    public class CreateUpdateAmmunitionDto
    {
        public string Name { get; set; }

        public string ItemNo { get; set; }

        // All other fields are optional - only Name and ItemNo are required

        public string? PartNo { get; set; }

        public string? ArmNumber { get; set; }

        public decimal? Price { get; set; }

        public long? MinimumQuantity { get; set; }

        public decimal? BulletDiameter { get; set; }

        public long? BulletDiameterUnitId { get; set; }

        public bool? IsLinked { get; set; }

        public string? Primer { get; set; }

        public decimal? TotalWeight { get; set; }

        public long? NatureOptionId { get; set; }

        public string? Nsn { get; set; }

        public long? PrimaryPurposId { get; set; }

        public long? ProjectileColorId { get; set; }

        public long? ProjectailMaterialId { get; set; }

        public long? CaseTypeId { get; set; }

        public long? PropellantId { get; set; }

        public long? CompatibilityId { get; set; }

        public long? HazardDivisionId { get; set; }
    }
}
