using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Exception;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.Data.Interfaces.Repositories;


namespace Ettad.User.Services.Services
{
    public class JwtServices : IJwtServices
    {
        public const string TokenPurposeClaim = "token_purpose";
        public const string TokenPurposeRoleSelection = "RoleSelection";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEffectiveRoleRepository _effectiveRoleService;

        public JwtServices(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtOptions> jwtOptions,
            IDateTimeProvider dateTimeProvider,
            RoleManager<ApplicationRole> roleManager,
            IEffectiveRoleRepository effectiveRoleService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _roleManager = roleManager;
            _effectiveRoleService = effectiveRoleService ?? throw new ArgumentNullException(nameof(effectiveRoleService));

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

            var user = await _userManager.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == userId)
                       ?? throw new Exception("server.invalidLogin");

            var effectiveRoleId = await _effectiveRoleService.GetEffectiveRoleIdAsync(user.Id);
            var effectiveRole = !string.IsNullOrEmpty(effectiveRoleId)
                ? await _roleManager.FindByIdAsync(effectiveRoleId)
                : null;
            if (effectiveRole == null)
                throw new ApiException("server.roleSelectionRequired");

            var now = _dateTimeProvider.Now;
            var expires = now.AddMinutes(_jwtOptions.AccessTokenExpireInMinutes);

            var claims = await BuildUserClaimsAsync(user, effectiveRole);
            
            // Add unique JWT ID (jti) claim for token blacklisting support
            // This allows us to invalidate specific tokens when users logout
            var tokenId = Guid.NewGuid().ToString();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, tokenId));

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

            // Store CurrentTokenId for single-session validation (invalidate previous sessions on new login)
            user.CurrentTokenId = tokenId;
            await _userManager.UpdateAsync(user);

            return new AuthenticatedResponse
            {
                AccessToken = tokenString,
                ExpiresAt = expires // present local time to caller
            };
        }

        public string GenerateRoleSelectionToken(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("userId required", nameof(userId));

            var now = _dateTimeProvider.Now;
            var expires = now.AddMinutes(Math.Max(1, _jwtOptions.RoleSelectionTokenExpireInMinutes));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(TokenPurposeClaim, TokenPurposeRoleSelection),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string ValidateRoleSelectionTokenAndGetUserId(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    // Match JwtBearer middleware (Program.cs): issuer/audience are optional in config
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };

                var principal = handler.ValidateToken(token, parameters, out _);
                var purpose = principal.FindFirst(TokenPurposeClaim)?.Value;
                if (purpose != TokenPurposeRoleSelection)
                    return null;

                return principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                       ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task<AuthenticatedResponse> GenerateAzureJWTokenAsync(LoginWithAzureInformation information)
        {
            if (information == null || string.IsNullOrWhiteSpace(information.Username))
                throw new ApiException("server.invalidLogin");

            // In many flows the Azure token is validated previously by middleware.
            // Here we simply produce an application JWT for the local user identity.
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == information.Username && !u.IsDeleted)
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

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userRefreshToken.UserId && !u.IsDeleted)
                       ?? throw new ApiException("server.invalidRefreshRequest");

            // Verify refresh token match and expiry
            if (user.RefreshToken != userRefreshToken.RefreshToken)
                throw new ApiException("server.invalidRefreshToken");

            if (!user.RefreshTokenExpiryDate.HasValue || user.RefreshTokenExpiryDate.Value < _dateTimeProvider.Now)
                throw new ApiException("server.refreshTokenExpired");

            // Issue a new refresh token.
            // Keep the old token in PreviousRefreshToken for a 60-second grace window.
            // This covers the scenario where the backend rotated the token and wrote it
            // to the DB, but the Set-Cookie response was lost before the client received
            // it (network drop / load-balancer timeout). On the client's retry the old
            // cookie is still presented — we accept it within the grace window so the
            // user is not permanently locked out.
            var newRefresh = GenerateRefreshToken();
            user.PreviousRefreshToken = user.RefreshToken;
            user.PreviousRefreshTokenExpiresAt = _dateTimeProvider.Now.AddSeconds(60);
            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiryDate = _dateTimeProvider.Now.AddMinutes(_jwtOptions.RefreshTokenExpireInMinutes);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new ApiException("server.unableToUpdateRefreshToken");

            // Generate a new JWT
            var authResponse = await GenerateJWTokenAsync(user.Id);
            authResponse.RefreshToken = newRefresh;
            return authResponse;
        }

        // helper: gather claims for the user including roles and user claims
        private async Task<List<Claim>> BuildUserClaimsAsync(ApplicationUser user, ApplicationRole effectiveRole)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
          

            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
        };

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

           

            if (user.DepartmentId.HasValue)
            {
                claims.Add(new Claim("DepartmentId", user.DepartmentId.Value.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(user.FullNameEN))
            {
                claims.Add(new Claim("FullNameEN", user.FullNameEN));
            }

            if (!string.IsNullOrWhiteSpace(user.FullNameAR))
            {
                claims.Add(new Claim("FullNameAR", user.FullNameAR));
            }

            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                claims.Add(new Claim("UserName", user.UserName));
            }

            if (user.Department != null)
            {
                var departmentName = !string.IsNullOrWhiteSpace(user.Department.NameEn)
                    ? user.Department.NameEn
                    : user.Department.NameAr;

                if (!string.IsNullOrWhiteSpace(departmentName))
                {
                    claims.Add(new Claim("DepartmentName", departmentName));
                }
            }

            claims.Add(new Claim("ActiveRoleId", effectiveRole.Id));
            claims.Add(new Claim(ClaimTypes.Role, effectiveRole.Name ?? string.Empty));

            if (effectiveRole.IsSuperAdmin)
                claims.Add(new Claim("IsSuperAdmin", "true"));

            return await Task.FromResult(claims);
        }
    }

}
