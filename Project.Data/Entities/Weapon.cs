using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Weapon : BaseItem
    {
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
        public Unit BarrelLengthUnit { get; set; }
        public Unit OverallLengthUnit { get; set; }
        public Unit WeightUnit { get; set; }
        #endregion
    }
}
