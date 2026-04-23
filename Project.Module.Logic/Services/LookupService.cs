using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Time;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Interface;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Module.lookup.Interfaces;

namespace Ettad.Module.lookup.Services
{
   
        public class LookupService<T, TDto> : ILookupService<T, TDto>
            where T : class, ILookup
            where TDto : class
        {
            protected readonly ICrossCuttingRepository<T> _repository;
            protected readonly IMapper _mapper;
            protected readonly ICurrentUserService _currentUserService;
            protected readonly IDateTimeProvider _dateTimeProvider;
            protected readonly ILogger<LookupService<T, TDto>> _logger;

            public LookupService(
                ICrossCuttingRepository<T> repository, 
                IMapper mapper, 
                ICurrentUserService currentUserService,
                IDateTimeProvider dateTimeProvider,
                ILogger<LookupService<T, TDto>> logger)
            {
                _repository = repository;
                _mapper = mapper;
                _currentUserService = currentUserService;
                _dateTimeProvider = dateTimeProvider;
                _logger = logger;
            }

            public async Task<APIOperationResponse<T>> GetLookupItemById(int id)
            {
                try
                {
                    _logger.LogInformation("Retrieving lookup item {LookupType} with Id {Id} for Organization {OrganizationId} by User {UserName}", 
                        typeof(T).Name, id, _currentUserService.OrganizationId, _currentUserService.UserName);

                    var result = await _repository.GetByIdAsync(id);
                    if (result == null)
                    {
                        _logger.LogWarning("Lookup item {LookupType} with Id {Id} not found for Organization {OrganizationId}", 
                            typeof(T).Name, id, _currentUserService.OrganizationId);
                        return APIOperationResponse<T>.Fail(ResponseType.NotFound, CommonErrorCodes.NOT_FOUND, $"Lookup item with id {id} not found.");
                    }

                    _logger.LogInformation("Successfully retrieved lookup item {LookupType} with Id {Id}", typeof(T).Name, id);
                    return APIOperationResponse<T>.Success(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving lookup item {LookupType} with Id {Id} for Organization {OrganizationId}", 
                        typeof(T).Name, id, _currentUserService.OrganizationId);
                    return APIOperationResponse<T>.Fail(ResponseType.InternalServerError, CommonErrorCodes.OPERATION_FAILED, $"Error retrieving lookup by Id: {ex.Message}");
                }
            }

            public async Task<APIOperationResponse<T>> AddLookupItem(TDto item)
            {
                try
                {
                    _logger.LogInformation("User {UserName} attempting to create {LookupType} lookup item for Organization {OrganizationId} with data {@LookupData}", 
                        _currentUserService.UserName, typeof(T).Name, _currentUserService.OrganizationId, item);

                    var entity = _mapper.Map<T>(item);
                    
                    // Set audit fields if entity inherits from AuditEntity
                    if (entity is AuditEntity<long> auditEntity)
                    {
                        auditEntity.CreationDate = _dateTimeProvider.Now;
                        auditEntity.CreatedBy = _currentUserService.UserId;
                    }
                    else if (entity is AuditEntity auditEntityBase)
                    {
                        auditEntityBase.CreationDate = _dateTimeProvider.Now;
                        auditEntityBase.CreatedBy = _currentUserService.UserId;
                    }
                    
                    var result = await _repository.AddAsync(entity);
                    
                    // Try to get Id property using reflection
                    var idProp = typeof(T).GetProperty("Id");
                    var idValue = idProp?.GetValue(result);
                    
                    _logger.LogWarning("{LookupType} lookup item created successfully with Id {Id} by User {UserName}", 
                        typeof(T).Name, idValue ?? "N/A", _currentUserService.UserName);
                    
                    return APIOperationResponse<T>.Success(result);
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError(dbEx, "Database error adding {LookupType} lookup item for Organization {OrganizationId} by User {UserName}", 
                        typeof(T).Name, _currentUserService.OrganizationId, _currentUserService.UserName);
                    
                    // Extract inner exception message for better error details
                    var errorMessage = dbEx.Message;
                    if (dbEx.InnerException != null)
                    {
                        errorMessage = $"{errorMessage} {dbEx.InnerException.Message}";
                    }
                    
                    // Check for unique constraint violations
                    if (errorMessage.Contains("UNIQUE KEY") || errorMessage.Contains("unique constraint") || 
                        errorMessage.Contains("duplicate key") || errorMessage.Contains("Cannot insert duplicate"))
                    {
                        return APIOperationResponse<T>.Fail(ResponseType.BadRequest, CommonErrorCodes.OPERATION_FAILED, 
                            "A record with the same code or name already exists. Please use a unique value.");
                    }
                    
                    return APIOperationResponse<T>.Fail(ResponseType.InternalServerError, CommonErrorCodes.OPERATION_FAILED, 
                        $"Error adding lookup item: {errorMessage}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding {LookupType} lookup item for Organization {OrganizationId} by User {UserName}", 
                        typeof(T).Name, _currentUserService.OrganizationId, _currentUserService.UserName);
                    
                    var errorMessage = ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMessage = $"{errorMessage} {ex.InnerException.Message}";
                    }
                    
                    return APIOperationResponse<T>.Fail(ResponseType.InternalServerError, CommonErrorCodes.OPERATION_FAILED, 
                        $"Error adding lookup item: {errorMessage}");
                }
            }

