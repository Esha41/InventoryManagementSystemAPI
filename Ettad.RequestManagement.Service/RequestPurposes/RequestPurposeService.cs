using AutoMapper;
using FluentValidation;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

        private static AttachmentRequirementParentType PurposeParent =>
            AttachmentRequirementParentType.RequestPurpose;

        private const string DuplicateNameEnKey = "lookupManagement.errors.requestPurposeDuplicateNameEn";
        private const string DuplicateNameArKey = "lookupManagement.errors.requestPurposeDuplicateNameAr";
        private const string DuplicateNameGenericKey = "lookupManagement.errors.requestPurposeDuplicateName";
        private const string SaveFailedKey = "lookupManagement.errors.requestPurposeSaveFailed";

        public RequestPurposeService(
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            ICrossCuttingRepository<AttachmentRequirement> attachmentRequirementRepository,
            IMapper mapper,
            IValidator<CreateUpdateRequestPurposeDto> validator,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _requestPurposeRepository = requestPurposeRepository;
            _attachmentRequirementRepository = attachmentRequirementRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
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

                if (requestType == RequestType.Order)
                {
                    if (!inputDto.AllowanceContext.HasValue)
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            "Allowance context is required for order request purposes.");
                }

                var duplicateError = await ValidateUniqueNamesAsync(inputDto.NameEn, inputDto.NameAr, excludeId: null);
                if (duplicateError != null)
                    return APIOperationResponse<long>.BadRequest(duplicateError);

                var now = _dateTimeProvider.Now;
                var userId = _currentUserService.UserId;
                var requestPurpose = _mapper.Map<RequestPurpose>(inputDto);
                requestPurpose.RequestType = requestType;
                if (requestType != RequestType.Order)
                    requestPurpose.AllowanceContext = inputDto.AllowanceContext ?? RequestPurposeAllowanceContext.Both;
                requestPurpose.CreationDate = now;
                requestPurpose.CreatedBy = userId;

                var created = await _requestPurposeRepository.AddAsync(requestPurpose);

                if (inputDto.AttachmentRequirements != null)
                {
                    foreach (var arDto in inputDto.AttachmentRequirements)
                    {
                        var ar = _mapper.Map<AttachmentRequirement>(arDto);
                        ar.ParentType = PurposeParent;
                        ar.ParentId = created.Id;
                        ar.CreationDate = now;
                        ar.CreatedBy = userId;
                        await _attachmentRequirementRepository.AddAsync(ar);
                    }
                }

                return APIOperationResponse<long>.Success(created.Id, "Request purpose created successfully");
            }
            catch (DbUpdateException dbEx) when (TryMapDuplicateDbException(dbEx, out var duplicateMessage))
            {
                return APIOperationResponse<long>.BadRequest(duplicateMessage);
            }
            catch (DbUpdateException)
            {
                return APIOperationResponse<long>.ServerError(SaveFailedKey);
            }
            catch (Exception)
            {
                return APIOperationResponse<long>.ServerError(SaveFailedKey);
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
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Allowance context is required for order request purposes.");

                var duplicateError = await ValidateUniqueNamesAsync(inputDto.NameEn, inputDto.NameAr, excludeId: id);
                if (duplicateError != null)
                    return APIOperationResponse<bool>.BadRequest(duplicateError);

                var now = _dateTimeProvider.Now;
                var userId = _currentUserService.UserId;

                _mapper.Map(inputDto, existing);
                if (existing.RequestType != RequestType.Order && !inputDto.AllowanceContext.HasValue)
                    existing.AllowanceContext = RequestPurposeAllowanceContext.Both;
                existing.ModificationDate = now;
                existing.ModifiedBy = userId;

                await _requestPurposeRepository.UpdateAsync(existing);

                var attachmentRows = await LoadAttachmentRequirementsForPurposeAsync(existing.Id);
                await UpsertAttachmentRequirementsAsync(existing.Id, inputDto.AttachmentRequirements, attachmentRows, now, userId);

                return APIOperationResponse<bool>.Success(true, "Request purpose updated successfully");
            }
            catch (DbUpdateException dbEx) when (TryMapDuplicateDbException(dbEx, out var duplicateMessage))
            {
                return APIOperationResponse<bool>.BadRequest(duplicateMessage);
            }
            catch (DbUpdateException)
            {
                return APIOperationResponse<bool>.ServerError(SaveFailedKey);
            }
            catch (Exception)
            {
                return APIOperationResponse<bool>.ServerError(SaveFailedKey);
            }
        }

        /// <summary>
        /// Upserts attachment slots for a RequestPurpose (<see cref="PurposeParent"/> + <paramref name="requestPurposeId"/>).
        /// </summary>
        private async Task UpsertAttachmentRequirementsAsync(
            long requestPurposeId,
            List<CreateUpdateAttachmentRequirementDto> incoming,
            ICollection<AttachmentRequirement> existing,
            DateTime now,
            string? userId)
        {
            incoming ??= new List<CreateUpdateAttachmentRequirementDto>();
            existing ??= new List<AttachmentRequirement>();

            var existingActive = existing
                .Where(ar => !ar.IsDeleted
                            && ar.ParentType == PurposeParent
                            && ar.ParentId == requestPurposeId)
                .ToList();

            var incomingIds = incoming
                .Where(x => x.Id.HasValue && x.Id.Value > 0)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            foreach (var stale in existingActive.Where(ar => !incomingIds.Contains(ar.Id)))
            {
                stale.IsDeleted = true;
                stale.DeletionDate = now;
                stale.DeletedBy = userId;
                await _attachmentRequirementRepository.UpdateAsync(stale);
            }

            foreach (var dto in incoming)
            {
                if (dto.Id.HasValue && dto.Id.Value > 0)
                {
                    var row = existingActive.FirstOrDefault(ar =>
                        ar.Id == dto.Id.Value
                        && ar.ParentType == PurposeParent
                        && ar.ParentId == requestPurposeId);

                    if (row == null)
                        continue;

                    row.NameAr = dto.NameAr;
                    row.NameEn = dto.NameEn;
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
                    var entity = _mapper.Map<AttachmentRequirement>(dto);
                    entity.ParentType = PurposeParent;
                    entity.ParentId = requestPurposeId;
                    entity.CreationDate = now;
                    entity.CreatedBy = userId;
                    await _attachmentRequirementRepository.AddAsync(entity);
                }
            }
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

        private async Task<string?> ValidateUniqueNamesAsync(string? nameEn, string? nameAr, long? excludeId)
        {
            var trimmedEn = nameEn?.Trim();
            var trimmedAr = nameAr?.Trim();

            if (!string.IsNullOrEmpty(trimmedEn))
            {
                var existingEn = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.NameEn == trimmedEn && (!excludeId.HasValue || rp.Id != excludeId.Value),
                    includeSoftDeleted: true);

                if (existingEn != null)
                    return DuplicateNameEnKey;
            }

            if (!string.IsNullOrEmpty(trimmedAr))
            {
                var existingAr = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.NameAr == trimmedAr && (!excludeId.HasValue || rp.Id != excludeId.Value),
                    includeSoftDeleted: true);

                if (existingAr != null)
                    return DuplicateNameArKey;
            }

            return null;
        }

        private static bool TryMapDuplicateDbException(DbUpdateException dbEx, out string message)
        {
            var errorMessage = GetFullExceptionMessage(dbEx);

            if (errorMessage.Contains("IX_RequestPurposes_NameEn", StringComparison.OrdinalIgnoreCase)
                || (errorMessage.Contains("NameEn", StringComparison.OrdinalIgnoreCase)
                    && errorMessage.Contains("duplicate", StringComparison.OrdinalIgnoreCase)))
            {
                message = DuplicateNameEnKey;
                return true;
            }

            if (errorMessage.Contains("IX_RequestPurposes_NameAr", StringComparison.OrdinalIgnoreCase)
                || (errorMessage.Contains("NameAr", StringComparison.OrdinalIgnoreCase)
                    && errorMessage.Contains("duplicate", StringComparison.OrdinalIgnoreCase)))
            {
                message = DuplicateNameArKey;
                return true;
            }

            if (errorMessage.Contains("RequestPurposes", StringComparison.OrdinalIgnoreCase)
                && (errorMessage.Contains("UNIQUE KEY", StringComparison.OrdinalIgnoreCase)
                    || errorMessage.Contains("unique constraint", StringComparison.OrdinalIgnoreCase)
                    || errorMessage.Contains("duplicate key", StringComparison.OrdinalIgnoreCase)
                    || errorMessage.Contains("Cannot insert duplicate", StringComparison.OrdinalIgnoreCase)))
            {
                message = DuplicateNameGenericKey;
                return true;
            }

            message = string.Empty;
            return false;
        }

        private static string GetFullExceptionMessage(Exception ex)
        {
            var messages = new List<string>();
            for (var current = ex; current != null; current = current.InnerException)
            {
                if (!string.IsNullOrWhiteSpace(current.Message))
                    messages.Add(current.Message);
            }

            return string.Join(" ", messages);
        }
    }
}
