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
        
        Task<List<string>> GetActiveDelegatorsForUserAsync(string delegateeUserId);

        Task<APIOperationResponse<List<UserDto>>> GetAvailableUsersAsync(); 
    }
}