            public async Task<APIOperationResponse<T>> GetLookupItemByName(string name)
            {
                try
                {
                    _logger.LogInformation("Retrieving {LookupType} lookup item by name '{Name}' for Organization {OrganizationId}", 
                        typeof(T).Name, name, _currentUserService.OrganizationId);

                    var result = await _repository.FindOneAsync(x => x.NameAr == name || x.NameEn == name);
                    if (result == null)
                    {
                        _logger.LogWarning("{LookupType} lookup item with name '{Name}' not found for Organization {OrganizationId}", 
                            typeof(T).Name, name, _currentUserService.OrganizationId);
                        return APIOperationResponse<T> .Fail(ResponseType.NotFound, CommonErrorCodes.NOT_FOUND, $"Lookup item with name '{name}' not found.");
                    }

                    _logger.LogInformation("Successfully retrieved {LookupType} lookup item by name '{Name}'", typeof(T).Name, name);
                    return APIOperationResponse<T>.Success(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving {LookupType} lookup item by name '{Name}' for Organization {OrganizationId}", 
                        typeof(T).Name, name, _currentUserService.OrganizationId);
                    return APIOperationResponse<T>.Fail(ResponseType.InternalServerError, CommonErrorCodes.OPERATION_FAILED, $"Error retrieving lookup by name: {ex.Message}");
                }
            }

            public virtual async Task<APIOperationResponse<List<T>>> GetLookupItems(bool includeDeleted = false)
            {
                try
                {
                    _logger.LogInformation("Retrieving all {LookupType} lookup items for Organization {OrganizationId} (IncludeDeleted: {IncludeDeleted}) by User {UserName}", 
                        typeof(T).Name, _currentUserService.OrganizationId, includeDeleted, _currentUserService.UserName);

                    var items = _repository.Find(x => includeDeleted || !x.IsDeleted  );
                      
                    var result = await items.ToListAsync();
                    if (!_currentUserService.IsSuperAdmin)
                    {
                        var organIdProp = typeof(T).GetProperty("OrganizationId");
                        if (organIdProp != null){
                            result = result
                                .Where(x => (int)organIdProp.GetValue(x)! == _currentUserService.OrganizationId )
                                .ToList();
                        }
                    }

                    _logger.LogInformation("Successfully retrieved {Count} {LookupType} lookup items for Organization {OrganizationId}", 
                        result.Count, typeof(T).Name, _currentUserService.OrganizationId);

                    return APIOperationResponse<List<T>>.Success(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving {LookupType} lookup items for Organization {OrganizationId}", 
                        typeof(T).Name, _currentUserService.OrganizationId);
                    return APIOperationResponse<List<T>>.Fail(ResponseType.InternalServerError, CommonErrorCodes.OPERATION_FAILED, $"Error retrieving lookup items: {ex.Message}");
                }
            }

            public async Task<APIOperationResponse<List<T>>> GetLookupItemsByParentId(string parentKey, int parentId, bool includeDeleted = false)
            {
                try
                {
                    _logger.LogInformation("Retrieving {LookupType} lookup items by parent (Key: {ParentKey}, Id: {ParentId}) for Organization {OrganizationId}", 
                        typeof(T).Name, parentKey, parentId, _currentUserService.OrganizationId);

                    IQueryable<T> items = _repository.Find(x => includeDeleted || !x.IsDeleted);
                    var result = await items.ToListAsync();

                    if (!_currentUserService.IsSuperAdmin)
                    {
                        var organIdProp = typeof(T).GetProperty("OrganizationId");
                        if (organIdProp != null){
                            result = result
                                .Where(x => (int)organIdProp.GetValue(x)! == _currentUserService.OrganizationId)
                                .ToList();
                        }
                    }

                    _logger.LogInformation("Successfully retrieved {Count} {LookupType} lookup items by parent for Organization {OrganizationId}", 
                        result.Count, typeof(T).Name, _currentUserService.OrganizationId);

                    return APIOperationResponse<List<T>>.Success(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving {LookupType} lookup items by parent (Key: {ParentKey}, Id: {ParentId}) for Organization {OrganizationId}", 
                        typeof(T).Name, parentKey, parentId, _currentUserService.OrganizationId);
                    return APIOperationResponse<List<T>>.Fail(ResponseType.InternalServerError, CommonErrorCodes.OPERATION_FAILED, $"Error retrieving lookup items by parent: {ex.Message}");
                }
            }

