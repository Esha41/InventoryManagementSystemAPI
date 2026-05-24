using System.Collections.Concurrent;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.RequestManagement.Service.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ettad.RequestManagement.Service.Common.Services
{
    /// <inheritdoc cref="ISystemAttachmentSlotResolver"/>
    /// <remarks>
    /// Slot rows are static configuration (seeded once per environment), so a successful
    /// lookup is cached by Code for the lifetime of the application. Misconfiguration
    /// (missing row, wrong <c>ApplicableEntityType</c>) throws rather than silently
    /// linking files to the wrong owner.
    /// </remarks>
    public class SystemAttachmentSlotResolver : ISystemAttachmentSlotResolver
    {
        private readonly ICrossCuttingRepository<AttachmentRequirement> _attachmentRequirementRepository;
        private readonly ILogger<SystemAttachmentSlotResolver> _logger;

        private static readonly ConcurrentDictionary<string, AttachmentRequirement> _cache = new();

        public SystemAttachmentSlotResolver(
            ICrossCuttingRepository<AttachmentRequirement> attachmentRequirementRepository,
            ILogger<SystemAttachmentSlotResolver> logger)
        {
            _attachmentRequirementRepository = attachmentRequirementRepository;
            _logger = logger;
        }

        public async Task<AttachmentRequirement> ResolveAsync(
            SystemAttachmentSlotCode code,
            FileEntityType expectedEntity,
            CancellationToken cancellationToken = default)
        {
            var dbCode = code.ToDbCode();

            if (!_cache.TryGetValue(dbCode, out var slot))
            {
                slot = await _attachmentRequirementRepository.FindOneAsync(
                    ar => !ar.IsDeleted
                          && ar.ParentType == AttachmentRequirementParentType.System
                          && ar.Code == dbCode);

                if (slot == null)
                {
                    var message = $"System attachment slot '{dbCode}' is not seeded. " +
                                  "Run database seeding or add the row before using this feature.";
                    _logger.LogError(message);
                    throw new InvalidOperationException(message);
                }

                _cache.TryAdd(dbCode, slot);
            }

            if (slot.ApplicableEntityType.HasValue && slot.ApplicableEntityType.Value != expectedEntity)
            {
                var message = $"System attachment slot '{dbCode}' is reserved for entity " +
                              $"{slot.ApplicableEntityType}, not {expectedEntity}.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            return slot;
        }
    }
}
