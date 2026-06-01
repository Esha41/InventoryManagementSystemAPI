using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Comman.Idenitity;
using Ettad.Data.Interfaces.Repositories;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Ettad.User.Services.Events;
using Ettad.CrossCutting.Comman.Time;
using SettingsEntity = Ettad.Data.Entities.Settings.Settings;

namespace Ettad.User.Services.Services
{
    public class UserDelegationService : IUserDelegationService, IDelegationAuthorizationService
    {
        private readonly ICrossCuttingRepository<UserDelegation> _userDelegationRepository;
        private readonly ICrossCuttingRepository<SettingsEntity> _settingsRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserDelegationService> _logger;
        private readonly IMediator _mediator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEffectiveRoleRepository _effectiveRoleService;
        private readonly IPermissionService _permissionService;

        public UserDelegationService(
            ICrossCuttingRepository<UserDelegation> userDelegationRepository,
            ICrossCuttingRepository<SettingsEntity> settingsRepository,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            ILogger<UserDelegationService> logger,
            IMediator mediator,
            IDateTimeProvider dateTimeProvider,
            IEffectiveRoleRepository effectiveRoleService,
            IPermissionService permissionService)
        {
            _userDelegationRepository = userDelegationRepository;
            _settingsRepository = settingsRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
            _dateTimeProvider = dateTimeProvider;
            _effectiveRoleService = effectiveRoleService;
            _permissionService = permissionService;
        }

        public async Task<APIOperationResponse<bool>> CreateDelegationAsync(CreateUserDelegationDto dto)
        {
            var currentUserId = _currentUserService.UserId;

            dto.EndDate = dto.EndDate.Date.AddDays(1).AddTicks(-1);

            var validationResult = await ValidateDelegationAsync(dto, currentUserId);
            if (!validationResult.Succeeded)
            {
                return APIOperationResponse<bool>.Fail(
                    (ResponseType)validationResult.StatusCode,
                    validationResult.Message);
            }

            // Capture the role the delegator is currently logged in with. The delegatee will
            // inherit exactly this role's permissions/authority/visibility while the delegation
            // is active. Fall back to the delegator's effective role if the active role claim
            // is unavailable.
            var delegatorRoleId = _currentUserService.ActiveRoleId;
            if (string.IsNullOrEmpty(delegatorRoleId))
            {
                delegatorRoleId = await _effectiveRoleService.GetEffectiveRoleIdAsync(currentUserId);
            }

            var entity = new UserDelegation
            {
                DelegatorUserId = currentUserId,
                DelegateeUserId = dto.DelegateeUserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                IsActive = true,
                DelegationStatus = 0,
                DelegationScopes = 0,
                DelegatorRoleId = delegatorRoleId,
                CreatedBy = currentUserId,
                CreationDate = _dateTimeProvider.Now
            };

            await _userDelegationRepository.AddAsync(entity);

            await _mediator.Publish(new DelegationCreatedEvent
            {
                DelegationId = entity.Id,
                DelegatorUserId = currentUserId,
                DelegateeUserId = dto.DelegateeUserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason
            });

            return APIOperationResponse<bool>.Success(true, "Delegation request sent successfully. Awaiting approval from delegatee.");
        }

        private async Task<APIOperationResponse<bool>> ValidateDelegationAsync(CreateUserDelegationDto dto, string currentUserId)
        {
            var now = _dateTimeProvider.Now;

            if (dto.StartDate > dto.EndDate)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Start date must be before end date.");
            }

            if (dto.StartDate.Date < now.Date)
            {
                if (dto.EndDate < now)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "End date must be in the future.");
            }

