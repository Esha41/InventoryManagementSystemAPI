using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Ammunitions.Dtos
{
    public class AmmunitionImportDto
    {
        public int RowNumber { get; set; }
        // Base Item Properties
        public string Name { get; set; }
        public string ItemNo { get; set; }
        public string? PartNo { get; set; }
        public decimal? Price { get; set; }
        public long? MinimumQuantity { get; set; }
        public string? Nsn { get; set; }
        public string? Distribution { get; set; }
        public string? ReferenceNo { get; set; }
        public string? UNNumber { get; set; }
        public string? Notes { get; set; }
        
        // Base Item Lookup Names
        public string? Classification { get; set; }
        public string? Type { get; set; }

        // Ammunition Properties
        public string? ArmNumber { get; set; }
        public decimal? BulletDiameter { get; set; }
        public bool? IsLinked { get; set; }
        public string? Primer { get; set; }
        public decimal? TotalWeight { get; set; }

        // Ammunition Lookup Names
        public string? BulletDiameterUnit { get; set; }
        public string? NatureOption { get; set; }
        public string? PrimaryPurpose { get; set; }
        public string? ProjectileColor { get; set; }
        public string? ProjectileMaterial { get; set; }
        public string? CaseType { get; set; }
        public string? Propellant { get; set; }
        public string? Compatibility { get; set; }
        public string? HazardDivision { get; set; }
    }
}
