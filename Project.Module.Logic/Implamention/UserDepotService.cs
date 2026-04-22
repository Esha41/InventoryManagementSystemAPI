using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Lookups.Services.Contracts;
using Ettad.Module.lookup.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Data.Interfaces.Repositories;

namespace Ettad.Lookups.Services.Implementation
{
    /// <summary>
    /// Service for managing user-depot assignments.
    /// </summary>
    public class UserDepotService : IUserDepotService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrossCuttingRepository<UserDepot> _userDepotRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UserDepotService> _logger;
        private readonly ITransactionManager _transactionManager;

        public UserDepotService(
            ApplicationDbContext context,
            ICrossCuttingRepository<UserDepot> userDepotRepository,
            ICurrentUserService currentUserService,
            ILogger<UserDepotService> logger,
            ITransactionManager transactionManager)
        {
            _context = context;
            _userDepotRepository = userDepotRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _transactionManager = transactionManager;
        }

        public async Task<APIOperationResponse<List<DepotUserDto>>> GetUsersByDepotIdAsync(long depotId, CancellationToken cancellationToken = default)
        {
            try
            {
                var depotExists = await _context.Depots.AnyAsync(d => d.Id == depotId && !d.IsDeleted, cancellationToken);
                if (!depotExists)
                {
                    _logger.LogWarning("Depot {DepotId} not found or deleted", depotId);
                    return APIOperationResponse<List<DepotUserDto>>.Fail(ResponseType.NotFound, "Depot not found.");
                }

                var userDepots = await _context.UserDepots
                    .Where(ud => ud.DepotId == depotId)
                    .Join(_context.Users,
                        ud => ud.UserId,
                        u => u.Id,
                        (ud, u) => new DepotUserDto
                        {
                            Id = u.Id,
                            UserName = u.UserName ?? "",
                            FullNameEn = u.FullNameEN,
                            FullNameAr = u.FullNameAR
                        })
                    .ToListAsync(cancellationToken);

                _logger.LogInformation("Retrieved {Count} users for depot {DepotId}", userDepots.Count, depotId);
                return APIOperationResponse<List<DepotUserDto>>.Success(userDepots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users for depot {DepotId}", depotId);
                return APIOperationResponse<List<DepotUserDto>>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.OPERATION_FAILED,
                    $"Error retrieving depot users: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> SetDepotUserAssignmentsAsync(long depotId, IEnumerable<string> userIds, CancellationToken cancellationToken = default)
        {
            try
            {
                var depotExists = await _context.Depots.AnyAsync(d => d.Id == depotId && !d.IsDeleted, cancellationToken);
                if (!depotExists)
                {
                    _logger.LogWarning("Depot {DepotId} not found or deleted", depotId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Depot not found.");
                }

                var userIdList = userIds?.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList() ?? new List<string>();
                if (userIdList.Any())
                {
                    var validUserIds = await _context.Users
                        .Where(u => userIdList.Contains(u.Id) && !u.IsDeleted)
                        .Select(u => u.Id)
                        .ToListAsync(cancellationToken);

                    var invalidIds = userIdList.Except(validUserIds).ToList();
                    if (invalidIds.Any())
                    {
                        _logger.LogWarning("Invalid user IDs provided: {InvalidIds}", string.Join(", ", invalidIds));
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"Invalid user IDs: {string.Join(", ", invalidIds)}");
                    }
                }

                await using var transaction = await _transactionManager.BeginAsync(cancellationToken);
                try
                {
                    var existing = await _context.UserDepots.Where(ud => ud.DepotId == depotId).ToListAsync(cancellationToken);
                    _context.UserDepots.RemoveRange(existing);

                    foreach (var userId in userIdList)
                    {
                        _context.UserDepots.Add(new UserDepot { UserId = userId, DepotId = depotId });
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    await _transactionManager.CommitAsync(cancellationToken);

                    _logger.LogInformation("User {UserName} updated depot {DepotId} assignments. Assigned {Count} users.",
                        _currentUserService.UserName, depotId, userIdList.Count);

                    return APIOperationResponse<bool>.Success(true);
                }
                catch
                {
                    await _transactionManager.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting depot user assignments for depot {DepotId}", depotId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.OPERATION_FAILED,
                    $"Error updating depot assignments: {ex.Message}");
            }
        }
    }
}
