using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
  //  private readonly CrossCuttingRepository<EmployeeContact> _employeeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UserService> _logger;
    
    
    private readonly ApplicationDbContext _context;
    public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
         , ICurrentUserService currentUserService, ILogger<UserService> logger, ApplicationDbContext context)
       
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
       // _employeeRepository = employeeRepository;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<APIOperationResponse<UserDto>> GetByIdAsync(string id)
    {
        _logger.LogInformation("Getting user by ID. TargetUserId: {TargetUserId}, RequestedBy: {RequestedBy}", 
            id, _currentUserService.UserId);
        
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found. TargetUserId: {TargetUserId}, RequestedBy: {RequestedBy}", 
                id, _currentUserService.UserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.NotFound, "User not found");
        }

        _logger.LogInformation("User retrieved successfully. TargetUserId: {TargetUserId}, Username: {Username}", 
            id, user.UserName);
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
        _logger.LogInformation("Getting all users. RequestedBy: {RequestedBy}", _currentUserService.UserId);
        
        // 1️⃣ Get all users (no org filter)
        var users = await _userManager.Users.ToListAsync();

        // 2️⃣ Get all roles upfront to avoid repeated DB calls
        var allRoles = await _roleManager.Roles.ToListAsync();
        var allDepartments = await _context.Departments.ToListAsync();

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
            dto.DeparmentId = user.DepartmentId;

            dto.DepartmentName = allDepartments
           .FirstOrDefault(d => d.Id == user.DepartmentId)?.NameEn ?? string.Empty;
            dto.FullNameAR = user.FullNameAR;
            dto.FullNameEN = user.FullNameEN;
            dto.RankId = user.RankId;
            dto.MilitoryId= user.MilitoryId;
            mapped.Add(dto);
        }

        _logger.LogInformation("Successfully retrieved {UserCount} users. RequestedBy: {RequestedBy}", 
            mapped.Count, _currentUserService.UserId);
        return APIOperationResponse<List<UserDto>>.Success(mapped);
    }


    public async Task<APIOperationResponse<UserDto>> CreateAsync(CreateUserDto dto)
    {
        _logger.LogInformation("Creating new user. Username: {Username}, IsLdapUser: {IsLdapUser}, CreatedBy: {CreatedBy}", 
            dto?.UserName, dto?.IsLdapUser, _currentUserService.UserId);
        
        if (dto == null)
        {
            _logger.LogWarning("User creation failed: Invalid request. CreatedBy: {CreatedBy}", 
                _currentUserService.UserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, "Invalid request");
        }

        // 1️⃣ Create the user object
        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            IsLdapUser = dto.IsLdapUser,
            ExtraEmployeesView = dto.ExtraEmployeesView,
            EmployeeId = dto.EmployeeId,
            DepartmentId = dto.DepartmentId,            
            MilitoryId= dto.MilitoryId,
            RankId = dto.RankId,
            FullNameEN = dto.FullNameEN,
            FullNameAR = dto.FullNameAR
        };

        // 2️⃣ Create user in DB
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User creation failed: {Errors}. Username: {Username}, CreatedBy: {CreatedBy}", 
                errors, dto.UserName, _currentUserService.UserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, errors);
        }

        // 3️⃣ Assign roles (if any)
        if (dto.RoleIds != null && dto.RoleIds.Any())
        {
            var roleNames = await _roleManager.Roles
                .Where(r => dto.RoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            if (roleNames.Any())
            {
                _logger.LogInformation("Assigning {RoleCount} roles to new user. Username: {Username}, Roles: {Roles}", 
                    roleNames.Count, dto.UserName, string.Join(", ", roleNames));
                
                var addRolesResult = await _userManager.AddToRolesAsync(user, roleNames);
                if (!addRolesResult.Succeeded)
                {
                    var errors = string.Join(",", addRolesResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Role assignment failed. Username: {Username}, Errors: {Errors}", 
                        dto.UserName, errors);
                    return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, errors);
                }
            }
        }

        _logger.LogInformation("User created successfully. UserId: {UserId}, Username: {Username}, CreatedBy: {CreatedBy}", 
            user.Id, user.UserName, _currentUserService.UserId);
        
        // 4️⃣ Return created user
        var userDto = MapToDto(user);
        return APIOperationResponse<UserDto>.Success(userDto);
    }


    public async Task<APIOperationResponse<UserDto>> UpdateAsync(UpdateUserDto dto)
    {
        _logger.LogInformation("Updating user. TargetUserId: {TargetUserId}, Username: {Username}, UpdatedBy: {UpdatedBy}", 
            dto?.Id, dto?.UserName, _currentUserService.UserId);
        
        if (dto == null)
        {
            _logger.LogWarning("User update failed: Invalid request. UpdatedBy: {UpdatedBy}", 
                _currentUserService.UserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, "Invalid request");
        }

        // 1️⃣ Find user
        var user = await _userManager.FindByIdAsync(dto.Id);
        if (user == null)
        {
            _logger.LogWarning("User update failed: User not found. TargetUserId: {TargetUserId}, UpdatedBy: {UpdatedBy}", 
                dto.Id, _currentUserService.UserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.NotFound, "User not found");
        }

        var oldUsername = user.UserName;
        
        // 2️⃣ Update basic fields
        user.UserName = dto.UserName;
        user.Email =  dto.Email;
        user.IsLdapUser = dto.IsLdapUser;
        user.ExtraEmployeesView = dto.ExtraEmployeesView;
        user.EmployeeId = dto.EmployeeId;
        user.DepartmentId= dto.DepartmentId;        
        user.MilitoryId = dto.MilitoryId;
        user.RankId = dto.RankId;
        user.FullNameEN = dto.FullNameEN;
        user.FullNameAR = dto.FullNameAR;

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
                _logger.LogInformation("Removing {RoleCount} roles from user. UserId: {UserId}, Roles: {Roles}", 
                    rolesToRemove.Count, dto.Id, string.Join(", ", rolesToRemove));
                
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(",", removeResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to remove roles. UserId: {UserId}, Errors: {Errors}", 
                        dto.Id, errors);
                    return APIOperationResponse<UserDto>.Fail(ResponseType.InternalServerError, errors);
                }
            }

            // Add new roles
            var rolesToAdd = newRoleNames.Except(currentRoles).ToList();
            if (rolesToAdd.Any())
            {
                _logger.LogInformation("Adding {RoleCount} roles to user. UserId: {UserId}, Roles: {Roles}", 
                    rolesToAdd.Count, dto.Id, string.Join(", ", rolesToAdd));
                
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    var errors = string.Join(",", addResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to add roles. UserId: {UserId}, Errors: {Errors}", 
                        dto.Id, errors);
                    return APIOperationResponse<UserDto>.Fail(ResponseType.InternalServerError, errors);
                }
            }
        }

        // 4️⃣ Update user
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(",", updateResult.Errors.Select(e => e.Description));
            _logger.LogWarning("User update failed: {Errors}. UserId: {UserId}, UpdatedBy: {UpdatedBy}", 
                errors, dto.Id, _currentUserService.UserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.InternalServerError, errors);
        }

        _logger.LogInformation("User updated successfully. UserId: {UserId}, OldUsername: {OldUsername}, NewUsername: {NewUsername}, UpdatedBy: {UpdatedBy}", 
            dto.Id, oldUsername, user.UserName, _currentUserService.UserId);

        // 5️⃣ Return updated user
        return APIOperationResponse<UserDto>.Success(MapToDto(user));
    }



    public async Task<APIOperationResponse<bool>> DeleteAsync(string id)
    {
        _logger.LogInformation("Deleting user. TargetUserId: {TargetUserId}, DeletedBy: {DeletedBy}", 
            id, _currentUserService.UserId);
        
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User deletion failed: User not found. TargetUserId: {TargetUserId}, DeletedBy: {DeletedBy}", 
                id, _currentUserService.UserId);
            return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "User not found");
        }

        var username = user.UserName;
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User deletion failed: {Errors}. TargetUserId: {TargetUserId}, Username: {Username}, DeletedBy: {DeletedBy}", 
                errors, id, username, _currentUserService.UserId);
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, errors);
        }

        _logger.LogInformation("User deleted successfully. TargetUserId: {TargetUserId}, Username: {Username}, DeletedBy: {DeletedBy}", 
            id, username, _currentUserService.UserId);
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
            IsSelected = userRoleNames.Contains(role.Name),
            DeparmentId=user.DepartmentId
        }).ToList();

        return APIOperationResponse<List<UserRoleDto>>.Success(userRolesList);
    }

    public async Task<APIOperationResponse<bool>> UpdateUserRolesAsync(string userId, UpdateUserRolesDto dto)
    {
        _logger.LogInformation("Updating user roles. TargetUserId: {TargetUserId}, NewRoles: {NewRoles}, UpdatedBy: {UpdatedBy}", 
            userId, string.Join(", ", dto?.RoleNames ?? new List<string>()), _currentUserService.UserId);
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User roles update failed: User not found. TargetUserId: {TargetUserId}, UpdatedBy: {UpdatedBy}", 
                userId, _currentUserService.UserId);
            return APIOperationResponse<bool>.NotFound("User not found.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        _logger.LogInformation("Current user roles. TargetUserId: {TargetUserId}, Username: {Username}, CurrentRoles: {CurrentRoles}", 
            userId, user.UserName, string.Join(", ", currentRoles));

        // Remove all current roles
        var removalResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removalResult.Succeeded)
        {
            var errors = string.Join(",", removalResult.Errors.Select(e => e.Description));
            _logger.LogWarning("Failed to remove existing user roles. TargetUserId: {TargetUserId}, Errors: {Errors}", 
                userId, errors);
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, "Failed to remove existing user roles.");
        }

        // Add the new roles
        var additionResult = await _userManager.AddToRolesAsync(user, dto.RoleNames);
        if (!additionResult.Succeeded)
        {
            var errors = string.Join(",", additionResult.Errors.Select(e => e.Description));
            _logger.LogWarning("Failed to add new user roles. TargetUserId: {TargetUserId}, Errors: {Errors}", 
                userId, errors);
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, "Failed to add new user roles.");
        }

        _logger.LogInformation("User roles updated successfully. TargetUserId: {TargetUserId}, Username: {Username}, OldRoles: {OldRoles}, NewRoles: {NewRoles}, UpdatedBy: {UpdatedBy}", 
            userId, user.UserName, string.Join(", ", currentRoles), string.Join(", ", dto.RoleNames), _currentUserService.UserId);
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
            FullNameEN=user.FullNameEN,
            FullNameAR=user.FullNameAR,
            RankId = user.RankId,
            MilitoryId = user.MilitoryId
        };
}
