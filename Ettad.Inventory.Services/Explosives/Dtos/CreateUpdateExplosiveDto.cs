using Ettad.Data.Enums;
using Ettad.Inventory.Service.Common.Dtos;

namespace Ettad.Inventory.Service.Explosives.Dtos
{
    public class CreateUpdateExplosiveDto : CreateUpdateBaseItemDto
    {
        public string? ArmNumber { get; set; }
        public long? CompatibilityId { get; set; }
        public long? UnitId { get; set; }
        public long? HazardDivisionId { get; set; }
    }
}
