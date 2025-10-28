namespace Ettad.Data.Entities
{
    public class Ammunition : BaseItem
    {
        public decimal BulletDiameters { get; set; }

        public decimal CaseLength {  get; set; }

        public bool IsLinked { get; set; }

        public int? NatureOptionId { get; set; }

        public string Ncn { get; set; }

        public int? PrimaryPurposId { get; set; }

        public int? ProjectileColorId { get; set; }

        public decimal TotalWeight { get; set; }

        public int? ProjectileMaterialId { get; set; }

        public int CaseTypeId { get; set; }

        public string Primer { get; set; }

        public int PropellantId { get; set; }

        public string CompabilityGroup { get;set; }

        public string Compatibility { get; set; }

        #region Navigation Properties

        public NatureOption NatureOption { get; set; }
        public Color ProjectileColor { get; set; }
        public PrimaryPurpos PrimaryPurpos { get; set; }
        public ProjectailMaterial ProjectailMaterial { get; set; }
        public CaseType CaseType { get; set; }
        public Propellant Propellant { get; set; }
        #endregion
    }
}
