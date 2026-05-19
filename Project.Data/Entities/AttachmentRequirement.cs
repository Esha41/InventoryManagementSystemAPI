using Ettad.CrossCutting.Comman.Base;
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
        /// </summary>
        public long ParentId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public bool IsRequired { get; set; } = true;

        public int MinCount { get; set; } = 1;
        public int MaxCount { get; set; } = 1;

        public int DisplayOrder { get; set; } = 0;
    }
}
