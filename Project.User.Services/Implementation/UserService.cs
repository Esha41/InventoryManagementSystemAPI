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

    //public async Task<APIOperationResponse<List<UserDto>>> GetAllAsync() 
    //{ 
    //    var orgId = _currentUserService.OrganizationId; 
    //    var users = _userManager.Users.Where(a => a.OrganizationId == orgId).ToList(); 
    //    var mapped = users.Select(MapToDto).ToList(); 
    //    return APIOperationResponse<List<UserDto>>.Success(mapped); 
    //}

    public async Task<APIOperationResponse<List<UserDto>>> GetAllAsync()
    {
        // 1️⃣ Get all users (no org filter)
        var users = await _userManager.Users.ToListAsync();

        // 2️⃣ Get all roles upfront to avoid repeated DB calls
        var allRoles = await _roleManager.Roles.ToListAsync();

        var mapped = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = MapToDto(user);

            // 3️⃣ Get user roles (names)
            var roleNames = await _userManager.GetRolesAsync(user);

            // 4️⃣ Map role names → role IDs using pre-fetched roles
            dto.RoleIds = allRoles
                .Where(r => roleNames.Contains(r.Name))
                .Select(r => r.Id)
                .ToList();

            mapped.Add(dto);
        }

        return APIOperationResponse<List<UserDto>>.Success(mapped);
    }


    public async Task<APIOperationResponse<UserDto>> CreateAsync(CreateUserDto dto)
    {
        if (dto == null)
            return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, "Invalid request");

        // 1️⃣ Create the user object
        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.UserName,
            IsLdapUser = dto.IsLdapUser,
            ExtraEmployeesView = dto.ExtraEmployeesView,
            EmployeeId = dto.EmployeeId,
            OrganizationId = dto.OrganizationId ?? _currentUserService.OrganizationId
        };

        // 2️⃣ Create user in DB
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return APIOperationResponse<UserDto>.Fail(
                ResponseType.BadRequest,
                string.Join(",", result.Errors.Select(e => e.Description))
            );

        // 3️⃣ Assign roles (if any)
        if (dto.RoleIds != null && dto.RoleIds.Any())
        {
            var roleNames = await _roleManager.Roles
                .Where(r => dto.RoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            if (roleNames.Any())
            {
                var addRolesResult = await _userManager.AddToRolesAsync(user, roleNames);
                if (!addRolesResult.Succeeded)
                {
                    return APIOperationResponse<UserDto>.Fail(
                        ResponseType.BadRequest,
                        string.Join(",", addRolesResult.Errors.Select(e => e.Description))
                    );
                }
            }
        }

        // 4️⃣ Return created user
        var userDto = MapToDto(user);
        return APIOperationResponse<UserDto>.Success(userDto);
    }


    public async Task<APIOperationResponse<UserDto>> UpdateAsync(UpdateUserDto dto)
    {
        if (dto == null)
            return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, "Invalid request");

        // 1️⃣ Find user
        var user = await _userManager.FindByIdAsync(dto.Id);
        if (user == null)
            return APIOperationResponse<UserDto>.Fail(ResponseType.NotFound, "User not found");

        // 2️⃣ Update basic fields
        user.UserName = dto.UserName ?? user.UserName;
        user.Email = dto.UserName ?? user.Email;
        user.IsLdapUser = dto.IsLdapUser;
        user.ExtraEmployeesView = dto.ExtraEmployeesView;
        user.EmployeeId = dto.EmployeeId;
        user.OrganizationId = dto.OrganizationId;

        // 3️⃣ Update roles
        if (dto.RoleIds != null)
        {
            // Get current roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Map RoleIds to Role Names
            var newRoleNames = await _roleManager.Roles
                .Where(r => dto.RoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            // Remove roles that are no longer assigned
            var rolesToRemove = currentRoles.Except(newRoleNames).ToList();
            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                    return APIOperationResponse<UserDto>.Fail(
                        ResponseType.InternalServerError,
                        string.Join(",", removeResult.Errors.Select(e => e.Description))
                    );
            }

            // Add new roles
            var rolesToAdd = newRoleNames.Except(currentRoles).ToList();
            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                    return APIOperationResponse<UserDto>.Fail(
                        ResponseType.InternalServerError,
                        string.Join(",", addResult.Errors.Select(e => e.Description))
                    );
            }
        }

        // 4️⃣ Update user
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return APIOperationResponse<UserDto>.Fail(
                ResponseType.InternalServerError,
                string.Join(",", updateResult.Errors.Select(e => e.Description))
            );

        // 5️⃣ Return updated user
        return APIOperationResponse<UserDto>.Success(MapToDto(user));
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

    //public async Task<APIOperationResponse<List<UserRoleDto>>> GetUserRolesAsync(string userId)
    //{
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null)
    //    {
    //        return APIOperationResponse<List<UserRoleDto>>.NotFound("User not found.");
    //    }

    //    var userRolesList = new List<UserRoleDto>();

    //    var allRoles = await _roleManager.Roles.ToListAsync();

    //    foreach (var role in allRoles)
    //    {
    //        var userRoleDto = new UserRoleDto
    //        {
    //            RoleId = role.Id,
    //            RoleName = role.Name,
    //            IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
    //        };
    //        userRolesList.Add(userRoleDto);
    //    }

    //    return APIOperationResponse<List<UserRoleDto>>.Success(userRolesList);
    //}
    public async Task<APIOperationResponse<List<UserRoleDto>>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return APIOperationResponse<List<UserRoleDto>>.NotFound("User not found.");

        var allRoles = await _roleManager.Roles.ToListAsync();
        var userRoleNames = await _userManager.GetRolesAsync(user);

        var userRolesList = allRoles.Select(role => new UserRoleDto
        {
            RoleId = role.Id,
            RoleName = role.Name,
            IsSelected = userRoleNames.Contains(role.Name)
        }).ToList();

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

    public async Task<APIOperationResponse<List<UserDto>>> GetByRoleIdsAsync(IEnumerable<string> roleIds)
    {
        if (roleIds == null)
        {
            return APIOperationResponse<List<UserDto>>.Success(new List<UserDto>());
        }

        var distinctRoleIds = roleIds.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList();
        if (!distinctRoleIds.Any())
        {
            return APIOperationResponse<List<UserDto>>.Success(new List<UserDto>());
        }

        var roleNames = await _roleManager.Roles
            .Where(r => distinctRoleIds.Contains(r.Id))
            .Select(r => r.Name)
            .ToListAsync();

        if (!roleNames.Any())
        {
            return APIOperationResponse<List<UserDto>>.Success(new List<UserDto>());
        }

        var usersMap = new Dictionary<string, ApplicationUser>();

        foreach (var roleName in roleNames.Where(name => !string.IsNullOrWhiteSpace(name)))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            foreach (var user in usersInRole)
            {
                usersMap[user.Id] = user;
            }
        }

        var users = usersMap.Values.Select(MapToDto).ToList();
        return APIOperationResponse<List<UserDto>>.Success(users);
    }

    public async Task<APIOperationResponse<List<UserDto>>> GetSuperAdminsAsync()
    {
        var superAdmins = await _userManager.Users
            .Where(u => u.IsSuperAdmin)
            .ToListAsync();

        var result = superAdmins.Select(MapToDto).ToList();
        return APIOperationResponse<List<UserDto>>.Success(result);
    }

    private UserDto MapToDto(ApplicationUser user) =>
        new()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            IsLdapUser = user.IsLdapUser,
            IsSuperAdmin = user.IsSuperAdmin,
            ExtraEmployeesView = user.ExtraEmployeesView,
            EmployeeId = user.EmployeeId,
            OrganizationId = user.OrganizationId
        };
}
