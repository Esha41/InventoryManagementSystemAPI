using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO.AuthenticationDto;

namespace Ettad.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<APIOperationResponse<object>> AddAdminAsync(RegisterUserDto addAdminDto);
        Task<APIOperationResponse<object>> AddCashierAsync(RegisterUserDto addCashierDto);
        Task<APIOperationResponse<List<GetUserDto>>> GetAllAdminAsync();
        Task<APIOperationResponse<List<GetUserDto>>> GetAllCashierAsync();
        Task<APIOperationResponse<object>> ChangePasswordAsync(ChangePasswordDto changePasswordDto, string userId);
        Task<APIOperationResponse<LoginResponse>> Login(LoginRequest request);
        Task<APIOperationResponse<GetUserDto>> GetUserByIdAsync(string userId);

        Task<APIOperationResponse<object>> UpdateUserAsync(GetUserDto updateUserDto);
    }
}
