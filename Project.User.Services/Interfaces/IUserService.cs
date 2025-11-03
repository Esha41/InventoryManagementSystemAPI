using Ettad.ResponseHandler.Models;
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
            Task<APIOperationResponse<List<UserDto>>> GetAllAsync();
       // Task<APIOperationResponse<List<UserDto>>> GetAllUserAsync();
        Task<APIOperationResponse<UserDto>> CreateAsync(CreateUserDto dto);
            Task<APIOperationResponse<UserDto>> UpdateAsync(UpdateUserDto dto);
            Task<APIOperationResponse<bool>> DeleteAsync(string id);
        Task<APIOperationResponse<List<UserRoleDto>>> GetUserRolesAsync(string userId);
        Task<APIOperationResponse<bool>> UpdateUserRolesAsync(string userId, UpdateUserRolesDto dto);
    }

    

}
