using Ettad.CrossCutting.Comman.Base;
using Ettad.Comman.Enums;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Details for file usage. Links a file to a specific entity (Item, Request, etc.).
    /// </summary>
    public class FileUplodDetails : BaseEntity<long>
    {
        public long FileUplodMasterId { get; set; }

        /// <summary>
        /// Logical entity type that owns the file (e.g. Item, Request, Order).
        /// </summary>
        public FileEntityType EntityId { get; set; }

        /// <summary>
        /// Primary key of the owning entity (for example Item.Id when EntityId = Item).
        /// </summary>
        public long PrimaryId { get; set; }

        #region Navigation Properties

        public FileUplodMaster FileUplodMaster { get; set; }

        #endregion
    }
}


