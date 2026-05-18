using Microsoft.AspNetCore.Http;

namespace Ettad.RequestManagement.Service.Common.Dtos
{
    /// <summary>
    /// Multipart binding model for a single per-AttachmentRequirement upload group.
    /// Clients send indexed names like:
    ///   AttachmentUploads[0].AttachmentRequirementId
    ///   AttachmentUploads[0].Files (repeated)
    /// </summary>
    public class AttachmentUploadGroupDto
    {
        public long AttachmentRequirementId { get; set; }

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
