using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    public interface IUserDelegationService
    {
        Task<APIOperationResponse<bool>> CreateDelegationAsync(CreateUserDelegationDto dto);
        
        Task<APIOperationResponse<bool>> RevokeDelegationAsync(int delegationId);
        
        Task<APIOperationResponse<List<UserDelegationDto>>> GetMyDelegationsAsync(); 
        
        /// <summary>
        /// Gets active delegators for a user, optionally filtered by delegation scope.
        /// </summary>
        /// <param name="delegateeUserId">The user ID of the delegatee</param>
        /// <param name="scope">Optional scope to filter by. If null, returns all active delegators.</param>
        /// <returns>List of delegator user IDs</returns>
        Task<List<string>> GetActiveDelegatorsForUserAsync(string delegateeUserId, DelegationScope? scope = null);

        Task<APIOperationResponse<List<UserDto>>> GetAvailableUsersAsync();

        Task<APIOperationResponse<bool>> ApproveDelegationAsync(int delegationId);

        Task<APIOperationResponse<bool>> RejectDelegationAsync(int delegationId);

        Task<APIOperationResponse<List<UserDelegationDto>>> GetPendingDelegationsAsync();

        // Admin methods
        Task<APIOperationResponse<List<UserDelegationDto>>> GetAllDelegationsAsync();

        Task<APIOperationResponse<List<UserDelegationDto>>> GetDelegationHistoryAsync();

        Task<APIOperationResponse<bool>> GetAllowCrossDepartmentDelegationAsync();
        Task<APIOperationResponse<bool>> UpdateAllowCrossDepartmentDelegationAsync(bool allow);

        Task<APIOperationResponse<bool>> GetAllowDelegatorActionAsync();
        Task<APIOperationResponse<bool>> UpdateAllowDelegatorActionAsync(bool allow);

        Task<bool> IsUserRestrictedByDelegationAsync(string userId);
    }
}
