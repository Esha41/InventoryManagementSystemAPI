using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Data.Repository;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
  //  private readonly CrossCuttingRepository<EmployeeContact> _employeeRepository;
    private readonly ICurrentUserService _currentUserService ;
    public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
         , ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
       // _employeeRepository = employeeRepository;
        _currentUserService = currentUserService;
    }

    public async Task<APIOperationResponse<UserDto>> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return APIOperationResponse<UserDto>.Fail(ResponseType.NotFound, "User not found");

        return APIOperationResponse<UserDto>.Success(MapToDto(user));
    }

    public async Task<APIOperationResponse<List<UserDto>>> GetAllAsync()
    {
        var orgId = _currentUserService.OrganizationId;

        // ✅ Pull matching users (allow null org if needed)
        var users = await _userManager.Users
                       .ToListAsync();

        var mapped = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = MapToDto(user);

            // ✅ Get role names
            var roleNames = await _userManager.GetRolesAsync(user);

            // ✅ Convert to role IDs
            var roleIds = await _roleManager.Roles
                .Where(r => roleNames.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync();

            dto.RoleIds = roleIds;
            mapped.Add(dto);
        }

        return APIOperationResponse<List<UserDto>>.Success(mapped);
    }


    public async Task<APIOperationResponse<UserDto>> CreateAsync(CreateUserDto dto)
    {
        ApplicationUser user;

        if (!dto.IsLdapUser)
        {
            user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.UserName,
                IsLdapUser = dto.IsLdapUser,
                ExtraEmployeesView = dto.ExtraEmployeesView,
                EmployeeId = dto.EmployeeId,
                OrganizationId = dto.OrganizationId ?? _currentUserService.OrganizationId
            };
        }
        else
        {
            user = new ApplicationUser
            {
                UserName = "k",
                Email = "ii",
                IsLdapUser = dto.IsLdapUser,
                ExtraEmployeesView = dto.ExtraEmployeesView,
                EmployeeId = dto.EmployeeId,
                OrganizationId = dto.OrganizationId ?? _currentUserService.OrganizationId
            };
        }

        // 1️⃣ Create the user
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return APIOperationResponse<UserDto>.Fail(
                Ettad.ResponseHandler.Consts.ResponseType.BadRequest,
                string.Join(",", result.Errors.Select(e => e.Description))
            );

        // 2️⃣ Assign roles to the user
        if (dto.RoleIds != null && dto.RoleIds.Any())
        {
            foreach (var roleId in dto.RoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role != null)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!roleResult.Succeeded)
                        return APIOperationResponse<UserDto>.Fail(
                            Ettad.ResponseHandler.Consts.ResponseType.BadRequest,
                            string.Join(",", roleResult.Errors.Select(e => e.Description))
                        );
                }
            }
        }
               
        var userDto = MapToDto(user);

        return APIOperationResponse<UserDto>.Success(userDto);
    }

    public async Task<APIOperationResponse<UserDto>> UpdateAsync(UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.Id);
        if (user == null)
            return APIOperationResponse<UserDto>.Fail(ResponseType.NotFound, "User not found");
        else 
        {
            user.UserName = dto.UserName == null ? user.Email : dto.UserName;
            user.Email = dto.UserName == null ? user.Email : dto.UserName;
            user.IsLdapUser = dto.IsLdapUser;
            user.ExtraEmployeesView = dto.ExtraEmployeesView;
            user.EmployeeId = dto.EmployeeId;

            user.OrganizationId = dto.OrganizationId;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return APIOperationResponse<UserDto>.Fail(ResponseType.InternalServerError,
                    string.Join(",", result.Errors.Select(e => e.Description)));

            return APIOperationResponse<UserDto>.Success(MapToDto(user));
        }

    }

    public async Task<APIOperationResponse<bool>> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "User not found");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError,
                string.Join(",", result.Errors.Select(e => e.Description)));

        return APIOperationResponse<bool>.Success(true, "User deleted successfully");
    }

    public async Task<APIOperationResponse<List<UserRoleDto>>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return APIOperationResponse<List<UserRoleDto>>.NotFound("User not found.");
        }

        var userRolesList = new List<UserRoleDto>();

        var allRoles = await _roleManager.Roles.ToListAsync();

        foreach (var role in allRoles)
        {
            var userRoleDto = new UserRoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
            };
            userRolesList.Add(userRoleDto);
        }

        return APIOperationResponse<List<UserRoleDto>>.Success(userRolesList);
    }


    public async Task<APIOperationResponse<bool>> UpdateUserRolesAsync(string userId, UpdateUserRolesDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return APIOperationResponse<bool>.NotFound("User not found.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        // Remove all current roles
        var removalResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removalResult.Succeeded)
        {
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, "Failed to remove existing user roles.");
        }

        // Add the new roles
        var additionResult = await _userManager.AddToRolesAsync(user, dto.RoleNames);
        if (!additionResult.Succeeded)
        {
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, "Failed to add new user roles.");
        }

        return APIOperationResponse<bool>.Success(true, "User roles updated successfully.");
    }

    private UserDto MapToDto(ApplicationUser user) =>
        new()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            IsLdapUser = user.IsLdapUser,
            ExtraEmployeesView = user.ExtraEmployeesView,
            EmployeeId = user.EmployeeId,
            OrganizationId = user.OrganizationId
        };
}
