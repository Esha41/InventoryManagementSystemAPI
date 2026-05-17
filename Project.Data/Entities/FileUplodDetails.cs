using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.FileUpload;

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
        public FileEntityType Entity { get; set; }

        /// <summary>
        /// Primary key of the owning entity (for example Item.Id when Entity = Item).
        /// </summary>
        public long EntityId { get; set; }

        /// <summary>
        /// Optional link to the <see cref="AttachmentRequirement"/> slot this file fulfills.
        /// Null for entity-only / legacy "other" files that are not bound to a requirement.
        /// </summary>
        public long? AttachmentRequirementId { get; set; }

        #region Navigation Properties

        public FileUplodMaster FileUplodMaster { get; set; }

        public AttachmentRequirement AttachmentRequirement { get; set; }

        #endregion
    }
}


