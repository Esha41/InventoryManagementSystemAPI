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
        private readonly ITokenBlacklistService _tokenBlacklistService;

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
            ILogger<AccountServices> logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ICaptchaService captchaService, ITokenBlacklistService tokenBlacklistService)
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
            _tokenBlacklistService = tokenBlacklistService ?? throw new ArgumentNullException(nameof(tokenBlacklistService));
        }

        public async Task<APIOperationResponse<AuthenticatedResponse>> Login(
    LoginInformation loginInformation,
    CancellationToken cancellationToken = default)
        {
            var startTime = _dateTimeProvider.Now;
            var clientIp = GetClientIpAddress();
            var loginType = loginInformation?.IsLdap == true ? "LDAP" : "Admin";

            _logger.LogInformation(
                "[LOGIN] Attempt started | Username: {Username} | LoginType: {LoginType} | IP: {ClientIP} | Time: {StartTime}",
                loginInformation?.Username ?? "N/A", loginType, clientIp ?? "Unknown", startTime);

            try
            {
                if (string.IsNullOrWhiteSpace(loginInformation?.Username))
                {
                    _logger.LogWarning(
                        "[LOGIN] FAILED - Empty username | IP: {ClientIP}",
                        clientIp);
                    await RecordLoginAttemptAsync(loginInformation?.Username, null, false, "Empty username", LoginType.Unknown, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                // Check if user exists (including soft-deleted) to provide proper error message
                var userIncludingDeleted = await _userRepository.Users
                    .FirstOrDefaultAsync(u => u.UserName == loginInformation.Username.Trim(), cancellationToken);

                if (userIncludingDeleted != null && userIncludingDeleted.IsDeleted)
                {
                    _logger.LogWarning(
                        "[LOGIN] FAILED - User deleted | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                        loginInformation.Username, userIncludingDeleted.Id, clientIp);
                    await RecordLoginAttemptAsync(loginInformation.Username.Trim(), userIncludingDeleted.Id, false,
                        "User account has been deleted", LoginType.Admin, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Forbidden,
                        CommonErrorCodes.ACCOUNT_DELETED,
                        "server.accountDeleted");
                }

                var existingUser = await _userRepository.Users
                    .FirstOrDefaultAsync(u => u.UserName == loginInformation.Username.Trim() && !u.IsDeleted, cancellationToken);

                _logger.LogDebug(
                    "[LOGIN] User lookup | Username: {Username} | Found: {UserFound} | UserId: {UserId}",
                    loginInformation.Username, existingUser != null, existingUser?.Id ?? "N/A");

                // Check if account is locked (for existing users)
                if (existingUser != null)
                {
                    var isLocked = await IsAccountLockedAsync(loginInformation.Username.Trim(), cancellationToken);
                    if (isLocked)
                    {
                        _logger.LogWarning(
                            "[LOGIN] BLOCKED - Account locked | Username: {Username} | UserId: {UserId} | IP: {ClientIP} | Reason: Too many failed attempts",
                            loginInformation.Username, existingUser.Id, clientIp);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), existingUser.Id, false,
                            "Account locked due to too many failed attempts", LoginType.Admin, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Forbidden,
                            CommonErrorCodes.ACCOUNT_LOCKED,
                            "Account is temporarily locked due to too many failed login attempts. Please try again in 15 minutes.");
                    }
                }

                // Check if CAPTCHA is required (3+ failed attempts but not locked yet)
                var captchaRequired = await IsCaptchaRequiredAsync(loginInformation.Username.Trim(), cancellationToken);
                if (captchaRequired)
                {
                    _logger.LogDebug(
                        "[LOGIN] CAPTCHA required | Username: {Username} | CaptchaProvided: {CaptchaProvided}",
                        loginInformation.Username, !string.IsNullOrWhiteSpace(loginInformation.CaptchaId));

                    // If CAPTCHA is required but not provided, return error indicating CAPTCHA is needed
                    if (string.IsNullOrWhiteSpace(loginInformation.CaptchaId) || string.IsNullOrWhiteSpace(loginInformation.CaptchaCode))
                    {
                        _logger.LogWarning(
                            "[LOGIN] BLOCKED - CAPTCHA required but not provided | Username: {Username} | IP: {ClientIP}",
                            loginInformation.Username, clientIp);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), existingUser?.Id, false,
                            "CAPTCHA required but not provided", existingUser != null ? LoginType.Admin : LoginType.Unknown, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.CAPTCHA_REQUIRED,
                            "CAPTCHA verification is required. Please complete the CAPTCHA and try again.");
                    }

                    // Validate CAPTCHA code
                    var captchaValid = _captchaService.ValidateCaptcha(loginInformation.CaptchaId, loginInformation.CaptchaCode);
                    if (!captchaValid)
                    {
                        _logger.LogWarning(
                            "[LOGIN] BLOCKED - Invalid CAPTCHA | Username: {Username} | CaptchaId: {CaptchaId} | IP: {ClientIP}",
                            loginInformation.Username, loginInformation.CaptchaId, clientIp);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), existingUser?.Id, false,
                            "Invalid CAPTCHA code", existingUser != null ? LoginType.Admin : LoginType.Unknown, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.CAPTCHA_INVALID,
                            "CAPTCHA verification failed. Please try again.");
                    }

                    _logger.LogDebug("[LOGIN] CAPTCHA validated successfully | Username: {Username}", loginInformation.Username);
                }

                // If not an LDAP login and user doesn't exist, return invalid credentials
                if (!loginInformation.IsLdap && existingUser == null)
                {
                    _logger.LogWarning(
                        "[LOGIN] FAILED - User not found | Username: {Username} | IP: {ClientIP}",
                        loginInformation.Username, clientIp);
                    await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false, "User not found", LoginType.Admin, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                var isAdminLogin = existingUser != null && !loginInformation.IsLdap;

                _logger.LogInformation(
                    "[LOGIN] Routing to handler | Username: {Username} | LoginType: {LoginType} | IsLocalUser: {IsLocalUser}",
                    loginInformation.Username, isAdminLogin ? "Admin" : (loginInformation.IsLdap ? "LDAP" : "Unknown"), isAdminLogin);

                if (isAdminLogin)
                {
                    return await LoginWithAdmin(existingUser, loginInformation, cancellationToken);
                }
                else
                {
                    return await LoginWithLdap(loginInformation, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                var duration = (_dateTimeProvider.Now - startTime).TotalMilliseconds;
                _logger.LogError(ex,
                    "[LOGIN] ERROR - Exception occurred | Username: {Username} | IP: {ClientIP} | Duration: {Duration}ms | Error: {ErrorMessage}",
                    loginInformation?.Username ?? "N/A", clientIp, duration, ex.Message);
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
            var startTime = _dateTimeProvider.Now;
            var clientIp = GetClientIpAddress();

            _logger.LogInformation(
                "[ADMIN LOGIN] Started | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                loginInformation.Username, user.Id, clientIp);

            // Check if user is soft-deleted (defensive check - should not happen due to filtering)
            if (user.IsDeleted)
            {
                _logger.LogWarning(
                    "[ADMIN LOGIN] FAILED - User deleted | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                    loginInformation.Username, user.Id, clientIp);
                await RecordLoginAttemptAsync(loginInformation.Username, user.Id, false, "User account has been deleted", LoginType.Admin, cancellationToken);
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.Forbidden,
                    CommonErrorCodes.ACCOUNT_DELETED,
                    "server.accountDeleted");
            }

            // Check if account is active
            if (!user.IsActive)
            {
                _logger.LogWarning(
                    "[ADMIN LOGIN] FAILED - Account disabled | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                    loginInformation.Username, user.Id, clientIp);
                await RecordLoginAttemptAsync(loginInformation.Username, user.Id, false,
                    "Account is disabled", LoginType.Admin, cancellationToken);
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.Forbidden,
                    CommonErrorCodes.ACCOUNT_DISABLED,
                    "Your account has been disabled. Please contact your administrator.");
            }

            _logger.LogDebug(
                "[ADMIN LOGIN] Validating password | Username: {Username} | UserId: {UserId}",
                loginInformation.Username, user.Id);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginInformation.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                _logger.LogWarning(
                    "[ADMIN LOGIN] FAILED - Invalid password | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                    loginInformation.Username, user.Id, clientIp);

                await RecordLoginAttemptAsync(loginInformation.Username, user.Id, false, "Invalid password", LoginType.Admin, cancellationToken);
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.Unauthorized,
                    CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                    "server.invalidLogin");
            }

            // Record successful login
            await RecordLoginAttemptAsync(loginInformation.Username, user.Id, true, null, LoginType.Admin, cancellationToken);

            var authResponse = await CreateAndReturnAuthResponseAsync(user, cancellationToken);

            var duration = (_dateTimeProvider.Now - startTime).TotalMilliseconds;
            _logger.LogInformation(
                "[ADMIN LOGIN] SUCCESS | Username: {Username} | UserId: {UserId} | IP: {ClientIP} | Duration: {Duration}ms",
                loginInformation.Username, user.Id, clientIp, duration);

            return APIOperationResponse<AuthenticatedResponse>.Success(authResponse);
        }


        public async Task<APIOperationResponse<AuthenticatedResponse>> LoginWithLdap(
    LoginInformation loginInformation,
    CancellationToken cancellationToken = default)
        {
            var startTime = _dateTimeProvider.Now;
            var clientIp = GetClientIpAddress();

            _logger.LogInformation(
                "[LDAP LOGIN] Started | Username: {Username} | IP: {ClientIP}",
                loginInformation?.Username ?? "N/A", clientIp);

            try
            {
                // Step 1: Retrieve and validate LDAP settings
                _logger.LogDebug("[LDAP LOGIN] Retrieving LDAP settings");
                var ldapSettingsResponse = await _ldapSettingsService.GetLdapSettings(cancellationToken);

                if (!ldapSettingsResponse.Succeeded || ldapSettingsResponse.Data == null)
                {
                    _logger.LogWarning(
                        "[LDAP LOGIN] FAILED - LDAP settings unavailable | Username: {Username} | IP: {ClientIP}",
                        loginInformation?.Username ?? "N/A", clientIp);

                    await RecordLoginAttemptAsync(loginInformation?.Username ?? "Unknown", null, false,
                        "Failed to retrieve LDAP settings", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_LDAP_SETTINGS,
                        "server.invalidLdapSettings");
                }

                var ldapSettings = ldapSettingsResponse.Data;

                _logger.LogDebug(
                    "[LDAP LOGIN] LDAP settings loaded | Server: {Server} | Domain: {Domain} | IsActive: {IsActive}",
                    ldapSettings.LdapServer, ldapSettings.LdapDomain, ldapSettings.IsActive);

                if (!ldapSettings.IsActive)
                {
                    _logger.LogWarning(
                        "[LDAP LOGIN] FAILED - LDAP disabled | Username: {Username} | IP: {ClientIP}",
                        loginInformation?.Username ?? "N/A", clientIp);

                    await RecordLoginAttemptAsync(loginInformation?.Username ?? "Unknown", null, false,
                        "LDAP settings inactive", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_LDAP_SETTINGS,
                        "server.invalidLdapSettings");
                }

                // Step 2: Validate input credentials
                if (string.IsNullOrWhiteSpace(loginInformation?.Username))
                {
                    _logger.LogWarning("[LDAP LOGIN] FAILED - Empty username | IP: {ClientIP}", clientIp);

                    await RecordLoginAttemptAsync("Unknown", null, false, "Username is empty", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                if (string.IsNullOrWhiteSpace(loginInformation?.Password))
                {
                    _logger.LogWarning(
                        "[LDAP LOGIN] FAILED - Empty password | Username: {Username} | IP: {ClientIP}",
                        loginInformation?.Username, clientIp);

                    await RecordLoginAttemptAsync(loginInformation?.Username ?? "Unknown", null, false, "Password is empty", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                // Step 3: Check account lockout status
                var isLocked = await IsAccountLockedAsync(loginInformation.Username.Trim(), cancellationToken);
                if (isLocked)
                {
                    _logger.LogWarning(
                        "[LDAP LOGIN] BLOCKED - Account locked | Username: {Username} | IP: {ClientIP} | Reason: Too many failed attempts",
                        loginInformation.Username, clientIp);
                    await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false,
                        "Account locked due to too many failed attempts", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Forbidden,
                        CommonErrorCodes.ACCOUNT_LOCKED,
                        "Account is temporarily locked due to too many failed login attempts. Please try again in 15 minutes.");
                }

                // Step 4: Check CAPTCHA requirement
                var captchaRequired = await IsCaptchaRequiredAsync(loginInformation.Username.Trim(), cancellationToken);
                if (captchaRequired)
                {
                    _logger.LogDebug(
                        "[LDAP LOGIN] CAPTCHA required | Username: {Username} | CaptchaProvided: {CaptchaProvided}",
                        loginInformation.Username, !string.IsNullOrWhiteSpace(loginInformation.CaptchaId));

                    if (string.IsNullOrWhiteSpace(loginInformation.CaptchaId) || string.IsNullOrWhiteSpace(loginInformation.CaptchaCode))
                    {
                        _logger.LogWarning(
                            "[LDAP LOGIN] BLOCKED - CAPTCHA required but not provided | Username: {Username} | IP: {ClientIP}",
                            loginInformation.Username, clientIp);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false,
                            "CAPTCHA required but not provided", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.CAPTCHA_REQUIRED,
                            "CAPTCHA verification is required. Please complete the CAPTCHA and try again.");
                    }

                    var captchaValid = _captchaService.ValidateCaptcha(loginInformation.CaptchaId, loginInformation.CaptchaCode);
                    if (!captchaValid)
                    {
                        _logger.LogWarning(
                            "[LDAP LOGIN] BLOCKED - Invalid CAPTCHA | Username: {Username} | CaptchaId: {CaptchaId} | IP: {ClientIP}",
                            loginInformation.Username, loginInformation.CaptchaId, clientIp);
                        await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false,
                            "Invalid CAPTCHA code", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.CAPTCHA_INVALID,
                            "CAPTCHA verification failed. Please try again.");
                    }

                    _logger.LogDebug("[LDAP LOGIN] CAPTCHA validated successfully | Username: {Username}", loginInformation.Username);
                }

                // Step 5: Extract username without domain for LDAP authentication
                string usernameForLdapAuth = loginInformation.Username.Trim();
                if (usernameForLdapAuth.Contains("@"))
                {
                    var parts = usernameForLdapAuth.Split('@');
                    if (parts.Length == 2)
                    {
                        usernameForLdapAuth = parts[0];
                        var providedDomain = parts[1];

                        // Validate that the provided domain matches the LDAP domain
                        if (!providedDomain.Equals(ldapSettings.LdapDomain, StringComparison.OrdinalIgnoreCase))
                        {
                            _logger.LogWarning(
                                "[LDAP LOGIN] FAILED - Domain mismatch | Username: {Username} | ProvidedDomain: {ProvidedDomain} | ExpectedDomain: {ExpectedDomain} | IP: {ClientIP}",
                                loginInformation.Username, providedDomain, ldapSettings.LdapDomain, clientIp);
                            await RecordLoginAttemptAsync(loginInformation.Username, null, false,
                                $"Domain mismatch. Expected domain: {ldapSettings.LdapDomain}", LoginType.LDAP, cancellationToken);
                            return APIOperationResponse<AuthenticatedResponse>.Fail(
                                ResponseType.Unauthorized,
                                CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                                "server.invalidLogin");
                        }
                    }
                }

                // Step 6: Authenticate against LDAP server
                _logger.LogDebug(
                    "[LDAP LOGIN] Authenticating with LDAP server | Username: {Username} | UsernameForAuth: {UsernameForAuth} | Server: {Server} | Domain: {Domain}",
                    loginInformation.Username, usernameForLdapAuth, ldapSettings.LdapServer, ldapSettings.LdapDomain);

                var loginSucceeded = await _ldapAuthenticator.ValidateAsync(
                    usernameForLdapAuth,
                    loginInformation.Password,
                    ldapSettings,
                    cancellationToken);

                if (!loginSucceeded)
                {
                    _logger.LogWarning(
                        "[LDAP LOGIN] FAILED - Invalid LDAP credentials | Username: {Username} | Server: {Server} | IP: {ClientIP}",
                        loginInformation.Username, ldapSettings.LdapServer, clientIp);

                    await RecordLoginAttemptAsync(loginInformation.Username.Trim(), null, false, "Invalid LDAP credentials", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                _logger.LogDebug(
                    "[LDAP LOGIN] LDAP authentication successful | Username: {Username}",
                    usernameForLdapAuth);

                // Step 7: Resolve username for database storage - check if domain is already included
                string resolvedUsername;
                string usernameWithoutDomain = usernameForLdapAuth;
                string originalUsername = loginInformation.Username.Trim();

                // Check if original username already ends with the LDAP domain
                var expectedDomainSuffix = $"@{ldapSettings.LdapDomain}";
                if (originalUsername.EndsWith(expectedDomainSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    // Username already contains the correct domain, use it as-is
                    resolvedUsername = originalUsername;
                    usernameWithoutDomain = originalUsername.Substring(0, originalUsername.Length - expectedDomainSuffix.Length);
                    _logger.LogDebug(
                        "[LDAP LOGIN] Username already contains domain | ResolvedUsername: {ResolvedUsername} | UsernameWithoutDomain: {UsernameWithoutDomain}",
                        resolvedUsername, usernameWithoutDomain);
                }
                else if (originalUsername.Contains("@"))
                {
                    // Username contains @ but with different domain - this should have been caught in Step 5
                    // But handle it defensively here as well
                    var parts = originalUsername.Split('@');
                    if (parts.Length == 2)
                    {
                        var providedDomain = parts[1];
                        _logger.LogWarning(
                            "[LDAP LOGIN] Domain mismatch detected in Step 7 | Username: {Username} | ProvidedDomain: {ProvidedDomain} | ExpectedDomain: {ExpectedDomain} | IP: {ClientIP}",
                            originalUsername, providedDomain, ldapSettings.LdapDomain, clientIp);
                        await RecordLoginAttemptAsync(originalUsername, null, false,
                            $"Domain mismatch. Expected domain: {ldapSettings.LdapDomain}", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Unauthorized,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "server.invalidLogin");
                    }
                    else
                    {
                        // Invalid format (multiple @ symbols)
                        _logger.LogWarning(
                            "[LDAP LOGIN] FAILED - Invalid username format | Username: {Username} | IP: {ClientIP}",
                            originalUsername, clientIp);
                        await RecordLoginAttemptAsync(originalUsername, null, false, "Invalid username format", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.BadRequest,
                            CommonErrorCodes.INVALID_USERNAME_FORMAT,
                            "Invalid username format. Please use username or username@domain.com");
                    }
                }
                else
                {
                    // No domain provided, add the LDAP domain
                    resolvedUsername = $"{usernameForLdapAuth}@{ldapSettings.LdapDomain}";
                    _logger.LogDebug(
                        "[LDAP LOGIN] Adding domain to username | Username: {Username} | Domain: {Domain}",
                        usernameForLdapAuth, ldapSettings.LdapDomain);
                }

                // Step 8: Look up or create local user
                _logger.LogDebug(
                    "[LDAP LOGIN] Looking up local user | ResolvedUsername: {ResolvedUsername}",
                    resolvedUsername);

                // Check if user exists (including soft-deleted) to provide proper error message
                var userIncludingDeleted = await _userRepository.Users
                    .FirstOrDefaultAsync(u => u.UserName == resolvedUsername, cancellationToken);

                if (userIncludingDeleted == null)
                {
                    userIncludingDeleted = await _context.Users
                        .FirstOrDefaultAsync(u => u.LdapUserName == usernameWithoutDomain, cancellationToken);
                }

                if (userIncludingDeleted != null && userIncludingDeleted.IsDeleted)
                {
                    _logger.LogWarning(
                        "[LDAP LOGIN] FAILED - User deleted | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                        resolvedUsername, userIncludingDeleted.Id, clientIp);
                    await RecordLoginAttemptAsync(resolvedUsername, userIncludingDeleted.Id, false,
                        "User account has been deleted", LoginType.LDAP, cancellationToken);
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Forbidden,
                        CommonErrorCodes.ACCOUNT_DELETED,
                        "server.accountDeleted");
                }

                var user = await _userRepository.Users
                    .FirstOrDefaultAsync(u => u.UserName == resolvedUsername && !u.IsDeleted, cancellationToken);

                if (user == null)
                {
                    user = await _context.Users
                        .FirstOrDefaultAsync(u => u.LdapUserName == usernameWithoutDomain && !u.IsDeleted, cancellationToken);
                }

                // Step 9: Create new user if not exists
                if (user == null)
                {
                    _logger.LogInformation(
                        "[LDAP LOGIN] Creating new local user | Username: {Username} | ResolvedUsername: {ResolvedUsername} | LdapUserName: {LdapUserName}",
                        loginInformation.Username, resolvedUsername, usernameWithoutDomain);

                    user = new ApplicationUser
                    {
                        UserName = resolvedUsername,
                        Email = resolvedUsername,
                        FullNameAR = resolvedUsername,
                        FullNameEN = resolvedUsername,
                        IsLdapUser = true,
                        LdapUserName = usernameWithoutDomain
                    };

                    await _userRepository.CreateAsync(user);

                    _logger.LogInformation(
                        "[LDAP LOGIN] User created successfully | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                        user.UserName, user.Id, clientIp);
                }
                else
                {
                    _logger.LogDebug(
                        "[LDAP LOGIN] Local user found | Username: {Username} | UserId: {UserId} | IsActive: {IsActive} | IsDeleted: {IsDeleted}",
                        user.UserName, user.Id, user.IsActive, user.IsDeleted);

                    // Check if existing user is soft-deleted (defensive check - should not happen due to filtering)
                    if (user.IsDeleted)
                    {
                        _logger.LogWarning(
                            "[LDAP LOGIN] FAILED - User deleted | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                            user.UserName, user.Id, clientIp);
                        await RecordLoginAttemptAsync(resolvedUsername, user.Id, false, "User account has been deleted", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Forbidden,
                            CommonErrorCodes.ACCOUNT_DELETED,
                            "server.accountDeleted");
                    }

                    // Check if LDAP user is active
                    if (!user.IsActive)
                    {
                        _logger.LogWarning(
                            "[LDAP LOGIN] FAILED - Account disabled | Username: {Username} | UserId: {UserId} | IP: {ClientIP}",
                            user.UserName, user.Id, clientIp);
                        await RecordLoginAttemptAsync(resolvedUsername, user.Id, false, "Account is disabled", LoginType.LDAP, cancellationToken);
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Forbidden,
                            CommonErrorCodes.ACCOUNT_DISABLED,
                            "Your account has been disabled. Please contact your administrator.");
                    }
                }

                // Step 10: Generate authentication response
                _logger.LogDebug("[LDAP LOGIN] Generating auth response | UserId: {UserId}", user.Id);
                var response = await CreateAndReturnAuthResponseAsync(user, cancellationToken);

                // Record successful login
                await RecordLoginAttemptAsync(resolvedUsername, user.Id, true, null, LoginType.LDAP, cancellationToken);

                var duration = (_dateTimeProvider.Now - startTime).TotalMilliseconds;
                _logger.LogInformation(
                    "[LDAP LOGIN] SUCCESS | Username: {Username} | UserId: {UserId} | IP: {ClientIP} | Domain: {Domain} | Duration: {Duration}ms",
                    resolvedUsername, user.Id, clientIp, ldapSettings.LdapDomain, duration);

                return APIOperationResponse<AuthenticatedResponse>.Success(response);
            }
            catch (Exception ex)
            {
                var duration = (_dateTimeProvider.Now - startTime).TotalMilliseconds;
                _logger.LogError(ex,
                    "[LDAP LOGIN] ERROR - Exception occurred | Username: {Username} | IP: {ClientIP} | Duration: {Duration}ms | Error: {ErrorMessage}",
                    loginInformation?.Username ?? "N/A", clientIp, duration, ex.Message);

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

        public async Task<APIOperationResponse<AuthenticatedResponse>> RefreshTokenFromCookieAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.UN_AUTHORIZED,
                        "server.invalidRefreshRequest");
                }

                var user = await _userRepository.Users
                    .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && !u.IsDeleted, cancellationToken);
                if (user == null)
                {
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.UN_AUTHORIZED,
                        "server.invalidRefreshToken");
                }

                var authResponse = await _jwtServices.RefreshAsync(new UserRefreshToken(user.Id, refreshToken));
                return APIOperationResponse<AuthenticatedResponse>.Success(authResponse);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning(ex, "Refresh token validation failed: {Message}", ex.Message);
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.Unauthorized,
                    CommonErrorCodes.UN_AUTHORIZED,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during token refresh");
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    "server.unableToRefreshToken");
            }
        }

        private async Task<AuthenticatedResponse> CreateAndReturnAuthResponseAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var refreshToken = _jwtServices.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryDate = _dateTimeProvider.Now.AddMinutes(_jwtOptions.RefreshTokenExpireInMinutes);


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
                return APIOperationResponse<List<ClaimDto>>.Success(roleClaims);

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

                // Blacklist the current JWT token
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    try
                    {
                        // Extract JWT token from Authorization header
                        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            var token = authHeader.Substring("Bearer ".Length).Trim();

                            // Extract jti claim from the token
                            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                            if (handler.CanReadToken(token))
                            {
                                var jwtToken = handler.ReadJwtToken(token);
                                var jtiClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti);

                                if (jtiClaim != null && !string.IsNullOrWhiteSpace(jtiClaim.Value))
                                {
                                    // Get token expiration
                                    var expirationClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Exp);
                                    DateTime expiresAt = _dateTimeProvider.Now.AddMinutes(_jwtOptions.AccessTokenExpireInMinutes);

                                    if (expirationClaim != null && long.TryParse(expirationClaim.Value, out long exp))
                                    {
                                        expiresAt = DateTimeOffset.FromUnixTimeSeconds(exp).LocalDateTime;
                                    }

                                    // Add token to blacklist
                                    await _tokenBlacklistService.BlacklistTokenAsync(
                                        jtiClaim.Value,
                                        userId,
                                        expiresAt,
                                        "User logout",
                                        cancellationToken);

                                    _logger.LogInformation("JWT token blacklisted on logout. TokenId: {TokenId}, UserId: {UserId}",
                                        jtiClaim.Value, userId);
                                }
                                else
                                {
                                    _logger.LogWarning("JWT token does not contain jti claim. UserId: {UserId}", userId);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log but don't fail logout if token blacklisting fails
                        _logger.LogWarning(ex, "Failed to blacklist JWT token on logout. UserId: {UserId}", userId);
                    }
                }

                // Clear all tokens and set logout timestamp
                var hadRefreshToken = !string.IsNullOrEmpty(user.RefreshToken);
                user.RefreshToken = null;
                user.RefreshTokenExpiryDate = null;

                await _userRepository.UpdateAsync(user);

                // Clear refresh token cookie
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
        /// Only counts failed attempts that occurred AFTER the most recent successful login
        /// This ensures that after successful login, the failed attempts count resets
        /// </summary>
        private async Task<bool> IsAccountLockedAsync(string username, CancellationToken cancellationToken)
        {
            var lockoutThreshold = _dateTimeProvider.Now.AddMinutes(-LOCKOUT_DURATION_MINUTES);
            
            // Find the most recent successful login for this username
            var mostRecentSuccessfulLogin = await _context.LoginAttempts
                .Where(la => la.Username == username && la.IsSuccessful)
                .OrderByDescending(la => la.AttemptDate)
                .FirstOrDefaultAsync(cancellationToken);

            // Only count failed attempts that occurred:
            // 1. After the most recent successful login (if one exists), OR
            // 2. Within the lockout window (if no successful login exists)
            DateTime? countFromDate = mostRecentSuccessfulLogin?.AttemptDate;
            var effectiveThreshold = countFromDate.HasValue && countFromDate.Value > lockoutThreshold
                ? countFromDate.Value
                : lockoutThreshold;

            var failedAttempts = await _context.LoginAttempts
                .Where(la => la.Username == username 
                    && !la.IsSuccessful 
                    && la.AttemptDate >= effectiveThreshold)
                .CountAsync(cancellationToken);

            return failedAttempts >= MAX_FAILED_ATTEMPTS;
        }

        /// <summary>
        /// Checks if CAPTCHA is required (3+ failed attempts but account not locked yet)
        /// Only counts failed attempts that occurred AFTER the most recent successful login
        /// This ensures that after successful login, the failed attempts count resets
        /// </summary>
        private async Task<bool> IsCaptchaRequiredAsync(string username, CancellationToken cancellationToken)
        {
            var lockoutThreshold = _dateTimeProvider.Now.AddMinutes(-LOCKOUT_DURATION_MINUTES);
            
            // Find the most recent successful login for this username
            var mostRecentSuccessfulLogin = await _context.LoginAttempts
                .Where(la => la.Username == username && la.IsSuccessful)
                .OrderByDescending(la => la.AttemptDate)
                .FirstOrDefaultAsync(cancellationToken);

            // Only count failed attempts that occurred:
            // 1. After the most recent successful login (if one exists), OR
            // 2. Within the lockout window (if no successful login exists)
            DateTime? countFromDate = mostRecentSuccessfulLogin?.AttemptDate;
            var effectiveThreshold = countFromDate.HasValue && countFromDate.Value > lockoutThreshold
                ? countFromDate.Value
                : lockoutThreshold;

            // Get failed attempts after the most recent successful login (or within lockout window)
            var failedAttempts = await _context.LoginAttempts
                .Where(la => la.Username == username 
                    && !la.IsSuccessful 
                    && la.AttemptDate >= effectiveThreshold)
                .CountAsync(cancellationToken);

            // If no failed attempts, no CAPTCHA required
            if (failedAttempts == 0)
            {
                return false;
            }

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
                    AttemptDate = _dateTimeProvider.Now
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