            if (dto.DelegateeUserId == currentUserId)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "You cannot delegate to yourself.");
            }

            var delegatee = await _userManager.FindByIdAsync(dto.DelegateeUserId);
            if (delegatee == null)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Delegatee user not found.");
            }

            var hasOverlap = await _userDelegationRepository
                .Find(d => d.DelegatorUserId == currentUserId &&
                           d.IsActive && !d.IsDeleted &&
                           d.StartDate < dto.EndDate &&
                           dto.StartDate < d.EndDate)
                .AnyAsync();

            if (hasOverlap)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "You already have an active delegation overlapping with this period.");
            }

            return APIOperationResponse<bool>.Success(true);
        }

        public async Task<List<string>> GetActiveDelegatorsForUserAsync(string delegateeUserId)
        {
            var now = _dateTimeProvider.Now;

            return await _userDelegationRepository
                .Find(d => d.DelegateeUserId == delegateeUserId &&
                            d.IsActive && !d.IsDeleted &&
                            d.DelegationStatus == 1 &&
                            d.StartDate <= now &&
                            d.EndDate >= now)
                .Select(d => d.DelegatorUserId)
                .ToListAsync();
        }

        public async Task<List<ActiveDelegationInfo>> GetActiveDelegationsForUserAsync(string delegateeUserId)
        {
            var now = _dateTimeProvider.Now;

            return await _userDelegationRepository
                .Find(d => d.DelegateeUserId == delegateeUserId &&
                            d.IsActive && !d.IsDeleted &&
                            d.DelegationStatus == 1 &&
                            d.StartDate <= now &&
                            d.EndDate >= now)
                .Select(d => new ActiveDelegationInfo
                {
                    DelegatorUserId = d.DelegatorUserId,
                    DelegatorRoleId = d.DelegatorRoleId
                })
                .ToListAsync();
        }

        public async Task<APIOperationResponse<List<UserDelegationDto>>> GetMyDelegationsAsync()
        {
            var currentUserId = _currentUserService.UserId;

            var delegations = await _userDelegationRepository
                .Find(d => !d.IsDeleted && (
                    d.DelegatorUserId == currentUserId ||
                    d.DelegateeUserId == currentUserId && d.DelegationStatus != 0
                ), false, "DelegateeUser", "DelegatorUser")
                .OrderByDescending(d => d.StartDate)
                .ToListAsync();

            var dtos = delegations.Select(d => MapToDto(d, currentUserId)).ToList();
            return APIOperationResponse<List<UserDelegationDto>>.Success(dtos);
        }

        public async Task<APIOperationResponse<List<UserDelegationDto>>> GetDelegationHistoryAsync()
        {
            var delegations = await _userDelegationRepository
                .Find(d => !d.IsDeleted, false, "DelegatorUser", "DelegateeUser")
                .OrderByDescending(d => d.CreationDate)
                .ToListAsync();

            var dtos = delegations.Select(MapToDto).ToList();
            return APIOperationResponse<List<UserDelegationDto>>.Success(dtos);
        }

        public async Task<APIOperationResponse<bool>> GetAllowCrossDepartmentDelegationAsync()
        {
            var setting = await _settingsRepository
                .Find(s => s.Key == "Delegation.AllowCrossDepartment")
                .FirstOrDefaultAsync();

            bool allow = true;
            if (setting != null && bool.TryParse(setting.Value, out bool val))
            {
                allow = val;
            }

            return APIOperationResponse<bool>.Success(allow);
        }

        public async Task<APIOperationResponse<bool>> UpdateAllowCrossDepartmentDelegationAsync(bool allow)
        {
            var currentUserId = _currentUserService.UserId;
            var setting = await _settingsRepository
                .Find(s => s.Key == "Delegation.AllowCrossDepartment")
                .FirstOrDefaultAsync();

            if (setting == null)
            {
                await _settingsRepository.AddAsync(new SettingsEntity
                {
                    Key = "Delegation.AllowCrossDepartment",
                    Value = allow.ToString(),
                    Group = "Delegation",
                    CreatedBy = currentUserId,
                    CreationDate = _dateTimeProvider.Now
                });
            }
            else
            {
                setting.Value = allow.ToString();
                setting.ModifiedBy = currentUserId;
                setting.ModificationDate = _dateTimeProvider.Now;
                await _settingsRepository.UpdateAsync(setting);
            }

            return APIOperationResponse<bool>.Success(true, "Delegation settings updated successfully.");
        }

        public async Task<APIOperationResponse<bool>> GetAllowDelegatorActionAsync()
        {
            var setting = await _settingsRepository
                .Find(s => s.Key == "Delegation.AllowDelegatorAction")
                .FirstOrDefaultAsync();

            bool allow = true;
            if (setting != null && bool.TryParse(setting.Value, out bool val))
            {
                allow = val;
            }

            return APIOperationResponse<bool>.Success(allow);
        }

        public async Task<APIOperationResponse<bool>> UpdateAllowDelegatorActionAsync(bool allow)
        {
            var currentUserId = _currentUserService.UserId;
            var setting = await _settingsRepository
                .Find(s => s.Key == "Delegation.AllowDelegatorAction")
                .FirstOrDefaultAsync();

            if (setting == null)
            {
                await _settingsRepository.AddAsync(new SettingsEntity
                {
                    Key = "Delegation.AllowDelegatorAction",
                    Value = allow.ToString(),
                    Group = "Delegation",
                    CreatedBy = currentUserId,
                    CreationDate = _dateTimeProvider.Now
                });
            }
            else
            {
                setting.Value = allow.ToString();
                setting.ModifiedBy = currentUserId;
                setting.ModificationDate = _dateTimeProvider.Now;
                await _settingsRepository.UpdateAsync(setting);
            }

            return APIOperationResponse<bool>.Success(true, "Delegation settings updated successfully.");
        }

        public async Task<APIOperationResponse<List<UserDto>>> GetAvailableUsersAsync()
        {
            var currentUserId = _currentUserService.UserId;

            var allowCrossDepartment = (await GetAllowCrossDepartmentDelegationAsync()).Data;

            var query = _userManager.Users
                .Where(u => u.Id != currentUserId && u.IsActive && !u.IsDeleted);

            if (!allowCrossDepartment)
            {
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                if (currentUser?.DepartmentId != null)
                {
                    query = query.Where(u => u.DepartmentId == currentUser.DepartmentId);
                }
            }

            var users = await query
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullNameEN = u.FullNameEN,
                    FullNameAR = u.FullNameAR,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    MilitoryId = u.MilitoryId,
                    DepartmentId = u.DepartmentId,
                    RankId = u.RankId
                })
                .ToListAsync();

            return APIOperationResponse<List<UserDto>>.Success(users);
        }

        public async Task<APIOperationResponse<bool>> RevokeDelegationAsync(int delegationId)
        {
            var currentUserId = _currentUserService.UserId;

            var delegation = await _userDelegationRepository.FindOneAsync(d => d.Id == delegationId && !d.IsDeleted);
            if (delegation == null)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Delegation not found.");
            }

            if (delegation.DelegatorUserId != currentUserId &&
                !_currentUserService.IsSuperAdmin &&
                !_currentUserService.IsUserHasClaim("Permissions.UserDelegations.Delete") &&
                !_currentUserService.IsUserHasClaim("DelegationManagement"))
            {
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only revoke your own delegations.");
            }

            delegation.IsActive = false;
            delegation.ModifiedBy = currentUserId;
            delegation.ModificationDate = _dateTimeProvider.Now;

            await _userDelegationRepository.UpdateAsync(delegation);

            // Delegatee no longer inherits this delegation's role; refresh their cached permissions.
            await _permissionService.InvalidatePermissionCacheForUserAsync(delegation.DelegateeUserId);

            return APIOperationResponse<bool>.Success(true, "Delegation revoked successfully.");
        }

        public async Task<APIOperationResponse<bool>> ApproveDelegationAsync(int delegationId)
        {
            var currentUserId = _currentUserService.UserId;

            var delegation = await _userDelegationRepository.FindOneAsync(d => d.Id == delegationId && !d.IsDeleted);

            if (delegation == null)
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Delegation not found.");

            if (delegation.DelegateeUserId != currentUserId)
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only approve delegations assigned to you.");

            if (delegation.DelegationStatus != 0)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This delegation has already been processed.");

            if (!delegation.IsActive)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This delegation has been revoked or is no longer active.");

            delegation.DelegationStatus = 1;
            delegation.ModifiedBy = currentUserId;
            delegation.ModificationDate = _dateTimeProvider.Now;

            await _userDelegationRepository.UpdateAsync(delegation);

            var delegatee = await _userManager.FindByIdAsync(currentUserId);
            var delegateeName = delegatee?.FullNameEN ?? delegatee?.FullNameAR ?? delegatee?.UserName ?? "Unknown";

            await _mediator.Publish(new DelegationApprovedEvent
            {
                DelegationId = delegation.Id,
                DelegatorUserId = delegation.DelegatorUserId,
                DelegateeUserId = currentUserId,
                DelegateeName = delegateeName
            });

            // Delegatee now inherits the delegator's role; refresh their cached permissions.
            await _permissionService.InvalidatePermissionCacheForUserAsync(delegation.DelegateeUserId);

            return APIOperationResponse<bool>.Success(true, "Delegation approved successfully.");
        }

        public async Task<APIOperationResponse<bool>> RejectDelegationAsync(int delegationId)
        {
            var currentUserId = _currentUserService.UserId;

            var delegation = await _userDelegationRepository.FindOneAsync(d => d.Id == delegationId && !d.IsDeleted);

            if (delegation == null)
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Delegation not found.");

            if (delegation.DelegateeUserId != currentUserId)
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only reject delegations assigned to you.");

            if (delegation.DelegationStatus != 0)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This delegation has already been processed.");

            if (!delegation.IsActive)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This delegation has been revoked or is no longer active.");

            delegation.DelegationStatus = 2;
            delegation.IsActive = false;
            delegation.ModifiedBy = currentUserId;
            delegation.ModificationDate = _dateTimeProvider.Now;

            await _userDelegationRepository.UpdateAsync(delegation);

            var delegatee = await _userManager.FindByIdAsync(currentUserId);
            var delegateeName = delegatee?.FullNameEN ?? delegatee?.FullNameAR ?? delegatee?.UserName ?? "Unknown";

            await _mediator.Publish(new DelegationRejectedEvent
            {
                DelegationId = delegation.Id,
                DelegatorUserId = delegation.DelegatorUserId,
                DelegateeUserId = currentUserId,
                DelegateeName = delegateeName
            });

            // Delegatee no longer inherits this delegation's role; refresh their cached permissions.
            await _permissionService.InvalidatePermissionCacheForUserAsync(delegation.DelegateeUserId);

            return APIOperationResponse<bool>.Success(true, "Delegation rejected successfully.");
        }

        public async Task<APIOperationResponse<List<UserDelegationDto>>> GetPendingDelegationsAsync()
        {
            var currentUserId = _currentUserService.UserId;

            var delegations = await _userDelegationRepository
                .Find(d => d.DelegateeUserId == currentUserId &&
                            !d.IsDeleted &&
                            d.IsActive &&
                            d.DelegationStatus == 0,
                    false,
                    "DelegateeUser",
                    "DelegatorUser")
                .OrderByDescending(d => d.CreationDate)
                .ToListAsync();

            var dtos = delegations.Select(MapToDto).ToList();
            return APIOperationResponse<List<UserDelegationDto>>.Success(dtos);
        }

        private UserDelegationDto MapToDto(UserDelegation entity)
        {
            return MapToDto(entity, _currentUserService.UserId);
        }

        private UserDelegationDto MapToDto(UserDelegation entity, string currentUserId)
        {
            var now = _dateTimeProvider.Now;
            string status = "Expired";
            if (entity.IsActive)
            {
                if (now < entity.StartDate) status = "Future";
                else if (now >= entity.StartDate && now <= entity.EndDate) status = "Active";
            }

            return new UserDelegationDto
            {
                Id = entity.Id,
                DelegatorUserId = entity.DelegatorUserId,
                DelegatorUserName = entity.DelegatorUser?.UserName,
                DelegatorFullName = entity.DelegatorUser?.FullNameEN ?? entity.DelegatorUser?.FullNameAR,
                DelegateeUserId = entity.DelegateeUserId,
                DelegateeUserName = entity.DelegateeUser?.UserName,
                DelegateeFullName = entity.DelegateeUser?.FullNameEN ?? entity.DelegateeUser?.FullNameAR,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Reason = entity.Reason,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreationDate,
                Status = status,
                DelegationStatus = entity.DelegationStatus,
                IsIncoming = entity.DelegateeUserId == currentUserId
            };
        }

        public async Task<APIOperationResponse<List<UserDelegationDto>>> GetAllDelegationsAsync()
        {
            var delegations = await _userDelegationRepository
                .Find(d => !d.IsDeleted, false, "DelegatorUser", "DelegateeUser")
                .OrderByDescending(d => d.CreationDate)
                .ToListAsync();

            var dtos = delegations.Select(MapToDto).ToList();
            return APIOperationResponse<List<UserDelegationDto>>.Success(dtos);
        }

        public async Task<bool> IsUserRestrictedByDelegationAsync(string userId)
        {
            var settingResponse = await GetAllowDelegatorActionAsync();
            if (settingResponse.Succeeded && settingResponse.Data)
            {
                return false;
            }

            var now = _dateTimeProvider.Now;

            var hasActiveDelegation = await _userDelegationRepository
                .Find(d => d.DelegatorUserId == userId &&
                           !d.IsDeleted &&
                           d.IsActive &&
                           d.DelegationStatus == 1 &&
                           d.StartDate <= now &&
                           d.EndDate >= now)
                .AnyAsync();

            return hasActiveDelegation;
        }

        public async Task<bool> IsUserRestrictedAsync(string userId)
        {
            return await IsUserRestrictedByDelegationAsync(userId);
        }
    }
}