            public async Task<APIOperationResponse<T>> UpdateLookupItem(long id, TDto item)
            {
                try
                {
                    _logger.LogInformation("User {UserName} attempting to update {LookupType} lookup item with Id {Id} for Organization {OrganizationId} with data {@LookupData}", 
                        _currentUserService.UserName, typeof(T).Name, id, _currentUserService.OrganizationId, item);

                    var existingEntity = await _repository.GetByIdAsync(id);
                    if (existingEntity == null)
                    {
                        _logger.LogWarning("{LookupType} lookup item with Id {Id} not found for update by User {UserName}", 
                            typeof(T).Name, id, _currentUserService.UserName);
                        return APIOperationResponse<T>.Fail(ResponseType.NotFound, CommonErrorCodes.NOT_FOUND, $"Lookup item with id {id} not found.");
                    }

                    _mapper.Map(item, existingEntity);
                    await _repository.UpdateAsync(existingEntity);

                    _logger.LogWarning("{LookupType} lookup item with Id {Id} updated successfully by User {UserName}", 
                        typeof(T).Name, id, _currentUserService.UserName);

                    return APIOperationResponse<T>.Success(existingEntity);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating {LookupType} lookup item with Id {Id} for Organization {OrganizationId} by User {UserName}", 
                        typeof(T).Name, id, _currentUserService.OrganizationId, _currentUserService.UserName);
                    return APIOperationResponse<T>.Fail(ResponseType.InternalServerError, CommonErrorCodes.OPERATION_FAILED, $"Error updating lookup item: {ex.Message}");
                }
            }

            public async Task<APIOperationResponse<List<T>>> SearchLookupItems(string searchText, bool includeDeleted = false)
            {
                try
                {
                    _logger.LogInformation("Searching {LookupType} lookup items with text '{SearchText}' for Organization {OrganizationId} by User {UserName}", 
                        typeof(T).Name, searchText, _currentUserService.OrganizationId, _currentUserService.UserName);

                    if (string.IsNullOrWhiteSpace(searchText))
                    {
                        _logger.LogInformation("Empty search text provided, returning all {LookupType} items", typeof(T).Name);
                        return await GetLookupItems(includeDeleted);
                    }

                    var items = _repository.Find(x =>
                        (includeDeleted || !x.IsDeleted) &&
                        (x.NameAr.Contains(searchText) || x.NameEn.Contains(searchText))
                    );

                    var result = await items.ToListAsync();
                    
                    _logger.LogInformation("Search for {LookupType} returned {Count} results for search text '{SearchText}'", 
                        typeof(T).Name, result.Count, searchText);

                    return APIOperationResponse<List<T>>.Success(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error searching {LookupType} lookup items with text '{SearchText}' for Organization {OrganizationId}", 
                        typeof(T).Name, searchText, _currentUserService.OrganizationId);
                    return APIOperationResponse<List<T>>.Fail(ResponseType.InternalServerError,CommonErrorCodes.OPERATION_FAILED,$"Error searching lookup items: {ex.Message}");
                }
            }

            public async Task<APIOperationResponse<T>> SoftDeleteLookupItem(long id, TDto item)
        {
            try
            {
                _logger.LogInformation("User {UserName} attempting to soft delete {LookupType} lookup item with Id {Id} for Organization {OrganizationId}", 
                    _currentUserService.UserName, typeof(T).Name, id, _currentUserService.OrganizationId);

                var existingEntity = await _repository.GetByIdAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("{LookupType} lookup item with Id {Id} not found for soft delete by User {UserName}", 
                        typeof(T).Name, id, _currentUserService.UserName);
                    return APIOperationResponse<T>.Fail(
                        ResponseType.NotFound,
                        CommonErrorCodes.NOT_FOUND,
                        $"Lookup item with id {id} not found.");
                }

                // Map the DTO to entity (so other updates can also be applied if passed)
                _mapper.Map(item, existingEntity);

                // Force soft delete
                var isDeletedProp = typeof(T).GetProperty("IsDeleted");
                if (isDeletedProp != null && isDeletedProp.PropertyType == typeof(bool))
                {
                    isDeletedProp.SetValue(existingEntity, true);
                }

                await _repository.UpdateAsync(existingEntity);

                _logger.LogWarning("{LookupType} lookup item with Id {Id} soft deleted successfully by User {UserName}", 
                    typeof(T).Name, id, _currentUserService.UserName);

                return APIOperationResponse<T>.Success(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting {LookupType} lookup item with Id {Id} for Organization {OrganizationId} by User {UserName}", 
                    typeof(T).Name, id, _currentUserService.OrganizationId, _currentUserService.UserName);
                return APIOperationResponse<T>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.OPERATION_FAILED,
                    $"Error soft deleting lookup item: {ex.Message}");
            }
        }

    }
}


