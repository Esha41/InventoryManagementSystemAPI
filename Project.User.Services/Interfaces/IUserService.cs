using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Services.DataTransferObject.AuthenticationDto;
using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{

    public interface IUserService
    {
        Task<APIOperationResponse<UserDto>> GetByIdAsync(string id);
        Task<APIOperationResponse<PaginatedList<UserDto>>> GetAllAsync(CrossCutting.Comman.Models.PagedListRequest request);
        Task<APIOperationResponse<List<UserDto>>> GetAllForExportAsync(FilterData filter);
        Task<APIOperationResponse<UserDto>> CreateAsync(CreateUserDto dto);
        Task<APIOperationResponse<UserDto>> UpdateAsync(UpdateUserDto dto);
        Task<APIOperationResponse<bool>> DeleteAsync(string id);
        Task<APIOperationResponse<List<UserRoleDto>>> GetUserRolesAsync(string userId);
        Task<APIOperationResponse<bool>> UpdateUserRolesAsync(string userId, UpdateUserRolesDto dto);
        Task<APIOperationResponse<List<UserDto>>> GetByRoleIdsAsync(IEnumerable<string> roleIds);
        Task<APIOperationResponse<List<UserDto>>> GetSuperAdminsAsync();
        Task<APIOperationResponse<UserDto>> GetCurrentUserAsync();
        Task<APIOperationResponse<bool>> ToggleUserStatusAsync(string id);
        Task<APIOperationResponse<bool>> ChangePasswordAsync(ChangePasswordDto dto);
        Task<APIOperationResponse<UserSummaryDto>> GetUsersSummaryAsync();
    }
}
