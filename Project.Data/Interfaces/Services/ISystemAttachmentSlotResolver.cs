using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Entities;

namespace Ettad.Data.Interfaces.Services
{
    /// <summary>
    /// Resolves product-defined attachment slots (rows on <see cref="AttachmentRequirement"/>
    /// with <c>ParentType = System</c>) by their compile-time
    /// <see cref="SystemAttachmentSlotCode"/>.
    /// </summary>
    /// <remarks>
    /// The resolver enforces the slot's <see cref="AttachmentRequirement.ApplicableEntityType"/>
    /// against the caller's expected <see cref="FileEntityType"/>, so a feature that owns one
    /// entity (e.g. Order create) cannot accidentally upload against a slot reserved for a
    /// different entity (e.g. an Employee passport slot).
    /// </remarks>
    public interface ISystemAttachmentSlotResolver
    {
        /// <summary>
        /// Returns the <see cref="AttachmentRequirement"/> row for the given system slot.
        /// Throws when the row is missing or when its <c>ApplicableEntityType</c> does not
        /// match <paramref name="expectedEntity"/>.
        /// </summary>
        Task<AttachmentRequirement> ResolveAsync(
            SystemAttachmentSlotCode code,
            FileEntityType expectedEntity,
            CancellationToken cancellationToken = default);
    }
}
