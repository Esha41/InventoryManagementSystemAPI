using AutoMapper;
using FluentValidation;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.RequestManagement.Service.RequestPurposes
{
    public class RequestPurposeService : IRequestPurposeService
    {
        private readonly ICrossCuttingRepository<RequestPurpose> _requestPurposeRepository;
        private readonly ICrossCuttingRepository<AttachmentRequirement> _attachmentRequirementRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateRequestPurposeDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITransactionManager _transactionManager;

        private static AttachmentRequirementParentType PurposeParent =>
            AttachmentRequirementParentType.RequestPurpose;

        private const string DuplicateNameEnKey = "lookupManagement.errors.requestPurposeDuplicateNameEn";
        private const string DuplicateNameArKey = "lookupManagement.errors.requestPurposeDuplicateNameAr";
        private const string DuplicateAttachmentNameEnKey = "lookupManagement.errors.requestPurposeAttachmentDuplicateNameEn";

        public RequestPurposeService(
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            ICrossCuttingRepository<AttachmentRequirement> attachmentRequirementRepository,
            IMapper mapper,
            IValidator<CreateUpdateRequestPurposeDto> validator,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            ITransactionManager transactionManager)
        {
            _requestPurposeRepository = requestPurposeRepository;
            _attachmentRequirementRepository = attachmentRequirementRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
            _transactionManager = transactionManager;
        }

        public async Task<APIOperationResponse<long>> CreateForDiscardAsync(CreateUpdateRequestPurposeDto inputDto)
        {
            return await CreateAsync(inputDto, RequestType.Discard);
        }

        public async Task<APIOperationResponse<long>> CreateForReturnAsync(CreateUpdateRequestPurposeDto inputDto)
        {
            return await CreateAsync(inputDto, RequestType.Return);
        }

        public async Task<APIOperationResponse<long>> CreateForOrderAsync(CreateUpdateRequestPurposeDto inputDto)
        {
            return await CreateAsync(inputDto, RequestType.Order);
        }

        private async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateRequestPurposeDto inputDto, RequestType requestType)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                if (requestType == RequestType.Order && !inputDto.AllowanceContext.HasValue)
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                        "Allowance context is required for order request purposes.");
                }

                var now = _dateTimeProvider.Now;
                var userId = _currentUserService.UserId;
                var trimmedEn = NormalizeName(inputDto.NameEn);
                var trimmedAr = NormalizeName(inputDto.NameAr);

                await _transactionManager.BeginAsync();

                try
                {
                    var softDeleted = await _requestPurposeRepository.FindOneAsync(
                        rp => rp.IsDeleted
                              && rp.RequestType == requestType
                              && rp.NameEn == trimmedEn
                              && rp.NameAr == trimmedAr,
                        includeSoftDeleted: true);

                    if (softDeleted != null)
                    {
                        _mapper.Map(inputDto, softDeleted);
                        softDeleted.NameEn = trimmedEn;
                        softDeleted.NameAr = trimmedAr;
                        softDeleted.RequestType = requestType;
                        if (requestType != RequestType.Order)
                            softDeleted.AllowanceContext = inputDto.AllowanceContext ?? RequestPurposeAllowanceContext.Both;
                        RestoreSoftDeleted(softDeleted, now, userId);

                        await _requestPurposeRepository.UpdateAsync(softDeleted);

                        var restoredAttachmentRows = await LoadAllAttachmentRequirementsForPurposeAsync(softDeleted.Id);
                        var attachmentError = await UpsertAttachmentRequirementsAsync(
                            softDeleted.Id, inputDto.AttachmentRequirements, restoredAttachmentRows, now, userId);
                        if (attachmentError != null)
                        {
                            await _transactionManager.RollbackAsync();
                            return APIOperationResponse<long>.BadRequest(attachmentError);
                        }

                        await _transactionManager.CommitAsync();
                        return APIOperationResponse<long>.Success(softDeleted.Id, "Request purpose restored successfully");
                    }

                    var duplicateError = await ValidateActiveUniqueNamesAsync(trimmedEn, trimmedAr, excludeId: null);
                    if (duplicateError != null)
                    {
                        await _transactionManager.RollbackAsync();
                        return APIOperationResponse<long>.BadRequest(duplicateError);
                    }

                    var requestPurpose = _mapper.Map<RequestPurpose>(inputDto);
                    requestPurpose.NameEn = trimmedEn;
                    requestPurpose.NameAr = trimmedAr;
                    requestPurpose.RequestType = requestType;
                    if (requestType != RequestType.Order)
                        requestPurpose.AllowanceContext = inputDto.AllowanceContext ?? RequestPurposeAllowanceContext.Both;
                    requestPurpose.CreationDate = now;
                    requestPurpose.CreatedBy = userId;

                    var created = await _requestPurposeRepository.AddAsync(requestPurpose);

                    if (inputDto.AttachmentRequirements != null && inputDto.AttachmentRequirements.Count > 0)
                    {
                        var attachmentRows = await LoadAllAttachmentRequirementsForPurposeAsync(created.Id);
                        var attachmentError = await UpsertAttachmentRequirementsAsync(
                            created.Id, inputDto.AttachmentRequirements, attachmentRows, now, userId);
                        if (attachmentError != null)
                        {
                            await _transactionManager.RollbackAsync();
                            return APIOperationResponse<long>.BadRequest(attachmentError);
                        }
                    }

                    await _transactionManager.CommitAsync();
                    return APIOperationResponse<long>.Success(created.Id, "Request purpose created successfully");
                }
                catch
                {
                    await _transactionManager.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<RequestPurposeDto>> GetByIdForDiscardAsync(long id)
        {
            return await GetByIdAsync(id, RequestType.Discard);
        }

        public async Task<APIOperationResponse<RequestPurposeDto>> GetByIdForReturnAsync(long id)
        {
            return await GetByIdAsync(id, RequestType.Return);
        }

        public async Task<APIOperationResponse<RequestPurposeDto>> GetByIdForOrderAsync(long id)
        {
            return await GetByIdAsync(id, RequestType.Order);
        }

        private async Task<APIOperationResponse<RequestPurposeDto>> GetByIdAsync(long id, RequestType requestType)
        {
            try
            {
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == id && rp.RequestType == requestType && !rp.IsDeleted);

                if (requestPurpose == null)
                    return APIOperationResponse<RequestPurposeDto>.Fail(ResponseType.NotFound, "Request purpose not found");

                var reqs = await LoadAttachmentRequirementsForPurposeAsync(requestPurpose.Id);
                var dto = _mapper.Map<RequestPurposeDto>(requestPurpose);
                dto.AttachmentRequirements = _mapper.Map<List<AttachmentRequirementDto>>(reqs);
                return APIOperationResponse<RequestPurposeDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<RequestPurposeDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForDiscardAsync()
        {
            return await GetAllAsync(RequestType.Discard);
        }

        public async Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForReturnAsync()
        {
            return await GetAllAsync(RequestType.Return);
        }

        public async Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForOrderAsync(bool? isFromAllowance = null)
        {
            return await GetAllAsync(RequestType.Order, isFromAllowance);
        }

        private async Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllAsync(
            RequestType requestType,
            bool? isFromAllowance = null)
        {
            try
            {
                var requestPurposes = (await _requestPurposeRepository.FindAsync(
                    rp => rp.RequestType == requestType && !rp.IsDeleted)).ToList();

                if (requestType == RequestType.Order && isFromAllowance.HasValue)
                {
                    requestPurposes = requestPurposes
                        .Where(rp => IsPurposeAllowedForAllowance(rp.AllowanceContext, isFromAllowance.Value))
                        .ToList();
                }

                var purposeIds = requestPurposes.Select(rp => rp.Id).ToList();
                var byPurposeId = await LoadAttachmentRequirementsGroupedByPurposeIdAsync(purposeIds);
                var dtos = new List<RequestPurposeDto>();
                foreach (var rp in requestPurposes)
                {
                    var dto = _mapper.Map<RequestPurposeDto>(rp);
                    dto.AttachmentRequirements = byPurposeId.TryGetValue(rp.Id, out var list)
                        ? _mapper.Map<List<AttachmentRequirementDto>>(list)
                        : new List<AttachmentRequirementDto>();
                    dtos.Add(dto);
                }

                return APIOperationResponse<List<RequestPurposeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<RequestPurposeDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateRequestPurposeDto inputDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var existing = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == id && !rp.IsDeleted);

                if (existing == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Request purpose not found");

                if (existing.RequestType == RequestType.Order && !inputDto.AllowanceContext.HasValue)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Allowance context is required for order request purposes.");
                }

                var trimmedEn = NormalizeName(inputDto.NameEn);
                var trimmedAr = NormalizeName(inputDto.NameAr);

                var duplicateError = await ValidateActiveUniqueNamesAsync(trimmedEn, trimmedAr, excludeId: id);
                if (duplicateError != null)
                    return APIOperationResponse<bool>.BadRequest(duplicateError);

                var now = _dateTimeProvider.Now;
                var userId = _currentUserService.UserId;

                await _transactionManager.BeginAsync();

                try
                {
                    _mapper.Map(inputDto, existing);
                    existing.NameEn = trimmedEn;
                    existing.NameAr = trimmedAr;
                    if (existing.RequestType != RequestType.Order && !inputDto.AllowanceContext.HasValue)
                        existing.AllowanceContext = RequestPurposeAllowanceContext.Both;
                    existing.ModificationDate = now;
                    existing.ModifiedBy = userId;

                    await _requestPurposeRepository.UpdateAsync(existing);

                    var attachmentRows = await LoadAllAttachmentRequirementsForPurposeAsync(existing.Id);
                    var attachmentError = await UpsertAttachmentRequirementsAsync(
                        existing.Id, inputDto.AttachmentRequirements, attachmentRows, now, userId);
                    if (attachmentError != null)
                    {
                        await _transactionManager.RollbackAsync();
                        return APIOperationResponse<bool>.BadRequest(attachmentError);
                    }

                    await _transactionManager.CommitAsync();
                    return APIOperationResponse<bool>.Success(true, "Request purpose updated successfully");
                }
                catch
                {
                    await _transactionManager.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Upserts attachment slots for a RequestPurpose (<see cref="PurposeParent"/> + <paramref name="requestPurposeId"/>).
        /// Returns an i18n error key when a duplicate English name is detected among active slots.
        /// </summary>
        private async Task<string?> UpsertAttachmentRequirementsAsync(
            long requestPurposeId,
            List<CreateUpdateAttachmentRequirementDto> incoming,
            ICollection<AttachmentRequirement> existing,
            DateTime now,
            string? userId)
        {
            incoming ??= new List<CreateUpdateAttachmentRequirementDto>();
            existing ??= new List<AttachmentRequirement>();

            var scopedExisting = existing
                .Where(ar => ar.ParentType == PurposeParent && ar.ParentId == requestPurposeId)
                .ToList();

            var existingActive = scopedExisting.Where(ar => !ar.IsDeleted).ToList();

            var incomingIds = incoming
                .Where(x => x.Id.HasValue && x.Id.Value > 0)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            var seenIncomingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var dto in incoming)
            {
                var nameEn = NormalizeName(dto.NameEn);
                if (string.IsNullOrEmpty(nameEn))
                    continue;

                if (!seenIncomingNames.Add(nameEn))
                    return DuplicateAttachmentNameEnKey;
            }

            foreach (var stale in existingActive.Where(ar => !incomingIds.Contains(ar.Id)))
            {
                stale.IsDeleted = true;
                stale.DeletionDate = now;
                stale.DeletedBy = userId;
                await _attachmentRequirementRepository.UpdateAsync(stale);
            }

            foreach (var dto in incoming)
            {
                var nameEn = NormalizeName(dto.NameEn);
                var nameAr = NormalizeName(dto.NameAr);

                if (dto.Id.HasValue && dto.Id.Value > 0)
                {
                    var row = scopedExisting.FirstOrDefault(ar => ar.Id == dto.Id.Value);
                    if (row == null)
                        continue;

                    if (row.IsDeleted)
                        RestoreSoftDeleted(row, now, userId);

                    row.NameAr = nameAr;
                    row.NameEn = nameEn;
                    row.IsRequired = dto.IsRequired;
                    row.MinCount = dto.MinCount;
                    row.MaxCount = dto.MaxCount;
                    row.DisplayOrder = dto.DisplayOrder;
                    row.ModificationDate = now;
                    row.ModifiedBy = userId;
                    await _attachmentRequirementRepository.UpdateAsync(row);
                }
                else
                {
                    var deletedMatch = scopedExisting.FirstOrDefault(ar =>
                        ar.IsDeleted
                        && string.Equals(ar.NameEn, nameEn, StringComparison.OrdinalIgnoreCase));

                    if (deletedMatch != null)
                    {
                        deletedMatch.NameAr = nameAr;
                        deletedMatch.NameEn = nameEn;
                        deletedMatch.IsRequired = dto.IsRequired;
                        deletedMatch.MinCount = dto.MinCount;
                        deletedMatch.MaxCount = dto.MaxCount;
                        deletedMatch.DisplayOrder = dto.DisplayOrder;
                        RestoreSoftDeleted(deletedMatch, now, userId);
                        await _attachmentRequirementRepository.UpdateAsync(deletedMatch);
                        continue;
                    }

                    if (existingActive.Any(ar =>
                            incomingIds.Contains(ar.Id)
                            && string.Equals(ar.NameEn, nameEn, StringComparison.OrdinalIgnoreCase)))
                    {
                        return DuplicateAttachmentNameEnKey;
                    }

                    var entity = _mapper.Map<AttachmentRequirement>(dto);
                    entity.NameEn = nameEn;
                    entity.NameAr = nameAr;
                    entity.ParentType = PurposeParent;
                    entity.ParentId = requestPurposeId;
                    entity.CreationDate = now;
                    entity.CreatedBy = userId;
                    await _attachmentRequirementRepository.AddAsync(entity);
                }
            }

            return null;
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(rp => rp.Id == id && !rp.IsDeleted);
                if (requestPurpose == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Request purpose not found");

                var now = _dateTimeProvider.Now;
                var userId = _currentUserService.UserId;

                var attachments = await LoadAttachmentRequirementsForPurposeAsync(requestPurpose.Id);
                foreach (var ar in attachments.Where(a => !a.IsDeleted))
                {
                    ar.IsDeleted = true;
                    ar.DeletionDate = now;
                    ar.DeletedBy = userId;
                    await _attachmentRequirementRepository.UpdateAsync(ar);
                }

                await _requestPurposeRepository.DeleteAsync(requestPurpose);
                return APIOperationResponse<bool>.Success(true, "Request purpose deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<List<AttachmentRequirement>> LoadAttachmentRequirementsForPurposeAsync(long purposeId)
        {
            var rows = await _attachmentRequirementRepository.FindAsync(ar =>
                !ar.IsDeleted
                && ar.ParentType == PurposeParent
                && ar.ParentId == purposeId);

            return rows.OrderBy(ar => ar.DisplayOrder).ToList();
        }

        private async Task<List<AttachmentRequirement>> LoadAllAttachmentRequirementsForPurposeAsync(long purposeId)
        {
            var rows = await _attachmentRequirementRepository.FindAsync(
                ar => ar.ParentType == PurposeParent && ar.ParentId == purposeId,
                includeSoftDeleted: true);

            return rows.OrderBy(ar => ar.DisplayOrder).ToList();
        }

        private async Task<Dictionary<long, List<AttachmentRequirement>>> LoadAttachmentRequirementsGroupedByPurposeIdAsync(
            List<long> purposeIds)
        {
            var result = new Dictionary<long, List<AttachmentRequirement>>();
            if (purposeIds.Count == 0)
                return result;

            var idSet = purposeIds.ToHashSet();
            var rows = await _attachmentRequirementRepository.FindAsync(ar =>
                !ar.IsDeleted
                && ar.ParentType == PurposeParent
                && idSet.Contains(ar.ParentId));

            foreach (var g in rows.GroupBy(ar => ar.ParentId))
                result[g.Key] = g.OrderBy(ar => ar.DisplayOrder).ToList();

            return result;
        }

        private static bool IsPurposeAllowedForAllowance(
            RequestPurposeAllowanceContext context,
            bool isFromAllowance)
        {
            return context switch
            {
                RequestPurposeAllowanceContext.Both => true,
                RequestPurposeAllowanceContext.FromAllowance => isFromAllowance,
                RequestPurposeAllowanceContext.OutsideAllowance => !isFromAllowance,
                _ => false
            };
        }

        /// <summary>
        /// Validates uniqueness against active (non-deleted) request purposes only.
        /// </summary>
        private async Task<string?> ValidateActiveUniqueNamesAsync(string? nameEn, string? nameAr, long? excludeId)
        {
            var trimmedEn = NormalizeName(nameEn);
            var trimmedAr = NormalizeName(nameAr);

            if (!string.IsNullOrEmpty(trimmedEn))
            {
                var existingEn = await _requestPurposeRepository.FindOneAsync(
                    rp => !rp.IsDeleted
                          && rp.NameEn == trimmedEn
                          && (!excludeId.HasValue || rp.Id != excludeId.Value));

                if (existingEn != null)
                    return DuplicateNameEnKey;
            }

            if (!string.IsNullOrEmpty(trimmedAr))
            {
                var existingAr = await _requestPurposeRepository.FindOneAsync(
                    rp => !rp.IsDeleted
                          && rp.NameAr == trimmedAr
                          && (!excludeId.HasValue || rp.Id != excludeId.Value));

                if (existingAr != null)
                    return DuplicateNameArKey;
            }

            return null;
        }

        private static string NormalizeName(string? name) => name?.Trim() ?? string.Empty;

        private static void RestoreSoftDeleted(RequestPurpose entity, DateTime now, string? userId)
        {
            entity.IsDeleted = false;
            entity.DeletionDate = null;
            entity.DeletedBy = null;
            entity.ModificationDate = now;
            entity.ModifiedBy = userId;
        }

        private static void RestoreSoftDeleted(AttachmentRequirement entity, DateTime now, string? userId)
        {
            entity.IsDeleted = false;
            entity.DeletionDate = null;
            entity.DeletedBy = null;
            entity.ModificationDate = now;
            entity.ModifiedBy = userId;
        }
    }
}
