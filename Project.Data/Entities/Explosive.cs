using Ettad.Data.Entities;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Explosive : BaseItem
    {
        public long? HazardDivisionId { get; set; }
        public long? UnitId { get; set; }

        #region Navigation Properties
        public HazardDivision HazardDivision { get; set; }
        public Unit Unit { get; set; }
        #endregion
    }
}
