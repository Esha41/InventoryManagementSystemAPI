using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.RequestManagement.Service.Common.Interfaces
{
    /// <summary>
    /// Validates uploads submitted for the AttachmentRequirements declared by a RequestPurpose,
    /// plus an optional bucket of "other" entity-only files.
    /// </summary>
    public interface IAttachmentRequirementUploadValidationService
    {
        /// <summary>
        /// Returns Success when:
        /// - Every required AttachmentRequirement for the purpose has at least MinCount non-empty files.
        /// - No requirement exceeds MaxCount files.
        /// - All keys in <paramref name="filesByAttachmentRequirementId"/> belong to attachment rows scoped to this request purpose
        ///   (<c>AttachmentRequirementParentType.RequestPurpose</c> + <paramref name="requestPurposeId"/>).
        /// otherFiles is optional and may be null/empty.
        /// </summary>
        Task<APIOperationResponse<bool>> ValidateAsync(
            long requestPurposeId,
            IReadOnlyDictionary<long, IReadOnlyList<IFormFile>> filesByAttachmentRequirementId,
            IReadOnlyList<IFormFile>? otherFiles,
            CancellationToken cancellationToken = default);
    }
}
