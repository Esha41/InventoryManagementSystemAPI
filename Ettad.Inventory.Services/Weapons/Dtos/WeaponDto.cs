using Ettad.Data.Enums;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Weapons.Dtos
{
    public class WeaponDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ItemNo { get; set; }

        public string? PartNo { get; set; }

        public decimal? Price { get; set; }

        public long? MinimumQuantity { get; set; }
        
        public string? Nsn { get; set; }

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

        #region Navigation Properties
        public UnitDto BarrelLengthUnit { get; set; }
        public UnitDto OverallLengthUnit { get; set; }
        public UnitDto WeightUnit { get; set; }
        #endregion
    }
}
