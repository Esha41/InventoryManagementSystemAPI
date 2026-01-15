using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Weapons.Dtos
{
    public class WeaponImportDto
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

        // Weapon Properties
        public string? Caliber { get; set; }
        public int? YearOfManufacture { get; set; }
        public string? Model { get; set; }

        // Weapon Lookup Names
        public string? CaliberUnit { get; set; }
        public string? CountryOfManufacture { get; set; }
    }
}

