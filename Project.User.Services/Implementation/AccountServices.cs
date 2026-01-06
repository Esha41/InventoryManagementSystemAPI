using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Exception;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.LdapSettings.Services.Interfaces;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Helpers;
using Ettad.User.Services.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace Ettad.User.Services.Implementation
{
    public class AccountServices : IAccountServices
    {
        private readonly IJwtServices _jwtServices;
        private readonly ILdapSettingsService _ldapSettingsService;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICaptchaService _captchaService;

        private readonly ApplicationDbContext _context;
        
        // Lockout configuration constants
        private const int MAX_FAILED_ATTEMPTS = 5;
        private const int LOCKOUT_DURATION_MINUTES = 15;
        private const int CAPTCHA_REQUIRED_AFTER_ATTEMPTS = 3;
        public AccountServices(
            IJwtServices jwtServices,
            ILdapSettingsService ldapSettingsService,
            //IUnitOfWork unitOfWork,
            ILdapAuthenticator ldapAuthenticator,
            IDateTimeProvider dateTimeProvider,
            IOptions<JwtOptions> jwtOptions,
            IOptions<AdminUsersOptions> adminUsers, UserManager<ApplicationUser> userRepository,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUserService, IEmailSender emailSender,
            ILogger<AccountServices> logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ICaptchaService captchaService)
        {
            _jwtServices = jwtServices ?? throw new ArgumentNullException(nameof(jwtServices));
            _ldapSettingsService = ldapSettingsService ?? throw new ArgumentNullException(nameof(ldapSettingsService));
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
            _httpContextAccessor = httpContextAccessor;
            _captchaService = captchaService;
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
                    await RecordLoginAttemptAsync(loginInformation?.Username, null, false, "Empty username", LoginType.Unknown, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }
                             
                var existingUser = await _userRepository.Users
                    .FirstOrDefaultAsync(u => u.UserName == loginInformation.Username.Trim() && !u.IsDeleted, cancellationToken);

                // Check if account is locked (for existing users)
                if (existingUser != null)
                {
                    var isLocked = await IsAccountLockedAsync(loginInformation.Username.Trim(), cancellationToken);
                    if (isLocked)
                    {
                        _logger.LogWarning("Login blocked: Account is locked due to too many failed attempts. Username: {Username}", 
                            loginInformation.Username);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), existingUser.Id, false, 
                            "Account locked due to too many failed attempts", LoginType.Admin, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Forbidden,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "Account is temporarily locked due to too many failed login attempts. Please try again in 15 minutes.");
                    }
                }

                // Check if CAPTCHA is required (3+ failed attempts but not locked yet)
                var captchaRequired = await IsCaptchaRequiredAsync(loginInformation.Username.Trim(), cancellationToken);
                if (captchaRequired)
                {
                    // If CAPTCHA is required but not provided, return error indicating CAPTCHA is needed
                    if (string.IsNullOrWhiteSpace(loginInformation.CaptchaId) || string.IsNullOrWhiteSpace(loginInformation.CaptchaCode))
                    {
                        _logger.LogWarning("Login blocked: CAPTCHA required but not provided. Username: {Username}", 
                            loginInformation.Username);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), existingUser?.Id, false, 
                            "CAPTCHA required but not provided", existingUser != null ? LoginType.Admin : LoginType.Unknown, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "CAPTCHA verification is required. Please complete the CAPTCHA and try again.");
                    }

                    // Validate CAPTCHA code
                    var captchaValid = _captchaService.ValidateCaptcha(loginInformation.CaptchaId, loginInformation.CaptchaCode);
                    if (!captchaValid)
                    {
                        _logger.LogWarning("Login blocked: Invalid CAPTCHA code. Username: {Username}", 
                            loginInformation.Username);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), existingUser?.Id, false, 
                            "Invalid CAPTCHA code", existingUser != null ? LoginType.Admin : LoginType.Unknown, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "CAPTCHA verification failed. Please try again.");
                    }
                }

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
                    await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false, 
                        "User not found and LDAP not configured", LoginType.Unknown, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error occurred. Username: {Username}", loginInformation?.Username);
                await RecordLoginAttemptAsync(loginInformation?.Username, null, false, ex.Message, LoginType.Unknown, cancellationToken);
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

            // Check if user is soft-deleted
            if (user.IsDeleted)
            {
                _logger.LogWarning("Admin login failed: User is deleted. Username: {Username}, UserId: {UserId}",
                    loginInformation.Username, user.Id);
                await RecordLoginAttemptAsync(loginInformation.Username, user.Id, false, "User is deleted", LoginType.Admin, cancellationToken);
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.Unauthorized,
                    CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                    "server.invalidLogin");
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginInformation.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                _logger.LogWarning("Admin login failed: Invalid password. Username: {Username}, UserId: {UserId}",
                    loginInformation.Username, user.Id);

                await RecordLoginAttemptAsync(loginInformation.Username, user.Id, false, "Invalid password", LoginType.Admin, cancellationToken);
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.Unauthorized,
                    CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                    "server.invalidLogin");
            }

            _logger.LogInformation("Admin login successful. Username: {Username}, UserId: {UserId}",
                loginInformation.Username, user.Id);

            // Record successful login
            await RecordLoginAttemptAsync(loginInformation.Username, user.Id, true, null, LoginType.Admin, cancellationToken);

            var authResponse = await CreateAndReturnAuthResponseAsync(user, cancellationToken);
            return APIOperationResponse<AuthenticatedResponse>.Success(authResponse);
        }


        public async Task<APIOperationResponse<AuthenticatedResponse>> LoginWithLdap(
    LoginInformation loginInformation,
    CancellationToken cancellationToken = default)
        {
            try
            {

                var ldapSettingsResponse = await _ldapSettingsService.GetLdapSettings(cancellationToken);

                if (!ldapSettingsResponse.Succeeded || ldapSettingsResponse.Data == null)
                {
                    _logger.LogWarning("LDAP login attempt failed: Failed to retrieve LDAP settings. Username: {Username}",
                        loginInformation?.Username);

                    await RecordLoginAttemptAsync(loginInformation?.Username ?? "Unknown", null, false, 
                        "Failed to retrieve LDAP settings", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_LDAP_SETTINGS,
                        "server.invalidLdapSettings");
                }

                var ldapSettings = ldapSettingsResponse.Data;

                if (!ldapSettings.IsActive)
                {
                    _logger.LogWarning("LDAP login attempt failed: LDAP settings inactive. Username: {Username}",
                        loginInformation?.Username);

                    await RecordLoginAttemptAsync(loginInformation?.Username ?? "Unknown", null, false, 
                        "LDAP settings inactive", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_LDAP_SETTINGS,
                        "server.invalidLdapSettings");
                }

                if (string.IsNullOrWhiteSpace(loginInformation?.Username))
                {
                    _logger.LogWarning("LDAP login attempt failed: Username is empty.");

                    await RecordLoginAttemptAsync("Unknown", null, false, "Username is empty", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                // Check if Windows logged-in user matches LDAP username
                _logger.LogInformation("Login request. IsAuthenticated: {Auth}, User: {User}",
     _httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated,_httpContextAccessor.HttpContext.User.Identity?.Name);

                var windowsUserFull = _httpContextAccessor.HttpContext?.User?.Identity?.Name; 
                // Trim domain
                var windowsUser = windowsUserFull?.Contains("\\") == true
                    ? windowsUserFull.Split('\\')[1]
                    : windowsUserFull;
                _logger.LogInformation("LDAP login attempt. Username: {Username}, WindowsIdentity: {windowsUser}",
                     loginInformation.Username, windowsUser ?? "N/A");
                if (string.IsNullOrEmpty(windowsUser))
                {
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "Windows authentication required.");
                }

                var inputUser = NormalizeUsername(loginInformation.Username.Trim());

                if (!windowsUser.Equals(inputUser, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(
                        "LDAP login blocked: Windows user mismatch. InputUser: {InputUser}, WindowsUser: {WindowsUser}",
                        inputUser,
                        windowsUser);

                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "You must log in from your own Windows account.");
                }
                // Check if account is locked (based on username)
                var isLocked = await IsAccountLockedAsync(loginInformation.Username.Trim(), cancellationToken);
                if (isLocked)
                {
                    _logger.LogWarning("LDAP login blocked: Account is locked due to too many failed attempts. Username: {Username}", 
                        loginInformation.Username);
                    await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false, 
                        "Account locked due to too many failed attempts", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Forbidden,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "Account is temporarily locked due to too many failed login attempts. Please try again in 15 minutes.");
                }

                // Check if CAPTCHA is required (3+ failed attempts but not locked yet)
                var captchaRequired = await IsCaptchaRequiredAsync(loginInformation.Username.Trim(), cancellationToken);
                if (captchaRequired)
                {
                    // If CAPTCHA is required but not provided, return error indicating CAPTCHA is needed
                    if (string.IsNullOrWhiteSpace(loginInformation.CaptchaId) || string.IsNullOrWhiteSpace(loginInformation.CaptchaCode))
                    {
                        _logger.LogWarning("LDAP login blocked: CAPTCHA required but not provided. Username: {Username}", 
                            loginInformation.Username);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false, 
                            "CAPTCHA required but not provided", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "CAPTCHA verification is required. Please complete the CAPTCHA and try again.");
                    }

                    // Validate CAPTCHA code
                    var captchaValid = _captchaService.ValidateCaptcha(loginInformation.CaptchaId, loginInformation.CaptchaCode);
                    if (!captchaValid)
                    {
                        _logger.LogWarning("LDAP login blocked: Invalid CAPTCHA code. Username: {Username}", 
                            loginInformation.Username);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false, 
                            "Invalid CAPTCHA code", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "CAPTCHA verification failed. Please try again.");
                    }
                }

                // 🧠 Step 1: Authenticate against LDAP
                var loginSucceeded = await _ldapAuthenticator.ValidateAsync(
                    loginInformation.Username.Trim(),
                    loginInformation.Password,
                    loginWithoutPassword: true,
                    ldapSettings,
                    cancellationToken);

                if (!loginSucceeded)
                {
                    _logger.LogWarning(
                        "LDAP login failed: Invalid credentials. Username: {Username}",
                        loginInformation.Username);

                    await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false, "Invalid LDAP credentials", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                var resolvedUsername = $"{loginInformation.Username.Trim()}@{ldapSettings.LdapDomain}";

                // 🧠 Step 2: Check if user exists by Username or LdapUserName (excluding soft-deleted users)
                var user = await _userRepository.Users
                    .FirstOrDefaultAsync(u => u.UserName == resolvedUsername && !u.IsDeleted, cancellationToken);

                if (user == null)
                {
                    user = await _context.Users
                        .FirstOrDefaultAsync(u => u.LdapUserName == resolvedUsername && !u.IsDeleted, cancellationToken);
                }

                // 🧩 Step 3: If user does not exist, create new
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = resolvedUsername,
                        Email = resolvedUsername,
                        FullNameAR = resolvedUsername,
                        FullNameEN = resolvedUsername,
                        IsLdapUser = true,
                        LdapUserName = loginInformation.Username.Trim()
                    };

                    await _userRepository.CreateAsync(user);
                    _logger.LogInformation(
                        "LDAP user auto-created. Username: {Username}, UserId: {UserId}",
                        user.UserName,
                        user.Id);
                }
                else
                {
                    // Check if existing user is soft-deleted
                    if (user.IsDeleted)
                    {
                        _logger.LogWarning("LDAP login failed: User is deleted. Username: {Username}, UserId: {UserId}",
                            user.UserName, user.Id);
                        await RecordLoginAttemptAsync(resolvedUsername, user.Id, false, "User is deleted", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Unauthorized,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "server.invalidLogin");
                    }

                    _logger.LogInformation(
                        "LDAP user already exists. Username: {Username}, UserId: {UserId}",
                        user.UserName,
                        user.Id);
                }

                // 🧠 Step 4: Build authenticated response
                var response = await CreateAndReturnAuthResponseAsync(user, cancellationToken);

                _logger.LogInformation(
                    "LDAP login successful. Username: {Username}, UserId: {UserId}",
                    resolvedUsername,
                    user.Id);

                // Record successful login
                await RecordLoginAttemptAsync(resolvedUsername, user.Id, true, null, LoginType.LDAP, cancellationToken);

                return APIOperationResponse<AuthenticatedResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "LDAP login attempt threw exception. Username: {Username}",
                    loginInformation?.Username);

                await RecordLoginAttemptAsync(loginInformation?.Username ?? "Unknown", null, false, 
                    $"Exception: {ex.Message}", LoginType.LDAP, cancellationToken);
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
            
            var user = await _userRepository.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email.Trim() && !u.IsDeleted);
            if (user == null)
            {
                _logger.LogWarning("Password reset failed: User not found. Email: {Email}", request.Email);
                // Don't reveal if user exists - security best practice
                return APIOperationResponse<string>.Success("If an account exists with this email, a password reset link has been sent.");
            }

            // Prevent LDAP users from resetting password (managed externally)
            if (user.IsLdapUser)
            {
                _logger.LogWarning("Password reset denied: LDAP user attempted reset. Email: {Email}, UserId: {UserId}", 
                    request.Email, user.Id);
                return APIOperationResponse<string>.Fail(
                    ResponseType.BadRequest, 
                    "LDAP users cannot reset their password through this system. Please contact your system administrator.");
            }

            var token = await _userRepository.GeneratePasswordResetTokenAsync(user);

            _logger.LogInformation("Password reset token generated. Email: {Email}, UserId: {UserId}", 
                request.Email, user.Id);

            // Send reset email
            var baseUrl = "http://localhost:4200/";
            var resetUrl = $"{baseUrl}auth/reset-password?email={System.Net.WebUtility.UrlEncode(request.Email)}&token={System.Net.WebUtility.UrlEncode(token)}";

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
                <p>This link will expire in 15 minutes.</p>
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
                return APIOperationResponse<string>.Fail(
                    ResponseType.InternalServerError, 
                    "Failed to send password reset email. Please try again later.");
            }
            
            return APIOperationResponse<string>.Success("If an account exists with this email, a password reset link has been sent.");
        }

        public async Task<APIOperationResponse<string>> ResetPasswordAsync(ResetPasswordDto request)
        {
            _logger.LogInformation("Password reset attempt. Email: {Email}", request.Email);
            
            var user = await _userRepository.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);
            if (user == null)
            {
                _logger.LogWarning("Password reset failed: User not found. Email: {Email}", request.Email);
                return APIOperationResponse<string>.Fail(ResponseType.BadRequest, "Invalid password reset request.");
            }

            // Prevent LDAP users from resetting password
            if (user.IsLdapUser)
            {
                _logger.LogWarning("Password reset denied: LDAP user attempted reset. Email: {Email}, UserId: {UserId}", 
                    request.Email, user.Id);
                return APIOperationResponse<string>.Fail(
                    ResponseType.BadRequest, 
                    "LDAP users cannot reset their password through this system. Please contact your system administrator.");
            }

            var result = await _userRepository.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Password reset failed: Validation errors. Email: {Email}, UserId: {UserId}, Errors: {Errors}", 
                    request.Email, user.Id, errors);
                return APIOperationResponse<string>.Fail(ResponseType.BadRequest, errors);
            }

            _logger.LogInformation("Password reset successful. Email: {Email}, UserId: {UserId}", 
                request.Email, user.Id);
            
            return APIOperationResponse<string>.Success("Password has been reset successfully.");
        }

        public async Task<APIOperationResponse<string>> LogoutAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var userName = _currentUserService.UserName;
                
                // Check for Windows authentication
                var windowsIdentity = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                var isWindowsAuthenticated = !string.IsNullOrEmpty(windowsIdentity);
                
                if (string.IsNullOrWhiteSpace(userId))
                {
                    // If no userId but Windows authenticated, still allow logout
                    if (isWindowsAuthenticated)
                    {
                        _logger.LogInformation("Windows user logged out (no user ID found). WindowsIdentity: {WindowsIdentity}", 
                            windowsIdentity);
                        return APIOperationResponse<string>.Success("Logged out successfully.");
                    }
                    
                    _logger.LogWarning("Logout attempt failed: No authenticated user context available.");
                    return APIOperationResponse<string>.Fail(
                        ResponseType.Unauthorized,
                        "No authenticated user found.");
                }

                var user = await _userRepository.FindByIdAsync(userId);
                if (user == null)
                {
                    // If Windows authenticated but user not found in DB, still allow logout
                    if (isWindowsAuthenticated)
                    {
                        _logger.LogInformation("Windows user logged out (user not found in database). UserId: {UserId}, WindowsIdentity: {WindowsIdentity}", 
                            userId, windowsIdentity);
                        return APIOperationResponse<string>.Success("Logged out successfully.");
                    }
                    
                    _logger.LogWarning("Logout attempt failed: User not found. UserId: {UserId}", userId);
                    return APIOperationResponse<string>.Fail(
                        ResponseType.NotFound,
                        "User not found.");
                }

                // Clear all tokens and set logout timestamp
                var hadRefreshToken = !string.IsNullOrEmpty(user.RefreshToken);
                user.RefreshToken = null;
                user.RefreshTokenExpiryDate = null;
               
                await _userRepository.UpdateAsync(user);

                // Clear refresh token cookie
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    try
                    {
                        httpContext.Response.Cookies.Delete("refreshToken", new Microsoft.AspNetCore.Http.CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None
                        });
                        _logger.LogInformation("Refresh token cookie deleted. UserId: {UserId}", userId);
                    }
                    catch (Exception ex)
                    {
                        // Log but don't fail logout if cookie deletion fails
                        _logger.LogWarning(ex, "Failed to delete refresh token cookie. UserId: {UserId}", userId);
                    }
                }

                // Log logout with user type information
                var userType = user.IsLdapUser ? "Windows/LDAP" : "Admin";
                var logMessage = isWindowsAuthenticated 
                    ? "Windows user logged out successfully. UserId: {UserId}, Username: {Username}, UserType: {UserType}, WindowsIdentity: {WindowsIdentity}, HadRefreshToken: {HadRefreshToken}"
                    : "User logged out successfully. UserId: {UserId}, Username: {Username}, UserType: {UserType}, HadRefreshToken: {HadRefreshToken}";
                
                if (isWindowsAuthenticated)
                {
                    _logger.LogInformation(logMessage, userId, user.UserName, userType, windowsIdentity, hadRefreshToken);
                }
                else
                {
                    _logger.LogInformation(logMessage, userId, user.UserName, userType, hadRefreshToken);
                }

                return APIOperationResponse<string>.Success("Logged out successfully.");
            }
            catch (Exception ex)
            {
                var windowsIdentity = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                _logger.LogError(ex, "Error occurred during logout. UserId: {UserId}, WindowsIdentity: {WindowsIdentity}", 
                    _currentUserService.UserId, windowsIdentity ?? "N/A");
                return APIOperationResponse<string>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "An error occurred during logout.");
            }
        }

        #region Login Tracking Helper Methods

        /// <summary>
        /// Checks if an account is locked due to too many failed login attempts
        /// </summary>
        private async Task<bool> IsAccountLockedAsync(string username, CancellationToken cancellationToken)
        {
            var lockoutThreshold = DateTime.UtcNow.AddMinutes(-LOCKOUT_DURATION_MINUTES);
            
            var failedAttempts = await _context.LoginAttempts
                .Where(la => la.Username == username 
                    && !la.IsSuccessful 
                    && la.AttemptDate >= lockoutThreshold)
                .CountAsync(cancellationToken);

            return failedAttempts >= MAX_FAILED_ATTEMPTS;
        }

        /// <summary>
        /// Checks if CAPTCHA is required (3+ failed attempts but account not locked yet)
        /// </summary>
        private async Task<bool> IsCaptchaRequiredAsync(string username, CancellationToken cancellationToken)
        {
            var lockoutThreshold = DateTime.UtcNow.AddMinutes(-LOCKOUT_DURATION_MINUTES);
            
            var failedAttempts = await _context.LoginAttempts
                .Where(la => la.Username == username 
                    && !la.IsSuccessful 
                    && la.AttemptDate >= lockoutThreshold)
                .CountAsync(cancellationToken);

            // CAPTCHA required if 3+ failed attempts but less than 5 (which would lock the account)
            return failedAttempts >= CAPTCHA_REQUIRED_AFTER_ATTEMPTS && failedAttempts < MAX_FAILED_ATTEMPTS;
        }

        /// <summary>
        /// Records a login attempt in the database
        /// </summary>
        private async Task RecordLoginAttemptAsync(
            string username, 
            string? userId, 
            bool isSuccessful, 
            string? failureReason, 
            LoginType loginType,
            CancellationToken cancellationToken)
        {
            try
            {
                var loginAttempt = new LoginAttempt
                {
                    Username = username ?? "Unknown",
                    UserId = userId,
                    IsSuccessful = isSuccessful,
                    FailureReason = failureReason,
                    IpAddress = GetClientIpAddress(),
                    UserAgent = GetUserAgent(),
                    LoginType = loginType,
                    AttemptDate = DateTime.UtcNow
                };

                _context.LoginAttempts.Add(loginAttempt);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log but don't fail the login if tracking fails
                _logger.LogError(ex, "Failed to record login attempt for username: {Username}", username);
            }
        }

        /// <summary>
        /// Gets the client IP address from HttpContext
        /// </summary>
        private string? GetClientIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return null;

                // Check for forwarded IP (when behind proxy/load balancer)
                var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    var ips = forwardedFor.Split(',');
                    return ips[0].Trim();
                }

                // Check for real IP header
                var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
                if (!string.IsNullOrEmpty(realIp))
                {
                    return realIp;
                }

                // Fall back to connection remote IP
                return httpContext.Connection.RemoteIpAddress?.ToString();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the User-Agent from HttpContext, enhanced with client information from headers if available
        /// </summary>
        private string? GetUserAgent()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return null;

                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                
                // Enhance with client information from custom headers if available
                var clientInfoParts = new List<string>();
                
                var browser = httpContext.Request.Headers["X-Client-Browser"].FirstOrDefault();
                var browserVersion = httpContext.Request.Headers["X-Client-BrowserVersion"].FirstOrDefault();
                var os = httpContext.Request.Headers["X-Client-OS"].FirstOrDefault();
                var osVersion = httpContext.Request.Headers["X-Client-OSVersion"].FirstOrDefault();
                var device = httpContext.Request.Headers["X-Client-Device"].FirstOrDefault();
                
                // Get Windows user from HttpContext (when Windows Authentication is enabled)
                var windowsUser = httpContext.User?.Identity?.Name;
                if (!string.IsNullOrEmpty(windowsUser))
                {
                    clientInfoParts.Add($"WindowsUser: {windowsUser}");
                }
                
                if (!string.IsNullOrEmpty(browser))
                {
                    var browserInfo = !string.IsNullOrEmpty(browserVersion) 
                        ? $"{browser} {browserVersion}" 
                        : browser;
                    clientInfoParts.Add($"Browser: {browserInfo}");
                }
                
                if (!string.IsNullOrEmpty(os))
                {
                    var osInfo = !string.IsNullOrEmpty(osVersion) 
                        ? $"{os} {osVersion}" 
                        : os;
                    clientInfoParts.Add($"OS: {osInfo}");
                }
                
                if (!string.IsNullOrEmpty(device))
                {
                    clientInfoParts.Add($"Device: {device}");
                }
                
                if (clientInfoParts.Any())
                {
                    var enhancedUserAgent = $"{userAgent} | {string.Join(" | ", clientInfoParts)}";
                    // Truncate if too long (max 500 chars for database)
                    return enhancedUserAgent.Length > 500 ? enhancedUserAgent.Substring(0, 500) : enhancedUserAgent;
                }
                
                return userAgent;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Normalizes username by removing domain prefix/suffix for comparison
        /// Handles formats like: DOMAIN\username, username@domain.com, username
        /// </summary>
        private static string NormalizeUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return string.Empty;

            // DOMAIN\username → username
            if (username.Contains("\\"))
                return username.Split('\\').Last();

            // username@domain.com → username
            if (username.Contains("@"))
                return username.Split('@').First();

            return username;
        }

        #endregion
    }
}
