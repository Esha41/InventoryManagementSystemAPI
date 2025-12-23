using Ettad.Data.Entities;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class Explosive : BaseItem
    {
        public long? HazardDivisionId { get; set; }
        public ExplosiveUnit Unit { get; set; }

        #region Navigation Properties
        public HazardDivision HazardDivision { get; set; }
        #endregion
    }
}
