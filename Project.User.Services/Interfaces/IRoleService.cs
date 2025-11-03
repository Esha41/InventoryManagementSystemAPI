using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Comman.Models.Identity;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    public interface IRoleService
    {
        Task<APIOperationResponse<PaginatedList<RoleDto>>> GetRolesAsync(PagedListRequest request);
        Task<APIOperationResponse<RoleDto>> GetRoleByIdAsync(string id);
        Task<APIOperationResponse<List<RoleDto>>> GetAllRolesAsync();
        Task<APIOperationResponse<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<APIOperationResponse<RoleDto>> UpdateRoleAsync(string id, UpdateRoleDto updateRoleDto);
        Task<APIOperationResponse<string>> DeleteRoleAsync(string id);
        Task<APIOperationResponse<List<CrudPermissions>>> GetPlainPermissionsForRoleAsync(string roleId);
        Task<APIOperationResponse<List<CrudPermissions>>> GetCrudPermissionsForRole(string roleId);
        Task<APIOperationResponse<bool>> AssignPermissionsToRoleAsync(AssignPermissionsDto assignPermissions);
        Task<APIOperationResponse<List<UserInRoleDto>>> GetUsersInRoleAsync(string roleId);

        Task<APIOperationResponse<bool>> RemoveUsersFromRoleAsync(string roleId, RemoveUsersFromRoleDto dto);
        Task<APIOperationResponse<List<ApplicationEntityDto>>> GetAllApplicationEntitiesAsync();

    }
}
