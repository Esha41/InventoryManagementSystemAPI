using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Explosives.Dtos
{
    public class ExplosiveImportDto
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

        // Explosive Properties
        public string? NEQUnit { get; set; } // Will be parsed as Enum if mapped correctly in service, or handled manually
        
        // Explosive Lookup Names
        public string? HazardDivision { get; set; }
    }
}
