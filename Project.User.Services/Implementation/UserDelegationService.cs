using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Comman.Idenitity;
using Ettad.EntityFramework.DataBaseContext;
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

namespace Ettad.User.Services.Implementation
{
    public class UserDelegationService : IUserDelegationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserDelegationService> _logger;
        private readonly IMediator _mediator;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserDelegationService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            ILogger<UserDelegationService> logger,
            IMediator mediator,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<bool>> CreateDelegationAsync(CreateUserDelegationDto dto)
        {
            var currentUserId = _currentUserService.UserId;
            
            // Adjust EndDate to be the end of the day (Inclusive)
            dto.EndDate = dto.EndDate.Date.AddDays(1).AddTicks(-1);

            // 1. Validation Logic
            var validationResult = await ValidateDelegationAsync(dto, currentUserId);
            if (!validationResult.Succeeded)
            {
                return APIOperationResponse<bool>.Fail(
                    (ResponseType)validationResult.StatusCode, 
                    validationResult.Message);
            }

            // 2. Creation Logic
            var entity = new UserDelegation
            {
                DelegatorUserId = currentUserId,
                DelegateeUserId = dto.DelegateeUserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                IsActive = true,
                DelegationStatus = 0, // Pending approval
                CreatedBy = currentUserId,
                CreationDate = _dateTimeProvider.Now 
            };

            _context.UserDelegations.Add(entity);
            await _context.SaveChangesAsync();

            // Publish event for notification handling
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
            // Use local time for business logic validations
            var now = _dateTimeProvider.Now;

            if (dto.StartDate > dto.EndDate)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Start date must be before end date.");
            }

