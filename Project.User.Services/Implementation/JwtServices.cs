using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Ettad.Comman.Idenitity;
    using Ettad.CrossCutting.Comman.Exception;
    using Ettad.CrossCutting.Comman.Idenitity;
    using Ettad.CrossCutting.Comman.Time;
    using Ettad.User.Services.DTO;
    using Ettad.User.Services.Interfaces;
    using StackExchange.Redis;

    public class JwtServices : IJwtServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public JwtServices(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtOptions> jwtOptions,
            IDateTimeProvider dateTimeProvider, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _roleManager = roleManager;

            if (string.IsNullOrWhiteSpace(_jwtOptions.Secret))
                throw new ArgumentException("JWT secret must be configured in JwtOptions.Secret");
        }

        public string GenerateRefreshToken()
        {
            // Cryptographically secure random token
            var randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            // Use Base64Url safe string
            return Convert.ToBase64String(randomBytes);
        }

        public async Task<AuthenticatedResponse> GenerateJWTokenAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("userId required", nameof(userId));

            var user = await _userManager.FindByIdAsync(userId)
                       ?? throw new Exception("server.invalidLogin");

            var now = _dateTimeProvider.UtcNow;
            var expires = now.AddMinutes(_jwtOptions.AccessTokenExpireInMinutes);

            var claims = await BuildUserClaimsAsync(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials

            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(jwt);

            return new AuthenticatedResponse
            {
                AccessToken = tokenString,
                ExpiresAt = expires.ToLocalTime() // optional: present local time to caller
            };
        }

        public async Task<AuthenticatedResponse> GenerateAzureJWTokenAsync(LoginWithAzureInformation information)
        {
            if (information == null || string.IsNullOrWhiteSpace(information.Username))
                throw new ApiException("server.invalidLogin");

            // In many flows the Azure token is validated previously by middleware.
            // Here we simply produce an application JWT for the local user identity.
            var user = await _userManager.FindByNameAsync(information.Username)
                       ?? throw new ApiException("server.invalidLogin");

            // Optionally add an azure-specific claim:
            // e.g. new Claim("azure_token", information.AzureToken ?? string.Empty)

            var auth = await GenerateJWTokenAsync(user.Id);
            return auth;
        }

        public async Task<AuthenticatedResponse> RefreshAsync(UserRefreshToken userRefreshToken)
        {
            if (userRefreshToken == null || string.IsNullOrWhiteSpace(userRefreshToken.UserId) || string.IsNullOrWhiteSpace(userRefreshToken.RefreshToken))
                throw new ApiException("server.invalidRefreshRequest");

            var user = await _userManager.FindByIdAsync(userRefreshToken.UserId)
                       ?? throw new ApiException("server.invalidRefreshRequest");

            // Verify refresh token match and expiry
            if (user.RefreshToken != userRefreshToken.RefreshToken)
                throw new ApiException("server.invalidRefreshToken");

            if (!user.RefreshTokenExpiryDate.HasValue || user.RefreshTokenExpiryDate.Value < _dateTimeProvider.UtcNow)
                throw new ApiException("server.refreshTokenExpired");

            // Issue a new refresh token and save
            var newRefresh = GenerateRefreshToken();
            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiryDate = _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpireInMinutes);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new ApiException("server.unableToUpdateRefreshToken");

            // Generate a new JWT
            var authResponse = await GenerateJWTokenAsync(user.Id);
            authResponse.RefreshToken = newRefresh;
            return authResponse;
        }

        // helper: gather claims for the user including roles and user claims
        private async Task<List<Claim>> BuildUserClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
          

            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
        };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            claims.Add(new Claim( "OrgId", user.OrganizationId.ToString()));

            var roleNames = await _userManager.GetRolesAsync(user);

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName); // ApplicationRole
                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));

                    if (role.IsSuperAdmin) // check your custom property
                    {
                        claims.Add(new Claim("IsSuperAdmin", "true"));
                    }
                }
            }


            // Include any user claims stored in Identity
            //   var userClaims = await _userManager.GetClaimsAsync(user);
            // claims.AddRange(userClaims);

            return claims;
        }
    }

}
