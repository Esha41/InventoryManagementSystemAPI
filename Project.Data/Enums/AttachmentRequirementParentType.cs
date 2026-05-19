namespace Ettad.Data.Enums
{
    /// <summary>
    /// Identifies which aggregate owns an <see cref="Entities.AttachmentRequirement"/> row via <see cref="Entities.AttachmentRequirement.ParentId"/>.
    /// </summary>
    public enum AttachmentRequirementParentType
    {
        RequestPurpose = 1

        // Future: DiscardPurpose = 2, ReturnPurpose = 3, ...
    }
}
