using Ettad.Data.Enums;
using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Explosives.Dtos
{
    public class CreateUpdateExplosiveDto : CreateUpdateBaseItemDto
    {
        public long? UnitId { get; set; }
        
        public long? HazardDivisionId { get; set; }
    }
}
