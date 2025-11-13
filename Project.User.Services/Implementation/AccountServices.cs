using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Exception;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Helpers;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Ettad.User.Services.Implementation
{
    public class AccountServices : IAccountServices
    {
        private readonly IJwtServices _jwtServices;
        private readonly ISettingsProvider _settingsProvider;
      //  private readonly IUnitOfWork _unitOfWork;
        private readonly ILdapAuthenticator _ldapAuthenticator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly JwtOptions _jwtOptions;
        private readonly AdminUsersOptions _adminUsers;
        private readonly UserManager<ApplicationUser> _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountServices> _logger;

        private readonly ApplicationDbContext _context;
        public AccountServices(
            IJwtServices jwtServices,
            ISettingsProvider settingsProvider,
            //IUnitOfWork unitOfWork,
            ILdapAuthenticator ldapAuthenticator,
            IDateTimeProvider dateTimeProvider,
            IOptions<JwtOptions> jwtOptions,
            IOptions<AdminUsersOptions> adminUsers, UserManager<ApplicationUser> userRepository,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUserService, IEmailSender emailSender,
            ILogger<AccountServices> logger, ApplicationDbContext context)
        {
            _jwtServices = jwtServices ?? throw new ArgumentNullException(nameof(jwtServices));
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            // _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _ldapAuthenticator = ldapAuthenticator ?? throw new ArgumentNullException(nameof(ldapAuthenticator));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _jwtOptions = jwtOptions?.Value ?? new JwtOptions();
            _adminUsers = adminUsers?.Value ?? new AdminUsersOptions();
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _signInManager = signInManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _emailSender = emailSender;
            _context = context;
            _logger = logger;
            _context = context;
        }

        public async Task<APIOperationResponse<AuthenticatedResponse>> Login(
    LoginInformation loginInformation,
    CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Login attempt. Username: {Username}", loginInformation?.Username);

            try
            {
                if (string.IsNullOrWhiteSpace(loginInformation?.Username))
                {
                    _logger.LogWarning("Login failed: Empty username provided");
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }
                             
                var existingUser = await _userRepository.FindByNameAsync(loginInformation.Username.Trim());

                var isAdminLogin = existingUser != null;

                _logger.LogInformation("Login type determined. Username: {Username}, IsAdminLogin: {IsAdminLogin}",
                    loginInformation.Username, isAdminLogin);

                if (isAdminLogin)
                {
                    return await LoginWithAdmin(existingUser, loginInformation, cancellationToken);
                }
                else if (loginInformation.IsLdap)
                {
                    return await LoginWithLdap(loginInformation, cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Login failed: LDAP not configured and user not found locally. Username: {Username}",
                        loginInformation.Username);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error occurred. Username: {Username}", loginInformation?.Username);
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    ex.Message);
            }
        }
        private async Task<APIOperationResponse<AuthenticatedResponse>> LoginWithAdmin(
            ApplicationUser user,
            LoginInformation loginInformation,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting admin login. Username: {Username}", loginInformation.Username);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginInformation.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                _logger.LogWarning("Admin login failed: Invalid password. Username: {Username}, UserId: {UserId}",
                    loginInformation.Username, user.Id);

                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.Unauthorized,
                    CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                    "server.invalidLogin");
            }

            _logger.LogInformation("Admin login successful. Username: {Username}, UserId: {UserId}",
                loginInformation.Username, user.Id);

            var authResponse = await CreateAndReturnAuthResponseAsync(user, cancellationToken);
            return APIOperationResponse<AuthenticatedResponse>.Success(authResponse);
        }


        public async Task<APIOperationResponse<AuthenticatedResponse>> LoginWithLdap(
    LoginInformation loginInformation,
    CancellationToken cancellationToken = default)
        {
            try
            {
                var ldapSettings = await _settingsProvider.GetLdapSettings(cancellationToken);

                if (!ldapSettings.IsActive)
                {
                    _logger.LogWarning("LDAP login attempt failed: LDAP settings inactive. Username: {Username}",
                        loginInformation?.Username);

                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_LDAP_SETTINGS,
                        "server.invalidLdapSettings");
                }

                if (string.IsNullOrWhiteSpace(loginInformation?.Username))
                {
                    _logger.LogWarning("LDAP login attempt failed: Username is empty.");

                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                var loginSucceeded = await _ldapAuthenticator.ValidateAsync(
                    loginInformation.Username.Trim(),
                    loginInformation.Password,
                    loginWithoutPassword: true,
                    ldapSettings,
                    cancellationToken);

                if (!loginSucceeded)
                {
                    _logger.LogWarning(
                        "LDAP login failed: Invalid password. Username: {Username}",
                        loginInformation.Username);

                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                var resolvedUsername = $"{loginInformation.Username.Trim()}@{ldapSettings.LdapDomain}";

                // Check if user exists in the database
                var user = await _userRepository.FindByNameAsync(resolvedUsername);
                if (user == null)
                {
                    // Auto-create the user
                    user = new ApplicationUser
                    {
                        UserName = resolvedUsername,
                        Email = resolvedUsername,
                       FullNameAR=resolvedUsername,
                       FullNameEN=resolvedUsername,
                       IsLdapUser=true,
                       LdapUserName=resolvedUsername,
                    };

                    await _userRepository.CreateAsync(user); // Make sure this saves to DB
                    _logger.LogInformation(
                        "LDAP user auto-created. Username: {Username}, UserId: {UserId}",
                        user.UserName,
                        user.Id);
                }

                var response = await CreateAndReturnAuthResponseAsync(user, cancellationToken);

                _logger.LogInformation(
                    "LDAP login successful. Username: {Username}, UserId: {UserId}",
                    resolvedUsername,
                    user.Id);

                return APIOperationResponse<AuthenticatedResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "LDAP login attempt threw exception. Username: {Username}",
                    loginInformation?.Username);

                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    ex.Message);
            }
        }



        public Task<AuthenticatedResponse> LoginWithAzure(LoginWithAzureInformation loginInformation, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(loginInformation?.Username)) throw new ApiException("server.invalidLogin");
            return _jwtServices.GenerateAzureJWTokenAsync(loginInformation);
        }

        public Task<AuthenticatedResponse> RefreshUserTokenAsync(UserRefreshToken userRefreshToken, CancellationToken cancellationToken = default)
        {
            return _jwtServices.RefreshAsync(userRefreshToken);
        }

        private async Task<AuthenticatedResponse> CreateAndReturnAuthResponseAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var refreshToken = _jwtServices.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryDate = _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpireInMinutes);

            await _userRepository.UpdateAsync(user);

            var authResponse = await _jwtServices.GenerateJWTokenAsync(user.Id);
            authResponse.RefreshToken = refreshToken;
            return authResponse;
        }
        public async Task<APIOperationResponse<List<ClaimDto>>> GetRoleClaimsOnlyAsync()
        {
            var roleClaims = new List<ClaimDto>();

            // 1. Get the current user
            var user = await _userRepository.FindByIdAsync(_currentUserService.UserId);
            if (user == null)
                return APIOperationResponse < List < ClaimDto >>.Success(roleClaims);

            // 2. Get user roles
            var roles = await _userRepository.GetRolesAsync(user);

            // 3. Collect claims for each role
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var claims = await _roleManager.GetClaimsAsync(role);


                    roleClaims.AddRange(claims.Select
                          (x => new ClaimDto
                          {
                              Id = x.Value,
                              ClaimType = x.Type,

                          }));
                }
            }

            return APIOperationResponse<List<ClaimDto>>.Success(roleClaims);
        }

        public async Task<APIOperationResponse<string>> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            _logger.LogInformation("Password reset requested. Email: {Email}", request.Email);
            
            var user = await _userRepository.FindByEmailAsync(request.Email.Trim());
            if (user == null)
            {
                _logger.LogWarning("Password reset failed: User not found. Email: {Email}", request.Email);
                return APIOperationResponse<string>.Fail(ResponseType.NotFound, "User not found");
            }

            var token = await _userRepository.GeneratePasswordResetTokenAsync(user);

            _logger.LogInformation("Password reset token generated. Email: {Email}, UserId: {UserId}", 
                request.Email, user.Id);

            // ?? Normally, you would send this token via Email/SMS using IEmailService
            // For now, just return it (not safe for production!)
            var baseUrl = "http://localhost:4200/";
            var resetUrl = $"{baseUrl}resetPassword?email={System.Net.WebUtility.UrlEncode(request.Email)}&token={System.Net.WebUtility.UrlEncode(token)}";

            string emailBody = $@"
            <html>
            <body style='font-family: Arial, sans-serif; text-align: center;'>
                <h2>Password Reset Request</h2>
                <p>Click the button below to reset your password:</p>
                <a href='{resetUrl}' style='text-decoration: none;'>
                    <button style='background-color: #007bff; color: white; padding: 10px 20px; 
                    border: none; border-radius: 5px; font-size: 16px; cursor: pointer;'>
                        Reset Password
                    </button>
                </a>
                <p>If you did not request a password reset, please ignore this email.</p>
            </body>
            </html>";

            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Reset Password", emailBody);
                _logger.LogInformation("Password reset email sent successfully. Email: {Email}, UserId: {UserId}", 
                    request.Email, user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email. Email: {Email}, UserId: {UserId}", 
                    request.Email, user.Id);
            }
            
            return APIOperationResponse<string>.Success(token);
        }

        public async Task<APIOperationResponse<string>> ResetPasswordAsync(ResetPasswordDto request)
        {
            _logger.LogInformation("Password reset attempt. Email: {Email}", request.Email);
            
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset failed: User not found. Email: {Email}", request.Email);
                return APIOperationResponse<string>.Fail(ResponseType.NotFound, "User not found");
            }

            var result = await _userRepository.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Password reset failed: Validation errors. Email: {Email}, UserId: {UserId}, Errors: {Errors}", 
                    request.Email, user.Id, errors);
                return APIOperationResponse<string>.Fail(ResponseType.BadRequest, errors);
            }

            _logger.LogInformation("Password reset successful. Email: {Email}, UserId: {UserId}", 
                request.Email, user.Id);
            
            return APIOperationResponse<string>.Success("Password has been reset successfully.");
        }

    }
}
