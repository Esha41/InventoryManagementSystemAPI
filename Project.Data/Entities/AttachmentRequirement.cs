using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Declares a labeled attachment slot owned by a polymorphic parent (<see cref="ParentType"/> + <see cref="ParentId"/>).
    /// Files uploaded against this requirement are linked via <see cref="FileUplodDetails.AttachmentRequirementId"/>.
    /// </summary>
    /// <remarks>
    /// Team-owned EF migration must align the database with <see cref="ParentType"/> + <see cref="ParentId"/>
    /// (replacing legacy <c>RequestPurposeId</c> when present).
    /// </remarks>
    public class AttachmentRequirement : FullAuditEntity<long>
    {
        public AttachmentRequirementParentType ParentType { get; set; }

        /// <summary>
        /// Primary key of the parent entity identified by <see cref="ParentType"/>.
        /// For <see cref="AttachmentRequirementParentType.System"/> rows this is <c>0</c>; the
        /// row is identified by <see cref="Code"/>.
        /// </summary>
        public long ParentId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        /// <summary>
        /// Stable logical key for product-defined system slots (for example
        /// <c>WEAPON_ASSOCIATION</c>). Null for admin-configured purpose slots.
        /// Unique when present.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// When set, restricts this slot to files whose <see cref="FileUplodDetails.Entity"/> matches.
        /// Null for legacy purpose slots (no entity restriction).
        /// </summary>
        public FileEntityType? ApplicableEntityType { get; set; }

        public bool IsRequired { get; set; } = true;

        public int MinCount { get; set; } = 1;
        public int MaxCount { get; set; } = 1;

        public int DisplayOrder { get; set; } = 0;
    }
}
