using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Ammunition : BaseItem
    {
        public decimal BulletDiameters { get; set; }

        public decimal CaseLength {  get; set; }

        public bool IsLinked { get; set; }
       
        public string Primer { get; set; }
      
        public decimal TotalWeight { get; set; }

        public int? NatureOptionId { get; set; }

        public int NsnId { get; set; }

        public int? PrimaryPurposId { get; set; }

        public int? ProjectileColorId { get; set; }

        public int? ProjectailMaterialId { get; set; }

        public int CaseTypeId { get; set; }

        public int PropellantId { get; set; }

        public int CompatibilityId { get; set; }

        public int HazardDivisionId { get; set; }

        #region Navigation Properties

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
