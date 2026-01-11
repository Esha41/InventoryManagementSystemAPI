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

        public async Task<APIOperationResponse<UserDelegationDto>> CreateDelegationAsync(CreateUserDelegationDto dto)
        {
            var currentUserId = _currentUserService.UserId;
            // Use Qatar Time (UTC+3) for business logic validations as the user operates in this timezone
            var qatarNow = DateTime.UtcNow.AddHours(3);

            // Adjust EndDate to be the end of the day (Inclusive)
            dto.EndDate = dto.EndDate.Date.AddDays(1).AddTicks(-1);

            if (dto.StartDate > dto.EndDate)
            {
                return APIOperationResponse<UserDelegationDto>.Fail(ResponseType.BadRequest, "Start date must be before end date.");
            }

            if (dto.StartDate.Date < qatarNow.Date)
            {
                 // Allow same day start (in Qatar time), but warn if date is strictly in the past
                 if (dto.EndDate < qatarNow)
                     return APIOperationResponse<UserDelegationDto>.Fail(ResponseType.BadRequest, "End date must be in the future.");
            }

            if (dto.DelegateeUserId == currentUserId)
            {
                return APIOperationResponse<UserDelegationDto>.Fail(ResponseType.BadRequest, "You cannot delegate to yourself.");
            }

            // check if delegatee exists
            var delegatee = await _userManager.FindByIdAsync(dto.DelegateeUserId);
            if (delegatee == null)
            {
                return APIOperationResponse<UserDelegationDto>.Fail(ResponseType.NotFound, "Delegatee user not found.");
            }

            // Check overlap
            var hasOverlap = await _context.UserDelegations
                .AnyAsync(d => d.DelegatorUserId == currentUserId &&
                               d.IsActive &&
                               d.StartDate < dto.EndDate &&
                               dto.StartDate < d.EndDate);

            if (hasOverlap)
            {
                return APIOperationResponse<UserDelegationDto>.Fail(ResponseType.BadRequest, "You already have an active delegation overlapping with this period.");
            }

            var entity = new UserDelegation
            {
                DelegatorUserId = currentUserId,
                DelegateeUserId = dto.DelegateeUserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                IsActive = true,
                CreatedBy = currentUserId,
                CreatedDate = DateTime.UtcNow 
            };

            _context.UserDelegations.Add(entity);
            await _context.SaveChangesAsync();

            // Load relations for DTO
            await _context.Entry(entity).Reference(e => e.DelegateeUser).LoadAsync();
            await _context.Entry(entity).Reference(e => e.DelegatorUser).LoadAsync();

            return APIOperationResponse<UserDelegationDto>.Success(MapToDto(entity));
        }

        public async Task<List<string>> GetActiveDelegatorsForUserAsync(string delegateeUserId)
        {
            // Use Qatar Time (UTC+3) for checking active status
            var qatarNow = DateTime.UtcNow.AddHours(3);
            
            return await _context.UserDelegations
                .Where(d => d.DelegateeUserId == delegateeUserId &&
                            d.IsActive &&
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
                .Where(d => d.DelegatorUserId == currentUserId)
                .OrderByDescending(d => d.StartDate)
                .ToListAsync();

            var dtos = delegations.Select(MapToDto).ToList();
            return APIOperationResponse<List<UserDelegationDto>>.Success(dtos);
        }

        public async Task<APIOperationResponse<bool>> RevokeDelegationAsync(int delegationId)
        {
            var currentUserId = _currentUserService.UserId;

            var delegation = await _context.UserDelegations.FindAsync(delegationId);
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
            delegation.ModifiedDate = DateTime.UtcNow;

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
                CreatedDate = entity.CreatedDate,
                Status = status
            };
        }
    }
}
