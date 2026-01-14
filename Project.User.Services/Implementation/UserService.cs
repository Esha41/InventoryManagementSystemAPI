using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Module.lookup.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Services.DataTransferObject.AuthenticationDto;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Ettad.CrossCutting.Comman.Time;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
  //  private readonly CrossCuttingRepository<EmployeeContact> _employeeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UserService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    
    private readonly ApplicationDbContext _context;
    public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
         , ICurrentUserService currentUserService, ILogger<UserService> logger, ApplicationDbContext context,
         IDateTimeProvider dateTimeProvider)
       
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
       // _employeeRepository = employeeRepository;
        _currentUserService = currentUserService;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<APIOperationResponse<UserDto>> GetByIdAsync(string id)
    {
        _logger.LogInformation("Getting user by ID. TargetUserId: {TargetUserId}, RequestedBy: {RequestedBy}", 
            id, _currentUserService.UserId);
        
        var user = await _userManager.Users
            .Include(u => u.Department)
            .Include(u => u.Rank)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        if (user == null)
        {
            _logger.LogWarning("User not found. TargetUserId: {TargetUserId}, RequestedBy: {RequestedBy}", 
                id, _currentUserService.UserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.NotFound, "User not found");
        }

        var dto = MapToDto(user);
        await PopulateRolesAsync(dto, user);

        _logger.LogInformation("User retrieved successfully. TargetUserId: {TargetUserId}, Username: {Username}", 
            id, user.UserName);
        return APIOperationResponse<UserDto>.Success(dto);
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
        _logger.LogInformation("Getting all users. RequestedBy: {RequestedBy}, IsSuperAdmin: {IsSuperAdmin}", 
            _currentUserService.UserId, _currentUserService.IsSuperAdmin);
        

        var users = await _userManager.Users
            .Where(u => !u.IsDeleted)
            .Include(u => u.Department)
            .Include(u => u.Rank)
            .ToListAsync();

        // 2️⃣ Get all roles upfront to avoid repeated DB calls
        var allRoles = await _roleManager.Roles.ToListAsync();
        
        // Get superadmin role IDs
        var superAdminRoleIds = allRoles.Where(r => r.IsSuperAdmin).Select(r => r.Id).ToHashSet();
        var superAdminRoleNames = allRoles.Where(r => r.IsSuperAdmin).Select(r => r.Name).ToHashSet();

        var mapped = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = MapToDto(user);

            // 3️⃣ Get user roles (names)
            var roleNames = await _userManager.GetRolesAsync(user);

            // 4️⃣ Map role names → role IDs using pre-fetched roles
            PopulateRoles(dto, roleNames, allRoles);
            
            // 5️⃣ Filter out superadmin users if the requesting user is not a superadmin
            if (!_currentUserService.IsSuperAdmin)
            {
                // Check if this user has any superadmin roles
                var hasSuperAdminRole = roleNames.Any(roleName => superAdminRoleNames.Contains(roleName));
                
                if (hasSuperAdminRole)
                {
                    _logger.LogDebug("Filtering out superadmin user from results. UserId: {UserId}, Username: {Username}", 
                        user.Id, user.UserName);
                    continue; // Skip this user
                }
            }
            
            mapped.Add(dto);
        }

        _logger.LogInformation("Successfully retrieved {UserCount} users (after filtering). RequestedBy: {RequestedBy}", 
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

        // Determine username and password based on user role and LDAP status
        string username;
        string password;
        bool isSuperAdmin = _currentUserService.IsSuperAdmin;

        if (!isSuperAdmin && dto.IsLdapUser)
        {
            // Non-super admin creating LDAP user: use LdapUserName and allow nullable password
            username = dto.LdapUserName ?? dto.UserName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("User creation failed: LdapUserName or UserName is required for LDAP user. CreatedBy: {CreatedBy}", 
                    _currentUserService.UserId);
                return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, "LdapUserName or UserName is required");
            }
            // For LDAP users, if password is null, generate a random password (won't be used for authentication)
            password = dto.Password ?? Guid.NewGuid().ToString() + "!@#$%^&*";
            _logger.LogInformation("Non-super admin creating LDAP user. Using LdapUserName: {LdapUserName}", username);
        }
        else
        {
            // Super admin or non-LDAP user: use standard UserName and require password
            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                _logger.LogWarning("User creation failed: UserName is required. CreatedBy: {CreatedBy}", 
                    _currentUserService.UserId);
                return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, "UserName is required");
            }
            username = dto.UserName;
            password = dto.Password;
            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("User creation failed: Password is required. CreatedBy: {CreatedBy}", 
                    _currentUserService.UserId);
                return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, "Password is required");
            }
        }

        // 1️⃣ Create the user object
        var user = new ApplicationUser
        {
            UserName = username,
            Email = dto.Email,
            IsLdapUser = dto.IsLdapUser,
            ExtraEmployeesView = dto.ExtraEmployeesView,
            DepartmentId = dto.DepartmentId,            
            MilitoryId= dto.MilitoryId,
            RankId = dto.RankId,
            FullNameEN = dto.FullNameEN,
            FullNameAR = dto.FullNameAR,
            LdapUserName = dto.LdapUserName,
            IsActive = dto.IsActive
        };

        // 2️⃣ Create user in DB
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User creation failed: {Errors}. Username: {Username}, CreatedBy: {CreatedBy}", 
                errors, username, _currentUserService.UserId);
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
                    roleNames.Count, username, string.Join(", ", roleNames));
                
                var addRolesResult = await _userManager.AddToRolesAsync(user, roleNames);
                if (!addRolesResult.Succeeded)
                {
                    var errors = string.Join(",", addRolesResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Role assignment failed. Username: {Username}, Errors: {Errors}", 
                        username, errors);
                    return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, errors);
                }
            }
        }

        _logger.LogInformation("User created successfully. UserId: {UserId}, Username: {Username}, CreatedBy: {CreatedBy}", 
            user.Id, user.UserName, _currentUserService.UserId);
        
        // 4️⃣ Return created user
        var userDto = MapToDto(user);
        await PopulateRolesAsync(userDto, user);
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
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == dto.Id && !u.IsDeleted);
        if (user == null)
        {
            _logger.LogWarning("User update failed: User not found. TargetUserId: {TargetUserId}, UpdatedBy: {UpdatedBy}", 
                dto.Id, _currentUserService.UserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.NotFound, "User not found");
        }
        
        // 2️⃣ Check if target user is a superadmin and requesting user is not
        if (!_currentUserService.IsSuperAdmin)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var hasSuperAdminRole = userRoles.Any(roleName => 
                allRoles.Any(r => r.Name == roleName && r.IsSuperAdmin));
                
            if (hasSuperAdminRole)
            {
                _logger.LogWarning("User update denied: Normal admin attempted to update superadmin user. TargetUserId: {TargetUserId}, UpdatedBy: {UpdatedBy}", 
                    dto.Id, _currentUserService.UserId);
                return APIOperationResponse<UserDto>.Fail(ResponseType.Forbidden, 
                    "You do not have permission to update superadmin users");
            }
        }

        var oldUsername = user.UserName;
        
        // 2️⃣ Update basic fields
        user.UserName = dto.UserName;
        user.Email =  dto.Email;
        user.IsLdapUser = dto.IsLdapUser;
        user.ExtraEmployeesView = dto.ExtraEmployeesView;
        user.DepartmentId= dto.DepartmentId;        
        user.MilitoryId = dto.MilitoryId;
        user.RankId = dto.RankId;
        user.FullNameEN = dto.FullNameEN;
        user.FullNameAR = dto.FullNameAR;
        user.LdapUserName= dto.LdapUserName;
        user.IsActive = dto.IsActive;
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
        var updatedDto = MapToDto(user);
        await PopulateRolesAsync(updatedDto, user);
        return APIOperationResponse<UserDto>.Success(updatedDto);
    }



    public async Task<APIOperationResponse<bool>> DeleteAsync(string id)
    {
        _logger.LogInformation("Soft deleting user. TargetUserId: {TargetUserId}, DeletedBy: {DeletedBy}", 
            id, _currentUserService.UserId);
        
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        
        if (user == null)
        {
            _logger.LogWarning("User deletion failed: User not found or already deleted. TargetUserId: {TargetUserId}, DeletedBy: {DeletedBy}", 
                id, _currentUserService.UserId);
            return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "User not found");
        }
        
        // Check if target user is a superadmin and requesting user is not
        if (!_currentUserService.IsSuperAdmin)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var hasSuperAdminRole = userRoles.Any(roleName => 
                allRoles.Any(r => r.Name == roleName && r.IsSuperAdmin));
                
            if (hasSuperAdminRole)
            {
                _logger.LogWarning("User deletion denied: Normal admin attempted to delete superadmin user. TargetUserId: {TargetUserId}, DeletedBy: {DeletedBy}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, 
                    "You do not have permission to delete superadmin users");
            }
        }

        var username = user.UserName;

        // Soft delete: Set IsDeleted flag instead of actually deleting
        user.IsDeleted = true;
        user.DeletionDate = _dateTimeProvider.Now;
        user.DeletedBy = _currentUserService.UserId;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User soft deletion failed: {Errors}. TargetUserId: {TargetUserId}, Username: {Username}, DeletedBy: {DeletedBy}", 
                errors, id, username, _currentUserService.UserId);
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, errors);
        }

        _logger.LogInformation("User soft deleted successfully. TargetUserId: {TargetUserId}, Username: {Username}, DeletedBy: {DeletedBy}", 
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
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
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
        
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
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

        var userIds = usersMap.Keys.ToList();
        if (!userIds.Any())
        {
            return APIOperationResponse<List<UserDto>>.Success(new List<UserDto>());
        }

        var users = await _userManager.Users
            .Where(u => userIds.Contains(u.Id) && !u.IsDeleted)
            .Include(u => u.Department)
            .Include(u => u.Rank)
            .ToListAsync();

        var allRolesList = await _roleManager.Roles.ToListAsync();
        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = MapToDto(user);
            var roleNamesForUser = await _userManager.GetRolesAsync(user);
            PopulateRoles(dto, roleNamesForUser, allRolesList);
            result.Add(dto);
        }

        return APIOperationResponse<List<UserDto>>.Success(result);
    }

    public async Task<APIOperationResponse<List<UserDto>>> GetSuperAdminsAsync()
    {
        var superAdmins = await _userManager.Users
            .Where(u => u.IsSuperAdmin && !u.IsDeleted)
            .Include(u => u.Department)
            .Include(u => u.Rank)
            .ToListAsync();

        var allRolesList = await _roleManager.Roles.ToListAsync();
        var result = new List<UserDto>();

        foreach (var user in superAdmins)
        {
            var dto = MapToDto(user);
            var roleNames = await _userManager.GetRolesAsync(user);
            PopulateRoles(dto, roleNames, allRolesList);
            result.Add(dto);
        }

        return APIOperationResponse<List<UserDto>>.Success(result);
    }

    public async Task<APIOperationResponse<UserDto>> GetCurrentUserAsync()
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            _logger.LogWarning("Failed to retrieve current user info. No authenticated user context available.");
            return APIOperationResponse<UserDto>.Fail(ResponseType.Unauthorized, "Current user context not found.");
        }

        var user = await _userManager.Users
            .Where(u => u.Id == currentUserId && !u.IsDeleted)
            .Include(u => u.Department)
            .Include(u => u.Rank)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            _logger.LogWarning("Current user not found in identity store. UserId: {UserId}", currentUserId);
            return APIOperationResponse<UserDto>.Fail(ResponseType.NotFound, "User not found.");
        }

        var dto = MapToDto(user);
        await PopulateRolesAsync(dto, user);

        _logger.LogInformation("Successfully retrieved current user info. UserId: {UserId}", currentUserId);
        return APIOperationResponse<UserDto>.Success(dto);
    }

    public async Task<APIOperationResponse<bool>> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            _logger.LogWarning("Password change failed: No authenticated user context available.");
            return APIOperationResponse<bool>.Fail(ResponseType.Unauthorized, "User not authenticated.");
        }

        _logger.LogInformation("Attempting to change password. UserId: {UserId}", currentUserId);

        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == currentUserId && !u.IsDeleted);

        if (user == null)
        {
            _logger.LogWarning("Password change failed: User not found. UserId: {UserId}", currentUserId);
            return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "User not found.");
        }

        // Prevent LDAP users from changing password (managed externally)
        if (user.IsLdapUser)
        {
            _logger.LogWarning("Password change denied: LDAP user attempted to change password. UserId: {UserId}, Username: {Username}", 
                currentUserId, user.UserName);
            return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
                "LDAP users cannot change their password through this system. Please contact your system administrator.");
        }

        // Verify old password and change to new password
        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            var errorMessage = string.Join(", ", errors);
            
            _logger.LogWarning("Password change failed: {Errors}. UserId: {UserId}, Username: {Username}", 
                errorMessage, currentUserId, user.UserName);
            
            return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errorMessage);
        }

        _logger.LogInformation("Password changed successfully. UserId: {UserId}, Username: {Username}", 
            currentUserId, user.UserName);

        return APIOperationResponse<bool>.Success(true, "Password changed successfully.");
    }

    private async Task PopulateRolesAsync(UserDto dto, ApplicationUser user)
    {
        var roleNames = await _userManager.GetRolesAsync(user);

        if (roleNames == null || !roleNames.Any())
        {
            dto.Roles = new List<UserRoleSummaryDto>();
            return;
        }

        var roles = await _roleManager.Roles
            .Where(r => r.Name != null && roleNames.Contains(r.Name))
            .Select(r => new { r.Id, r.Name })
            .ToListAsync();

        dto.Roles = roles
            .Select(r => new UserRoleSummaryDto
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            })
            .ToList();
    }

    private static void PopulateRoles(
        UserDto dto,
        IList<string> roleNames,
        List<ApplicationRole> allRoles)
    {
        if (roleNames == null || roleNames.Count == 0)
        {
            dto.Roles = new List<UserRoleSummaryDto>();
            return;
        }

        dto.Roles = allRoles
            .Where(r => r.Name != null && roleNames.Contains(r.Name))
            .Select(r => new UserRoleSummaryDto
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            })
            .ToList();
    }

    public async Task<APIOperationResponse<bool>> ToggleUserStatusAsync(string id)
    {
        _logger.LogInformation("Toggling user status. TargetUserId: {TargetUserId}, RequestedBy: {RequestedBy}", 
            id, _currentUserService.UserId);
        
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        
        if (user == null)
        {
            _logger.LogWarning("User status toggle failed: User not found. TargetUserId: {TargetUserId}, RequestedBy: {RequestedBy}", 
                id, _currentUserService.UserId);
            return APIOperationResponse<bool>.NotFound("User not found.");
        }

        // Prevent toggling superadmin status if current user is not a superadmin
        if (!_currentUserService.IsSuperAdmin)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var hasSuperAdminRole = userRoles.Any(roleName => 
                allRoles.Any(r => r.Name == roleName && r.IsSuperAdmin));
                
            if (hasSuperAdminRole)
            {
                _logger.LogWarning("User status toggle denied: Normal admin attempted to toggle superadmin. TargetUserId: {TargetUserId}, RequestedBy: {RequestedBy}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, 
                    "You do not have permission to disable superadmin users");
            }
        }

        user.IsActive = !user.IsActive;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to toggle user status. UserId: {UserId}, Errors: {Errors}", id, errors);
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, errors);
        }

        _logger.LogInformation("User status toggled successfully. UserId: {UserId}, NewStatus: {IsActive}, RequestedBy: {RequestedBy}", 
            id, user.IsActive, _currentUserService.UserId);
        
        return APIOperationResponse<bool>.Success(true, $"User {(user.IsActive ? "enabled" : "disabled")} successfully");
    }

    private UserDto MapToDto(ApplicationUser user)
    {
        var dto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            IsLdapUser = user.IsLdapUser,
            IsSuperAdmin = user.IsSuperAdmin,
            IsActive = user.IsActive,
            ExtraEmployeesView = user.ExtraEmployeesView ?? string.Empty,
            DeparmentId = user.DepartmentId,
            FullNameEN = user.FullNameEN ?? string.Empty,
            FullNameAR = user.FullNameAR ?? string.Empty,
            RankId = user.RankId,
            MilitoryId = user.MilitoryId,
            LdapUserName = user.LdapUserName ?? string.Empty,
        };

        if (user.Department != null)
        {
            dto.Department = MapDepartmentToDto(user.Department);
        }

        if (user.Rank != null)
        {
            dto.Rank = MapRankToDto(user.Rank);
        }

        return dto;
    }

    private static DepartmentDto MapDepartmentToDto(Department department) =>
        new()
        {
            Id = department.Id,
            Code = department.Code,
            NameAr = department.NameAr,
            NameEn = department.NameEn,
            IsDeleted = department.IsDeleted
        };

    private static RankDto MapRankToDto(Rank rank) =>
        new()
        {
            Id = rank.Id,
            NameAr = rank.NameAr,
            NameEn = rank.NameEn,
            IsDeleted = rank.IsDeleted
        };
}
