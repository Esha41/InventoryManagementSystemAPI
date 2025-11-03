using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Comman.Models.Identity;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.EntityFramework.Utiliies;
using Ettad.Infrastructure.Utilities;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ettad.User.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;        
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IMapper mapper , ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<APIOperationResponse<RoleDto>> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return APIOperationResponse<RoleDto>.NotFound($"Role with ID '{id}' not found.");
            }
            var roleDto = _mapper.Map<RoleDto>(role);
            return APIOperationResponse<RoleDto>.Success(roleDto);
        }
        public async Task<APIOperationResponse<List<RoleDto>>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            return APIOperationResponse<List<RoleDto>>.Success(roles, "Roles retrieved successfully");
        }


        public async Task<APIOperationResponse<PaginatedList<RoleDto>>> GetRolesAsync(PagedListRequest request)
        {
            var queryableRoles = _roleManager.Roles
                .ProjectTo<RoleDto>(_mapper.ConfigurationProvider);

            var paginatedRoles = await PaginatedList<RoleDto>.CreateAsyncForTableBinding(queryableRoles, request);

            return APIOperationResponse<PaginatedList<RoleDto>>.Success(paginatedRoles);
        }

        public async Task<APIOperationResponse<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            if (await _roleManager.RoleExistsAsync(createRoleDto.Name))
            {
                return APIOperationResponse<RoleDto>.BadRequest($"Role with name '{createRoleDto.Name}' already exists.");
            }

            var newRole = _mapper.Map<ApplicationRole>(createRoleDto);

            IdentityResult result = await _roleManager.CreateAsync(newRole);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIOperationResponse<RoleDto>.BadRequest("Failed to create role.", errors);
            }

            var roleDto = _mapper.Map<RoleDto>(newRole);
            return APIOperationResponse<RoleDto>.Success(roleDto, "Role created successfully.");
        }

        public async Task<APIOperationResponse<RoleDto>> UpdateRoleAsync(string id, UpdateRoleDto updateRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return APIOperationResponse<RoleDto>.NotFound($"Role with ID '{id}' not found.");
            }

            // Check if the new name is already taken by another role
            var existingRoleWithSameName = await _roleManager.FindByNameAsync(updateRoleDto.Name);
            if (existingRoleWithSameName != null && existingRoleWithSameName.Id != id)
            {
                return APIOperationResponse<RoleDto>.BadRequest($"Role name '{updateRoleDto.Name}' is already taken.");
            }

            // Update properties
            role.Name = updateRoleDto.Name;
            role.IsDefaultRole = updateRoleDto.IsDefaultRole;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIOperationResponse<RoleDto>.BadRequest("Failed to update role.", errors);
            }

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
                        if (roleClaims.Any(c => c == permission.DisplayValue))
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
                        if (roleClaims.Any(c => c == permission.DisplayValue))
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

            
            var orgId = _currentUserService.OrganizationId;
            var usersInOrg = usersInRole.Where(u => u.OrganizationId == orgId);

            var userDtos = usersInOrg.Select(u => new UserInRoleDto
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

                if (user.OrganizationId != orgId)
                {
                    errors.Add($"You do not have permission to modify user '{user.UserName}'.");
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

    }
}
