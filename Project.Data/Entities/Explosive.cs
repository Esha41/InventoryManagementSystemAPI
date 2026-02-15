using Ettad.Data.Entities;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Explosive : BaseItem
    {
        public string? ArmNumber { get; set; }
        public long? CompatibilityId { get; set; }
        public long? HazardDivisionId { get; set; }
        public long? UnitId { get; set; }

        #region Navigation Properties
        public Compatibility Compatibility { get; set; }
        public HazardDivision HazardDivision { get; set; }
        public Unit Unit { get; set; }
        #endregion
    }
}
