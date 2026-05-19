using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.RequestManagement.Service.Common.Interfaces;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ettad.RequestManagement.Service.Common.Services
{
    /// <inheritdoc cref="IAttachmentRequirementUploadValidationService"/>
    public class AttachmentRequirementUploadValidationService : IAttachmentRequirementUploadValidationService
    {
        private readonly ICrossCuttingRepository<AttachmentRequirement> _attachmentRequirementRepository;
        private readonly ILogger<AttachmentRequirementUploadValidationService> _logger;

        public AttachmentRequirementUploadValidationService(
            ICrossCuttingRepository<AttachmentRequirement> attachmentRequirementRepository,
            ILogger<AttachmentRequirementUploadValidationService> logger)
        {
            _attachmentRequirementRepository = attachmentRequirementRepository;
            _logger = logger;
        }

        public async Task<APIOperationResponse<bool>> ValidateAsync(
            long requestPurposeId,
            IReadOnlyDictionary<long, IReadOnlyList<IFormFile>> filesByAttachmentRequirementId,
            IReadOnlyList<IFormFile>? otherFiles,
            CancellationToken cancellationToken = default)
        {
            filesByAttachmentRequirementId ??= new Dictionary<long, IReadOnlyList<IFormFile>>();

            var requirements = (await _attachmentRequirementRepository.FindAsync(
                ar => !ar.IsDeleted
                      && ar.ParentType == AttachmentRequirementParentType.RequestPurpose
                      && ar.ParentId == requestPurposeId))
                .ToList();

            var requirementsById = requirements.ToDictionary(r => r.Id);

            var unknownKeys = filesByAttachmentRequirementId.Keys
                .Where(k => !requirementsById.ContainsKey(k))
                .ToList();

            if (unknownKeys.Count > 0)
            {
                var msg = $"Unknown AttachmentRequirementId(s) for request purpose {requestPurposeId}: {string.Join(", ", unknownKeys)}";
                _logger.LogWarning(msg);
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, msg);
            }

            var errors = new List<string>();

            foreach (var requirement in requirements)
            {
                filesByAttachmentRequirementId.TryGetValue(requirement.Id, out var providedFiles);

                var effectiveFiles = (providedFiles ?? Array.Empty<IFormFile>())
                    .Where(f => f != null && f.Length > 0)
                    .ToList();

                if (requirement.IsRequired && effectiveFiles.Count < requirement.MinCount)
                {
                    errors.Add($"Attachment '{requirement.NameEn}' requires at least {requirement.MinCount} file(s).");
                    continue;
                }

                if (!requirement.IsRequired && effectiveFiles.Count == 0)
                {
                    continue;
                }

                if (effectiveFiles.Count > requirement.MaxCount)
                {
                    errors.Add($"Attachment '{requirement.NameEn}' allows at most {requirement.MaxCount} file(s), {effectiveFiles.Count} were provided.");
                }
            }

            if (errors.Count > 0)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, string.Join(" ", errors));
            }

            return APIOperationResponse<bool>.Success(true);
        }
    }
}
