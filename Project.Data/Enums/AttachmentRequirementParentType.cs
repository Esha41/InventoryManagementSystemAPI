namespace Ettad.Data.Enums
{
    /// <summary>
    /// Identifies which aggregate owns an <see cref="Entities.AttachmentRequirement"/> row via <see cref="Entities.AttachmentRequirement.ParentId"/>.
    /// </summary>
    public enum AttachmentRequirementParentType
    {
        RequestPurpose = 1,

        /// <summary>
        /// Product-defined slots not configured per business entity (no parent row).
        /// Identified by <see cref="Entities.AttachmentRequirement.Code"/>; <c>ParentId</c> is 0.
        /// </summary>
        System = 2

        // Future: DiscardPurpose = 3, ReturnPurpose = 4, ...
    }
}
