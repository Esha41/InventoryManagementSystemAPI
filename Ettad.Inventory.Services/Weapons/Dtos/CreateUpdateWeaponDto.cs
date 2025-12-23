using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Weapons.Dtos
{
    public class CreateUpdateWeaponDto
    {
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

        public long? ClassificationId { get; set; }

        public long? TypeId { get; set; }

        public WeaponType WeaponType { get; set; }
        
        public string? Caliber { get; set; }
        
        public ActionType ActionType { get; set; }
        
        public decimal? BarrelLength { get; set; }
        public long? BarrelLengthUnitId { get; set; }
        
        public decimal? OverallLength { get; set; }
        public long? OverallLengthUnitId { get; set; }
        
        public decimal? Weight { get; set; }
        public long? WeightUnitId { get; set; }
        
        public int? Capacity { get; set; }
    }
}