            if (dto.StartDate.Date < now.Date)
            {
                // Allow same day start, but warn if date is strictly in the past
                if (dto.EndDate < now)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "End date must be in the future.");
            }

            if (dto.DelegateeUserId == currentUserId)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "You cannot delegate to yourself.");
            }

            // check if delegatee exists
            var delegatee = await _userManager.FindByIdAsync(dto.DelegateeUserId);
            if (delegatee == null)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Delegatee user not found.");
            }

            // Check overlap
            var hasOverlap = await _context.UserDelegations
                .AnyAsync(d => d.DelegatorUserId == currentUserId &&
                               d.IsActive && !d.IsDeleted &&
                               d.StartDate < dto.EndDate &&
                               dto.StartDate < d.EndDate);

            if (hasOverlap)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "You already have an active delegation overlapping with this period.");
            }

            return APIOperationResponse<bool>.Success(true);
        }

        public async Task<List<string>> GetActiveDelegatorsForUserAsync(string delegateeUserId)
        {
            // Use local time for checking active status
            var now = _dateTimeProvider.Now;
            
            return await _context.UserDelegations
                .Where(d => d.DelegateeUserId == delegateeUserId &&
                            d.IsActive && !d.IsDeleted &&
                            d.DelegationStatus == 1 && // Only approved delegations
                            d.StartDate <= now &&
                            d.EndDate >= now)
                .Select(d => d.DelegatorUserId)
                .ToListAsync();
        }

        public async Task<APIOperationResponse<List<UserDelegationDto>>> GetMyDelegationsAsync()
        {
            var currentUserId = _currentUserService.UserId;

            // Fetch delegations where I am the delegator (outgoing) 
            // OR where I am the delegatee (incoming) AND it's already approved/rejected
            var delegations = await _context.UserDelegations
                .Include(d => d.DelegateeUser)
                .Include(d => d.DelegatorUser)
                .Where(d => !d.IsDeleted && (
                    d.DelegatorUserId == currentUserId || 
                    (d.DelegateeUserId == currentUserId && d.DelegationStatus != 0)
                ))
                .OrderByDescending(d => d.StartDate)
                .ToListAsync();

            var dtos = delegations.Select(d => MapToDto(d, currentUserId)).ToList();
            return APIOperationResponse<List<UserDelegationDto>>.Success(dtos);
        }

        public async Task<APIOperationResponse<List<UserDto>>> GetAvailableUsersAsync()
        {
            var currentUserId = _currentUserService.UserId;

            var users = await _userManager.Users
                .Where(u => u.Id != currentUserId && u.IsActive && !u.IsDeleted)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullNameEN = u.FullNameEN,
                    FullNameAR = u.FullNameAR,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    MilitoryId = u.MilitoryId,
                    DeparmentId = u.DepartmentId,
                    RankId = u.RankId
                })
                .ToListAsync();

            return APIOperationResponse<List<UserDto>>.Success(users);
        }

        public async Task<APIOperationResponse<bool>> RevokeDelegationAsync(int delegationId)
        {
            var currentUserId = _currentUserService.UserId;

            var delegation = await _context.UserDelegations
                .FirstOrDefaultAsync(d => d.Id == delegationId && !d.IsDeleted);
            if (delegation == null)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Delegation not found.");
            }

            if (delegation.DelegatorUserId != currentUserId)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only revoke your own delegations.");
            }

            delegation.IsActive = false;
            delegation.ModifiedBy = currentUserId;
            delegation.ModificationDate = _dateTimeProvider.Now;

            await _context.SaveChangesAsync();
            return APIOperationResponse<bool>.Success(true, "Delegation revoked successfully.");
        }

        public async Task<APIOperationResponse<bool>> ApproveDelegationAsync(int delegationId)
        {
            var currentUserId = _currentUserService.UserId;

            var delegation = await _context.UserDelegations
                .Include(d => d.DelegatorUser)
                .FirstOrDefaultAsync(d => d.Id == delegationId && !d.IsDeleted);

            if (delegation == null)
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Delegation not found.");

            if (delegation.DelegateeUserId != currentUserId)
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only approve delegations assigned to you.");

            if (delegation.DelegationStatus != 0)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This delegation has already been processed.");

            if (!delegation.IsActive)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This delegation has been revoked or is no longer active.");

            delegation.DelegationStatus = 1; // Approved
            delegation.ModifiedBy = currentUserId;
            delegation.ModificationDate = _dateTimeProvider.Now;

            await _context.SaveChangesAsync();

            // Publish event for notification handling
            var delegatee = await _userManager.FindByIdAsync(currentUserId);
            var delegateeName = delegatee?.FullNameEN ?? delegatee?.FullNameAR ?? delegatee?.UserName ?? "Unknown";

            await _mediator.Publish(new DelegationApprovedEvent
            {
                DelegationId = delegation.Id,
                DelegatorUserId = delegation.DelegatorUserId,
                DelegateeUserId = currentUserId,
                DelegateeName = delegateeName
            });

            return APIOperationResponse<bool>.Success(true, "Delegation approved successfully.");
        }

        public async Task<APIOperationResponse<bool>> RejectDelegationAsync(int delegationId)
        {
            var currentUserId = _currentUserService.UserId;

            var delegation = await _context.UserDelegations
                .Include(d => d.DelegatorUser)
                .FirstOrDefaultAsync(d => d.Id == delegationId && !d.IsDeleted);

            if (delegation == null)
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Delegation not found.");

            if (delegation.DelegateeUserId != currentUserId)
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only reject delegations assigned to you.");

            if (delegation.DelegationStatus != 0)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This delegation has already been processed.");

            if (!delegation.IsActive)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This delegation has been revoked or is no longer active.");

            delegation.DelegationStatus = 2; // Rejected
            delegation.IsActive = false;
            delegation.ModifiedBy = currentUserId;
            delegation.ModificationDate = _dateTimeProvider.Now;

            await _context.SaveChangesAsync();

            // Publish event for notification handling
            var delegatee = await _userManager.FindByIdAsync(currentUserId);
            var delegateeName = delegatee?.FullNameEN ?? delegatee?.FullNameAR ?? delegatee?.UserName ?? "Unknown";

            await _mediator.Publish(new DelegationRejectedEvent
            {
                DelegationId = delegation.Id,
                DelegatorUserId = delegation.DelegatorUserId,
                DelegateeUserId = currentUserId,
                DelegateeName = delegateeName
            });

            return APIOperationResponse<bool>.Success(true, "Delegation rejected successfully.");
        }

        public async Task<APIOperationResponse<List<UserDelegationDto>>> GetPendingDelegationsAsync()
        {
            var currentUserId = _currentUserService.UserId;

            var delegations = await _context.UserDelegations
                .Include(d => d.DelegateeUser)
                .Include(d => d.DelegatorUser)
                .Where(d => d.DelegateeUserId == currentUserId && 
                            !d.IsDeleted && 
                            d.IsActive && 
                            d.DelegationStatus == 0) // Pending only
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
            // Map status based on local time
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
            var delegations = await _context.UserDelegations
                .Include(d => d.DelegatorUser)
                .Include(d => d.DelegateeUser)
                .Where(d => !d.IsDeleted)
                .OrderByDescending(d => d.CreationDate)
                .ToListAsync();

            var dtos = delegations.Select(MapToDto).ToList();
            return APIOperationResponse<List<UserDelegationDto>>.Success(dtos);
        }

        public async Task<APIOperationResponse<List<UserDelegationDto>>> GetDelegationHistoryAsync()
        {
            // Get all delegations including inactive/expired ones
            var delegations = await _context.UserDelegations
                .Include(d => d.DelegatorUser)
                .Include(d => d.DelegateeUser)
                .Where(d => !d.IsDeleted)
                .OrderByDescending(d => d.CreationDate)
                .ToListAsync();

            var dtos = delegations.Select(MapToDto).ToList();
            return APIOperationResponse<List<UserDelegationDto>>.Success(dtos);
        }
    }
}
