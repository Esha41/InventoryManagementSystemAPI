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

namespace Ettad.User.Services.Implementation
{
    public class UserDelegationService : IUserDelegationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserDelegationService> _logger;

        public UserDelegationService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            ILogger<UserDelegationService> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _logger = logger;
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
                CreatedBy = currentUserId,
                CreationDate = DateTime.UtcNow 
            };

            _context.UserDelegations.Add(entity);
            await _context.SaveChangesAsync();

            return APIOperationResponse<bool>.Success(true, "Delegation created successfully.");
        }

        private async Task<APIOperationResponse<bool>> ValidateDelegationAsync(CreateUserDelegationDto dto, string currentUserId)
        {
            // Use Qatar Time (UTC+3) for business logic validations
            var qatarNow = DateTime.UtcNow.AddHours(3);

            if (dto.StartDate > dto.EndDate)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Start date must be before end date.");
            }

            if (dto.StartDate.Date < qatarNow.Date)
            {
                // Allow same day start (in Qatar time), but warn if date is strictly in the past
                if (dto.EndDate < qatarNow)
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
            // Use Qatar Time (UTC+3) for checking active status
            var qatarNow = DateTime.UtcNow.AddHours(3);
            
            return await _context.UserDelegations
                .Where(d => d.DelegateeUserId == delegateeUserId &&
                            d.IsActive && !d.IsDeleted &&
                            d.StartDate <= qatarNow &&
                            d.EndDate >= qatarNow)
                .Select(d => d.DelegatorUserId)
                .ToListAsync();
        }

        public async Task<APIOperationResponse<List<UserDelegationDto>>> GetMyDelegationsAsync()
        {
            var currentUserId = _currentUserService.UserId;

            var delegations = await _context.UserDelegations
                .Include(d => d.DelegateeUser)
                .Include(d => d.DelegatorUser)
                .Where(d => d.DelegatorUserId == currentUserId && !d.IsDeleted)
                .OrderByDescending(d => d.StartDate)
                .ToListAsync();

            var dtos = delegations.Select(MapToDto).ToList();
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
            delegation.ModificationDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return APIOperationResponse<bool>.Success(true, "Delegation revoked successfully.");
        }

        private UserDelegationDto MapToDto(UserDelegation entity)
        {
            // Map status based on Qatar Time
            var qatarNow = DateTime.UtcNow.AddHours(3);
            string status = "Expired";
            if (entity.IsActive)
            {
                if (qatarNow < entity.StartDate) status = "Future";
                else if (qatarNow >= entity.StartDate && qatarNow <= entity.EndDate) status = "Active";
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
                Status = status
            };
        }
    }
}
