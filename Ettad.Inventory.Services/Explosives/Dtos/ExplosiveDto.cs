using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions.Dtos; // For HazardDivisionDto, CompatibilityDto if they are defining there or common
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Explosives.Dtos
{
    public class ExplosiveDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ItemNo { get; set; }

        public string? PartNo { get; set; }

        public decimal? Price { get; set; }

        public long? MinimumQuantity { get; set; }
        
        public string? Nsn { get; set; }

        public ExplosiveType ExplosiveType { get; set; }
        
        public string? UNNumber { get; set; }
        
        public decimal? NetExplosiveQuantity { get; set; }
        public long? NetExplosiveQuantityUnitId { get; set; }
        
        public decimal? TotalWeight { get; set; }
        public long? TotalWeightUnitId { get; set; }
        
        public long? HazardDivisionId { get; set; }
        
        public long? CompatibilityId { get; set; }

        #region Navigation Properties
        public UnitDto NetExplosiveQuantityUnit { get; set; }
        public UnitDto TotalWeightUnit { get; set; }
        public HazardDivisionDto HazardDivision { get; set; }
        public CompatibilityDto Compatibility { get; set; }
        #endregion
    }
}
