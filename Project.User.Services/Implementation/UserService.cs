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
using System.Linq;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models;
using Ettad.LdapSettings.Services.Interfaces;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
  //  private readonly CrossCuttingRepository<EmployeeContact> _employeeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UserService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILdapSettingsService _ldapSettingsService;
    
    
    private readonly ApplicationDbContext _context;
    public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
         , ICurrentUserService currentUserService, ILogger<UserService> logger, ApplicationDbContext context,
         IDateTimeProvider dateTimeProvider, ILdapSettingsService ldapSettingsService)
       
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
       // _employeeRepository = employeeRepository;
        _currentUserService = currentUserService;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _ldapSettingsService = ldapSettingsService;
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

    private IQueryable<ApplicationUser> GetUsersQuery(FilterData filter)
    {
        var query = _userManager.Users
            .Include(u => u.Department)
            .Include(u => u.Rank)
            .AsNoTracking();

        // Check if filter requests deleted users
        bool includeDeleted = false;
        if (filter != null)
        {
            // Check if IsDeleted filter is present and set to true
            if (filter.Field == "IsDeleted" && filter.Value?.ToLower() == "true")
            {
                includeDeleted = true;
            }
            else if (filter.Filters != null && filter.Filters.Any())
            {
                // Check nested filters for IsDeleted = true
                includeDeleted = filter.Filters.Any(f => 
                    f.Field == "IsDeleted" && f.Value?.ToLower() == "true");
            }
        }

        // Only filter out deleted users if not explicitly requested
        if (!includeDeleted)
        {
            query = query.Where(u => !u.IsDeleted);
        }

        // Filter out superadmin users if the requesting user is not a superadmin
        if (!_currentUserService.IsSuperAdmin)
        {
            var superAdminRoleIds = _context.Roles
                .Where(r => r.IsSuperAdmin)
                .Select(r => r.Id);

            var userRoles = _context.UserRoles; // Provided by IdentityDbContext

            query = query.Where(u => !userRoles.Any(ur => ur.UserId == u.Id && superAdminRoleIds.Contains(ur.RoleId)));
        }

        if (filter != null)
        {
            if (!string.IsNullOrEmpty(filter.Value) && string.IsNullOrEmpty(filter.Field) && (filter.Filters == null || !filter.Filters.Any()))
            {
                // Global search across multiple fields
                var searchTerm = filter.Value.ToLower();
                query = query.Where(u => 
                    (u.UserName != null && u.UserName.Contains(searchTerm)) || 
                    (u.Email != null && u.Email.Contains(searchTerm)) || 
                    (u.FullNameEN != null && u.FullNameEN.Contains(searchTerm)) || 
                    (u.FullNameAR != null && u.FullNameAR.Contains(searchTerm)) || 
                    (u.MilitoryId != null && u.MilitoryId.Contains(searchTerm)));
            }
            else
            {
                // Use standard FilterProvider for specific field filters
                query = Ettad.CrossCutting.Comman.Providers.FilterProvider.ToFilterView(query, filter);
            }
        }

        return query;
    }

    public async Task<APIOperationResponse<PaginatedList<UserDto>>> GetAllAsync(PagedListRequest request)
    {
        _logger.LogInformation("Getting users (Paginated). RequestedBy: {RequestedBy}, IsSuperAdmin: {IsSuperAdmin}, Page: {Page}, PageSize: {PageSize}",
            _currentUserService.UserId, _currentUserService.IsSuperAdmin, request.Page, request.PageSize);

        var query = GetUsersQuery(request.Filter);

        // Apply Pagination & Filtering (Using the utility we updated)
        var paginatedUsers = await PaginatedList<ApplicationUser>.CreateAsyncForTableBinding(query, request);

        // Map to DTOs and Batch Load Roles
        var userDtos = await MapToDtosWithRolesAsync(paginatedUsers.Items);

        var result = new PaginatedList<UserDto>(userDtos, paginatedUsers.TotalCount, paginatedUsers.PageIndex, request.PageSize);
      
        return APIOperationResponse<PaginatedList<UserDto>>.Success(result);
    }

    public async Task<APIOperationResponse<List<UserDto>>> GetAllForExportAsync(FilterData filter)
    {
        _logger.LogInformation("Getting all users for export. RequestedBy: {RequestedBy}, IsSuperAdmin: {IsSuperAdmin}",
            _currentUserService.UserId, _currentUserService.IsSuperAdmin);

        var query = GetUsersQuery(filter);

        // Apply filtering but NO pagination
        if (filter != null)
        {
            query = Ettad.CrossCutting.Comman.Providers.FilterProvider.ToFilterView(query, filter);
        }

        var users = await query.ToListAsync();
        var userDtos = await MapToDtosWithRolesAsync(users);

        return APIOperationResponse<List<UserDto>>.Success(userDtos);
    }

    private async Task<List<UserDto>> MapToDtosWithRolesAsync(List<ApplicationUser> users)
    {
        if (!users.Any()) return new List<UserDto>();

        var userIds = users.Select(u => u.Id).ToList();
        
        // Batch fetch all roles for these users in one query
        var userRolesMapping = await _context.UserRoles
            .Where(ur => userIds.Contains(ur.UserId))
            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, Role = r })
            .ToListAsync();

        var allRoles = await _roleManager.Roles.AsNoTracking().ToListAsync();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var dto = MapToDto(user);
            var roleNames = userRolesMapping
                .Where(m => m.UserId == user.Id)
                .Select(m => m.Role.Name!)
                .ToList();
            
            PopulateRoles(dto, roleNames, allRoles);
            userDtos.Add(dto);
        }

        return userDtos;
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

            // Validate LDAP domain
            var ldapSettingsResponse = await _ldapSettingsService.GetLdapSettings();
            if (!ldapSettingsResponse.Succeeded || ldapSettingsResponse.Data == null)
            {
                _logger.LogWarning("User creation failed: Unable to retrieve LDAP settings. CreatedBy: {CreatedBy}", 
                    _currentUserService.UserId);
                return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, "Unable to retrieve LDAP settings");
            }

            var ldapSettings = ldapSettingsResponse.Data;
            var ldapDomain = ldapSettings.LdapDomain?.Trim() ?? string.Empty;

            // Check if the username contains a domain (either DOMAIN\username or username@domain format)
            if (username.Contains("\\"))
            {
                // Format: DOMAIN\username
                var parts = username.Split('\\', 2);
                var providedDomain = parts[0].Trim();
                var usernameWithoutDomain = parts[1].Trim();

                if (!providedDomain.Equals(ldapDomain, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("User creation failed: Domain mismatch. Provided: {ProvidedDomain}, Expected: {ExpectedDomain}. CreatedBy: {CreatedBy}", 
                        providedDomain, ldapDomain, _currentUserService.UserId);
                    return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, 
                        $"Invalid domain. Please use the correct domain: {ldapDomain}");
                }

                // Use the username as-is since domain is correct
                username = username.Trim();
                dto.LdapUserName = username;
            }
            else if (username.Contains("@"))
            {
                // Format: username@domain
                var parts = username.Split('@', 2);
                var usernameWithoutDomain = parts[0].Trim();
                var providedDomain = parts[1].Trim();

                if (!providedDomain.Equals(ldapDomain, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("User creation failed: Domain mismatch. Provided: {ProvidedDomain}, Expected: {ExpectedDomain}. CreatedBy: {CreatedBy}", 
                        providedDomain, ldapDomain, _currentUserService.UserId);
                    return APIOperationResponse<UserDto>.Fail(ResponseType.BadRequest, 
                        $"Invalid domain. Please use the correct domain: {ldapDomain}");
                }

                // Use the username as-is since domain is correct
                username = username.Trim();
                dto.LdapUserName = username;
            }
            else
            {
                // No domain provided, append the domain from DB
                if (!string.IsNullOrWhiteSpace(ldapDomain))
                {
                    username = $"{username.Trim()}@{ldapDomain}";
                    dto.LdapUserName = username;
                    _logger.LogInformation("LDAP username auto-completed with domain. Username: {Username}, Domain: {Domain}", 
                        username, ldapDomain);
                }
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
    
    // Check if target user is a superadmin - superadmins cannot be deleted
    var userRoles = await _userManager.GetRolesAsync(user);
    var allRoles = await _roleManager.Roles.ToListAsync();
    var hasSuperAdminRole = userRoles.Any(roleName => 
        allRoles.Any(r => r.Name == roleName && r.IsSuperAdmin));
        
    if (hasSuperAdminRole)
    {
        _logger.LogWarning("User deletion denied: Cannot delete superadmin user. TargetUserId: {TargetUserId}, DeletedBy: {DeletedBy}", 
            id, _currentUserService.UserId);
        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, 
            "Superadmin users cannot be deleted");
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

    public async Task<APIOperationResponse<bool>> RestoreAsync(string id)
    {
        _logger.LogInformation("Restoring deleted user. TargetUserId: {TargetUserId}, RestoredBy: {RestoredBy}", 
            id, _currentUserService.UserId);
        
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted);
        
        if (user == null)
        {
            _logger.LogWarning("User restore failed: User not found or not deleted. TargetUserId: {TargetUserId}, RestoredBy: {RestoredBy}", 
                id, _currentUserService.UserId);
            return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Deleted user not found");
        }
        
        var username = user.UserName;

        // Restore: Clear deletion flags
        user.IsDeleted = false;
        user.DeletionDate = null;
        user.DeletedBy = null;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User restore failed: {Errors}. TargetUserId: {TargetUserId}, Username: {Username}, RestoredBy: {RestoredBy}", 
                errors, id, username, _currentUserService.UserId);
            return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, errors);
        }

        _logger.LogInformation("User restored successfully. TargetUserId: {TargetUserId}, Username: {Username}, RestoredBy: {RestoredBy}", 
            id, username, _currentUserService.UserId);
        return APIOperationResponse<bool>.Success(true, "User restored successfully");
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

    // Prevent toggling superadmin status - superadmins cannot be deactivated
    var userRoles = await _userManager.GetRolesAsync(user);
    var allRoles = await _roleManager.Roles.ToListAsync();
    var hasSuperAdminRole = userRoles.Any(roleName => 
        allRoles.Any(r => r.Name == roleName && r.IsSuperAdmin));
        
    if (hasSuperAdminRole)
    {
        _logger.LogWarning("User status toggle denied: Cannot toggle superadmin user status. TargetUserId: {TargetUserId}, RequestedBy: {RequestedBy}", 
            id, _currentUserService.UserId);
        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, 
            "Superadmin users cannot be deactivated");
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

    public async Task<APIOperationResponse<UserSummaryDto>> GetUsersSummaryAsync()
    {
        _logger.LogInformation("Getting users summary. RequestedBy: {RequestedBy}", _currentUserService.UserId);

        var query = _userManager.Users.Where(u => !u.IsDeleted);

        // Filter out superadmin users if the requesting user is not a superadmin
        if (!_currentUserService.IsSuperAdmin)
        {
            var superAdminRoleIds = _context.Roles
                .Where(r => r.IsSuperAdmin)
                .Select(r => r.Id);

            var userRoles = _context.UserRoles;
            query = query.Where(u => !userRoles.Any(ur => ur.UserId == u.Id && superAdminRoleIds.Contains(ur.RoleId)));
        }

        var stats = await query
            .GroupBy(u => 1)
            .Select(g => new UserSummaryDto
            {
                TotalUsers = g.Count(),
                ActiveUsers = g.Count(u => u.IsActive),
                InactiveUsers = g.Count(u => !u.IsActive)
            })
            .FirstOrDefaultAsync();

        return APIOperationResponse<UserSummaryDto>.Success(stats ?? new UserSummaryDto());
    }
}
