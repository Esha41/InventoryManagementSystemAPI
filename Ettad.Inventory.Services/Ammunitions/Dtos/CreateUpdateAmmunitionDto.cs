namespace Ettad.Inventory.Service.Ammunitions.Dtos
{
    public class CreateUpdateAmmunitionDto
    {
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

        public string? Nsn { get; set; }

        public long? PrimaryPurposId { get; set; }

        public long? ProjectileColorId { get; set; }

        public long? ProjectailMaterialId { get; set; }

        public long CaseTypeId { get; set; }

        public long PropellantId { get; set; }

        public long CompatibilityId { get; set; }

        public long HazardDivisionId { get; set; }
    }
}
