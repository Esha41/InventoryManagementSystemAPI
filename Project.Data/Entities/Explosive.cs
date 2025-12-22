using Ettad.Data.Entities;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Explosive : BaseItem
    {
        public ExplosiveType ExplosiveType { get; set; }
        
        public string? UNNumber { get; set; }
        
        public decimal? NetExplosiveQuantity { get; set; }
        public long? NetExplosiveQuantityUnitId { get; set; }
        
        public decimal? TotalWeight { get; set; }
        public long? TotalWeightUnitId { get; set; }
        
        public long? HazardDivisionId { get; set; }
        
        public long? CompatibilityId { get; set; }

        #region Navigation Properties
        public Unit NetExplosiveQuantityUnit { get; set; }
        public Unit TotalWeightUnit { get; set; }
        public HazardDivision HazardDivision { get; set; }
        public Compatibility Compatibility { get; set; }
        #endregion
    }
}
