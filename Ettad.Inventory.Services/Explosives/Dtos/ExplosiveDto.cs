using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions.Dtos; // For HazardDivisionDto, CompatibilityDto if they are defining there or common
using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Explosives.Dtos
{
    public class ExplosiveDto : BaseItemDto
    {
        public long? UnitId { get; set; }
        
        public long? HazardDivisionId { get; set; }

        #region Navigation Properties
        public UnitDto Unit { get; set; }
        public HazardDivisionDto HazardDivision { get; set; }
        #endregion
    }
}
