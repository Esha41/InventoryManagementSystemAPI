using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Comman.Models.Identity;
using Ettad.Data.Entities;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.EntityFramework.Utiliies;
using Ettad.Infrastructure.Utilities;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.User.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;        
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMemoryCache _cache;
        private readonly IDateTimeProvider _dateTimeProvider;

        // Protected role names that cannot be updated or deleted (used in business logic)
        private static readonly HashSet<string> ProtectedRoleNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Requesting Entity Commander (Order Requesting Entity)",
            "Supply Officer (Order Requesting Entity)",
            "Head of Ammunition Division (Directorate of Armament)",
            "Head of Depo Division (Inventory)"
        };

        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IMapper mapper , ApplicationDbContext context, IMemoryCache cache, IDateTimeProvider dateTimeProvider)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _context = context;
            _cache = cache;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Checks if a role name is protected and cannot be updated or deleted
        /// </summary>
        private bool IsProtectedRole(string roleName)
        {
            return ProtectedRoleNames.Contains(roleName);
        }

        /// <summary>
        /// Checks if the current user is a SuperAdmin using the cached value from ICurrentUserService
        /// </summary>
        private bool IsCurrentUserSuperAdmin => _currentUserService.IsSuperAdmin;

        public async Task<APIOperationResponse<RoleDto>> GetRoleByIdAsync(string id)
        {
            // 1️⃣ Find the role by ID
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return APIOperationResponse<RoleDto>.NotFound($"Role with ID '{id}' not found.");
            }

            // 2️⃣ Map to RoleDto
            var roleDto = _mapper.Map<RoleDto>(role);

            // 3️⃣ Get assigned application entity IDs
            var entityIds = await _context.RoleApplicationEntities
                .Where(x => x.RoleId == role.Id)
                .Select(x => x.ApplicationEntityId)
                .ToListAsync();

            roleDto.ApplicationEntityIds = entityIds;

            // 4️⃣ Return success response
            return APIOperationResponse<RoleDto>.Success(roleDto, "Role retrieved successfully.");
        }

        public async Task<APIOperationResponse<List<RoleDto>>> GetAllRolesAsync()
        {
            // Check if current user is SuperAdmin
            var isSuperAdmin = IsCurrentUserSuperAdmin;

            var rolesQuery = _roleManager.Roles.AsQueryable();

            // Filter out SuperAdmin roles if user is not SuperAdmin
            if (!isSuperAdmin)
            {
                rolesQuery = rolesQuery.Where(r => !r.IsSuperAdmin);
            }

            var roles = await rolesQuery
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    NameAr = r.NameAr,
                    IsSuperAdmin = r.IsSuperAdmin
                })
                .ToListAsync();

            return APIOperationResponse<List<RoleDto>>.Success(roles, "Roles retrieved successfully");
        }

        public async Task<APIOperationResponse<PaginatedList<RoleDto>>> GetRolesAsync(PagedListRequest request)
        {
            // Check if current user is SuperAdmin
            var isSuperAdmin = IsCurrentUserSuperAdmin;

            // 1️⃣ Get roles as IQueryable
            var queryableRoles = _roleManager.Roles.AsQueryable();

            // Filter out SuperAdmin roles if user is not SuperAdmin
            if (!isSuperAdmin)
            {
                queryableRoles = queryableRoles.Where(r => !r.IsSuperAdmin);
            }

            // 2️⃣ Project to RoleDto
            var projectedRoles = queryableRoles
                .Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    NameAr = role.NameAr,
                    IsDefaultRole = (bool)role.IsDefaultRole,
                    IsSuperAdmin = role.IsSuperAdmin
                });

            // 3️⃣ Apply pagination
            var paginatedRoles = await PaginatedList<RoleDto>.CreateAsyncForTableBinding(projectedRoles, request);

            // 4️⃣ Fetch assigned ApplicationEntityIds for each role
            var roleIds = paginatedRoles.Items.Select(r => r.Id).ToList();

            var roleEntities = await _context.RoleApplicationEntities
                .Where(x => roleIds.Contains(x.RoleId))
                .GroupBy(x => x.RoleId)
                .ToDictionaryAsync(g => g.Key, g => g.Select(e => e.ApplicationEntityId).ToList());

            foreach (var role in paginatedRoles.Items)
            {
                if (roleEntities.TryGetValue(role.Id, out var entityIds))
                {
                    role.ApplicationEntityIds = entityIds;
                }
            }

            // 5️⃣ Return paginated response
            return APIOperationResponse<PaginatedList<RoleDto>>.Success(paginatedRoles);
        }


        public async Task<APIOperationResponse<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            // Security: Only SuperAdmins can create SuperAdmin roles
            if (createRoleDto.IsSuperAdmin == true)
            {
                var isSuperAdmin = IsCurrentUserSuperAdmin;
                if (!isSuperAdmin)
                {
                    return APIOperationResponse<RoleDto>.BadRequest("Only SuperAdmins can create SuperAdmin roles.");
                }
            }

            // 1️⃣ Check if role exists
            if (await _roleManager.RoleExistsAsync(createRoleDto.Name))
            {
                return APIOperationResponse<RoleDto>.BadRequest($"Role with name '{createRoleDto.Name}' already exists.");
            }

            // 2️⃣ Map DTO to ApplicationRole
            var newRole = _mapper.Map<ApplicationRole>(createRoleDto);

            // 3️⃣ Create role in Identity
            IdentityResult result = await _roleManager.CreateAsync(newRole);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIOperationResponse<RoleDto>.BadRequest("Failed to create role.", errors);
            }

            // 4️⃣ Map created role to RoleDto
            var roleDto = _mapper.Map<RoleDto>(newRole);

            // 5️⃣ Assign selected application entities, if any
            if (createRoleDto.ApplicationEntityIds != null && createRoleDto.ApplicationEntityIds.Any())
            {
                foreach (var entityId in createRoleDto.ApplicationEntityIds)
                {
                    // Avoid duplicates
                    var exists = await _context.RoleApplicationEntities
                        .AnyAsync(x => x.RoleId == newRole.Id && x.ApplicationEntityId == entityId);

                    if (!exists)
                    {
                        _context.RoleApplicationEntities.Add(new RoleApplicationEntity
                        {
                            RoleId = newRole.Id,
                            ApplicationEntityId = entityId,
                            CreatedBy = "system", // replace with current user if available
                            CreationDate = _dateTimeProvider.Now
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }

            // 6️⃣ Return success
            return APIOperationResponse<RoleDto>.Success(roleDto, "Role created successfully.");
        }


        public async Task<APIOperationResponse<RoleDto>> UpdateRoleAsync(string id, UpdateRoleDto updateRoleDto)
        {
            // 1️⃣ Find the role
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return APIOperationResponse<RoleDto>.NotFound($"Role with ID '{id}' not found.");
            }

            // Security: Only SuperAdmins can update SuperAdmin roles
            if (role.IsSuperAdmin)
            {
                var isSuperAdmin = IsCurrentUserSuperAdmin;
                if (!isSuperAdmin)
                {
                    return APIOperationResponse<RoleDto>.BadRequest("Only SuperAdmins can update SuperAdmin roles.");
                }
            }

            // 1.5️⃣ Check if role is protected (cannot be updated)
            if (IsProtectedRole(role.Name))
            {
                return APIOperationResponse<RoleDto>.BadRequest($"The role '{role.Name}' is protected and cannot be updated. This role is used in critical business logic.");
            }

            // 2️⃣ Check if the new name is already taken by another role
            var existingRoleWithSameName = await _roleManager.FindByNameAsync(updateRoleDto.Name);
            if (existingRoleWithSameName != null && existingRoleWithSameName.Id != id)
            {
                return APIOperationResponse<RoleDto>.BadRequest($"Role name '{updateRoleDto.Name}' is already taken.");
            }

            // 3️⃣ Update role properties
            role.Name = updateRoleDto.Name;
            role.NameAr = updateRoleDto.NameAr;
            role.IsDefaultRole = updateRoleDto.IsDefaultRole;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIOperationResponse<RoleDto>.BadRequest("Failed to update role.", errors);
            }

            // 4️⃣ Update mapped application entities if provided
            if (updateRoleDto.ApplicationEntityIds != null)
            {
                // Remove existing mappings
                var existingMappings = _context.RoleApplicationEntities
                    .Where(x => x.RoleId == role.Id);
                _context.RoleApplicationEntities.RemoveRange(existingMappings);

                // Add new mappings
                foreach (var entityId in updateRoleDto.ApplicationEntityIds)
                {
                    _context.RoleApplicationEntities.Add(new RoleApplicationEntity
                    {
                        RoleId = role.Id,
                        ApplicationEntityId = entityId,
                        CreatedBy = "system", // replace with current user if available
                        CreationDate = DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();
            }

            // 5️⃣ Map updated role to DTO and return
            var updatedRoleDto = _mapper.Map<RoleDto>(role);
            return APIOperationResponse<RoleDto>.Success(updatedRoleDto, "Role updated successfully.");
        }


        public async Task<APIOperationResponse<string>> DeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return APIOperationResponse<string>.NotFound($"Role with ID '{id}' not found.");
            }

            // Security: Only SuperAdmins can delete SuperAdmin roles
            if (role.IsSuperAdmin)
            {
                var isSuperAdmin = IsCurrentUserSuperAdmin;
                if (!isSuperAdmin)
                {
                    return APIOperationResponse<string>.BadRequest("Only SuperAdmins can delete SuperAdmin roles.");
                }
            }

            // Check if role is protected (cannot be deleted)
            if (IsProtectedRole(role.Name))
            {
                return APIOperationResponse<string>.BadRequest($"The role '{role.Name}' is protected and cannot be deleted. This role is used in critical business logic.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIOperationResponse<string>.BadRequest("Failed to delete role.", errors);
            }

            return APIOperationResponse<string>.Deleted("Role deleted successfully.");
        }

        public async Task<APIOperationResponse<List<CrudPermissions>>> GetPlainPermissionsForRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return APIOperationResponse<List<CrudPermissions>>.NotFound($"Role with ID '{roleId}' not found.");
            }

            try
            {
                var roleClaims = (await _roleManager.GetClaimsAsync(role)).Select(c => c.Value).ToList();
                var allClaims = PlainPermissionsGenerator.GetPlainPermissionsWithGroup();

                foreach (var permissions in allClaims)
                {
                    foreach (var permission in permissions.PermissionsList)
                    {
                        if (roleClaims.Any(c => c.Equals(permission.DisplayValue, StringComparison.OrdinalIgnoreCase)))
                        {
                            permission.IsSelected = true;
                        }
                    }
                }

                return APIOperationResponse<List<CrudPermissions>>.Success(allClaims, "Permissions retrieved successfully.");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<CrudPermissions>>.BadRequest($"Error retrieving permissions: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<CrudPermissions>>> GetCrudPermissionsForRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return APIOperationResponse<List<CrudPermissions>>.NotFound($"Role with ID '{roleId}' not found.");
            }

            try
            {
                var roleClaims = (await _roleManager.GetClaimsAsync(role)).Select(c => c.Value).ToList();
                var allClaims = await new CrudPermissionsGenerator(_context).GenerateAllPermissions();

                foreach (var permissions in allClaims)
                {
                    foreach (var permission in permissions.PermissionsList)
                    {
                        if (roleClaims.Any(c => c.Equals(permission.DisplayValue, StringComparison.OrdinalIgnoreCase)))
                        {
                            permission.IsSelected = true;
                        }
                    }
                }

                return APIOperationResponse<List<CrudPermissions>>.Success(allClaims, "Permissions retrieved successfully.");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<CrudPermissions>>.BadRequest($"Error retrieving permissions: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> AssignPermissionsToRoleAsync(AssignPermissionsDto assignPermissions)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(assignPermissions.EntityId);
                if (role == null)
                {
                    return APIOperationResponse<bool>.NotFound($"Role with ID '{assignPermissions.EntityId}' not found.");
                }

                var oldRoleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in oldRoleClaims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                foreach (var claim in assignPermissions.PermissionsList)
                {
                    if (claim.Contains("ExtraEmployeesViewList-"))
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(
                            "Permissions-ExtraEmployeesView",
                            claim.Replace("ExtraEmployeesViewList-", "")));
                    }
                    else if (claim.Contains("AllowedLeaveClauseList-"))
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(
                            "Permissions-AllowedLeaveClauses",
                            claim.Replace("AllowedLeaveClauseList-", "")));
                    }
                    else if (claim.Contains("AllowedPermissionClauseList-"))
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(
                            "Permissions-AllowedPermissionClauses",
                            claim.Replace("AllowedPermissionClauseList-", "")));
                    }
                    else if (claim.Contains("BasedOnEntityList-"))
                    {
                        var claims = claim.Replace("BasedOnEntityList-", "").Split(",").ToList();
                        await _roleManager.AddClaimAsync(role, new Claim("Permissions-BasedOnEntity", claims[0]));
                        claims.RemoveAt(0);
                        await _roleManager.AddClaimAsync(role, new Claim("Permissions-EnityList", string.Join(",", claims)));
                    }
                    else
                    {
                        await _roleManager.AddClaimAsync(role, new Claim("Permissions", claim));
                    }
                }

                // Invalidate permission cache for all users in this role
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                foreach (var user in usersInRole)
                {
                    _cache.Remove($"user_permissions_{user.Id}");
                }

                return APIOperationResponse<bool>.Success(true, "Permissions assigned successfully.");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.BadRequest($"Error assigning permissions: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<UserInRoleDto>>> GetUsersInRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return APIOperationResponse<List<UserInRoleDto>>.NotFound($"Role with ID '{roleId}' not found.");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            
          
            

            var userDtos = usersInRole.Select(u => new UserInRoleDto
            {
                Id = u.Id,
                UserName = u.UserName
            }).ToList();

            return APIOperationResponse<List<UserInRoleDto>>.Success(userDtos);
        }

        public async Task<APIOperationResponse<bool>> RemoveUsersFromRoleAsync(string roleId, RemoveUsersFromRoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return APIOperationResponse<bool>.NotFound($"Role with ID '{roleId}' not found.");
            }

            var orgId = _currentUserService.OrganizationId;
            var errors = new List<string>();

            foreach (var userId in dto.UserIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    errors.Add($"User with ID '{userId}' not found.");
                    continue;
                }

                

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                    {
                        errors.AddRange(result.Errors.Select(e => $"Failed to remove user '{user.UserName}': {e.Description}"));
                    }
                }
            }

            if (errors.Any())
            {
                return APIOperationResponse<bool>.BadRequest("One or more errors occurred.", errors);
            }

            return APIOperationResponse<bool>.Success(true, "Users removed from role successfully.");
        }
        public async Task<APIOperationResponse<List<ApplicationEntityDto>>> GetAllApplicationEntitiesAsync()
        {
            var entities = await _context.ApplicationEntities
                .OrderBy(e => e.Id) // optional, ensures consistent ordering
                .Select(e => new ApplicationEntityDto
                {
                    Id = e.Id,
                    Code = e.Code,
                    NameAr = e.NameAr,
                    NameEn = e.NameEn,
                    IsDeleted = e.IsDeleted,
                    CreationDate = e.CreationDate,
                    ModificationDate = e.ModificationDate,
                    ModifiedBy = e.ModifiedBy,
                    CreatedBy = e.CreatedBy
                })
                .ToListAsync();

            return APIOperationResponse<List<ApplicationEntityDto>>.Success(entities, "Entities retrieved successfully");
        }
        public async Task<List<RoleApplicationEntityDto>> GetApplicationEntitiesByRoleAsync(string roleId)
        {
            return await _context.RoleApplicationEntities
                .Where(r => r.RoleId == roleId)
                .Select(r => new RoleApplicationEntityDto
                {
                    RoleId = r.RoleId,
                    ApplicationEntityId = r.ApplicationEntityId
                })
                .ToListAsync();
        }
    }
}
