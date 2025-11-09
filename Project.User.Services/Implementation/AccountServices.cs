using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Exception;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.IGenericRepository_IUOW;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Services.Helpers;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Helpers;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

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
        private readonly ApplicationDbContext _context;
        public AccountServices(
            IJwtServices jwtServices,
            ISettingsProvider settingsProvider,
//IUnitOfWork unitOfWork,
            ILdapAuthenticator ldapAuthenticator,
            IDateTimeProvider dateTimeProvider,
            IOptions<JwtOptions> jwtOptions,
            IOptions<AdminUsersOptions> adminUsers, UserManager<ApplicationUser> userRepository,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUserService , IEmailSender emailSender,ApplicationDbContext context)
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
        }

        public async Task<APIOperationResponse<AuthenticatedResponse>> Login(
      LoginInformation loginInformation,
      CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginInformation?.Username))
                {
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                var ldapSettings = await _settingsProvider.GetLdapSettings(cancellationToken);

                var isAdminLogin = true;
                    //_adminUsers.AdminUserNames
                   // .Any(a => loginInformation.Username.Contains(a, StringComparison.OrdinalIgnoreCase));

                ApplicationUser? user;

                if (isAdminLogin)
                {
                    user = await _userRepository.FindByNameAsync(loginInformation.Username.Trim());
                    if (user == null)
                    {
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Unauthorized,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "server.invalidLogin");
                    }

                    var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginInformation.Password, lockoutOnFailure: false);
                    if (!signInResult.Succeeded)
                    {

                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Unauthorized,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "server.invalidLogin");
                    }

                }
                else if (ldapSettings.IsActive)
                {
                    var loginSucceeded = await _ldapAuthenticator.ValidateAsync(
                        loginInformation.Username.Trim(),
                        loginInformation.Password,
                        loginWithoutPassword: false,
                        cancellationToken);

                    if (!loginSucceeded)
                    {
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Unauthorized,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "server.invalidLogin");
                    }

                    var resolvedUsername = $"{loginInformation.Username.Trim()}@{ldapSettings.LdapDomain}";
                    user = await _userRepository.FindByNameAsync(resolvedUsername);
                    if (user == null)
                    {
                        return APIOperationResponse<AuthenticatedResponse>.Fail(
                            ResponseType.Unauthorized,
                            CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                            "server.invalidLogin");
                    }
                }
                else
                {
                    return APIOperationResponse<AuthenticatedResponse>.Fail(
                        ResponseType.Unauthorized,
                        CommonErrorCodes.INVALID_EMAIL_OR_PASSWORD,
                        "server.invalidLogin");
                }

                var authResponse = await CreateAndReturnAuthResponseAsync(user, cancellationToken);
                return APIOperationResponse<AuthenticatedResponse>.Success(authResponse);
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                return APIOperationResponse<AuthenticatedResponse>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.SERVER_ERROR,
                    ex.Message);
            }
        }


        public async Task<AuthenticatedResponse> LoginWithLdap(LoginInformation loginInformation, CancellationToken cancellationToken = default)
        {
            var ldapSettings = await _settingsProvider.GetLdapSettings(cancellationToken);
            if (!ldapSettings.IsActive) throw new ApiException("server.invalidLdapSettings");
            if (string.IsNullOrWhiteSpace(loginInformation?.Username)) throw new ApiException("server.invalidLogin");

            var loginSucceeded = await _ldapAuthenticator.ValidateAsync(loginInformation.Username.Trim(), loginInformation.Password, loginWithoutPassword: true, cancellationToken);
            if (!loginSucceeded) throw new ApiException("server.invalidLogin");

            var resolvedUsername = $"{loginInformation.Username.Trim()}@{ldapSettings.LdapDomain}";
            var user = await _userRepository.FindByNameAsync(resolvedUsername)
                       ?? throw new ApiException("server.invalidLogin");
            

            return await CreateAndReturnAuthResponseAsync(user, cancellationToken);
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

            var departmentName = string.Empty;
            if (user.DepartmentId.HasValue)
            {
                departmentName = await _context.Departments
                    .Where(d => d.Id == user.DepartmentId.Value)
                    .Select(d => d.NameEn ?? d.NameAr ?? string.Empty)
                    .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;
            }

            var authResponse = await _jwtServices.GenerateJWTokenAsync(user.Id);
            authResponse.RefreshToken = refreshToken;
            authResponse.DepartmentId = user.DepartmentId;
            authResponse.DepartmentName = string.IsNullOrWhiteSpace(departmentName) ? null : departmentName;
            authResponse.EmployeeId = user.EmployeeId;
            authResponse.OrganizationId = user.OrganizationId;
            authResponse.UserName = user.UserName;
            authResponse.NameEn = user.FullNameEN;
            authResponse.NameAr = user.FullNameAR;
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
            var user = await _userRepository.FindByEmailAsync(request.Email.Trim());
            if (user == null)
                return APIOperationResponse<string>.Fail(ResponseType.NotFound, "User not found");

            var token = await _userRepository.GeneratePasswordResetTokenAsync(user);

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

            await _emailSender.SendEmailAsync(user.Email, "Reset Password", emailBody); ;
            return APIOperationResponse<string>.Success(token);
        }

        public async Task<APIOperationResponse<string>> ResetPasswordAsync(ResetPasswordDto request)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null)
                return APIOperationResponse<string>.Fail(ResponseType.NotFound, "User not found");

            var result = await _userRepository.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return APIOperationResponse<string>.Fail(ResponseType.BadRequest, errors);
            }

            return APIOperationResponse<string>.Success("Password has been reset successfully.");
        }

    }
}
