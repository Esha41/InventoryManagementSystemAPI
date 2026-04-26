using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Ettad.User.Services.Services
{
    public class OnboardingService : IOnboardingService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<OnboardingService> _logger;

        public OnboardingService(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            ILogger<OnboardingService> logger)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<OnboardingStatusDto>> GetStatusAsync()
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return APIOperationResponse<OnboardingStatusDto>.Fail(
                    ResponseType.Unauthorized, "Current user context not found.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return APIOperationResponse<OnboardingStatusDto>.Fail(
                    ResponseType.NotFound, "User not found.");
            }

            return APIOperationResponse<OnboardingStatusDto>.Success(
                new OnboardingStatusDto(user.IsOnboardingCompleted));
        }

        public async Task<APIOperationResponse<bool>> CompleteAsync()
        {
            var userId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return APIOperationResponse<bool>.Fail(
                    ResponseType.Unauthorized, "Current user context not found.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return APIOperationResponse<bool>.Fail(
                    ResponseType.NotFound, "User not found.");
            }

            if (!user.IsOnboardingCompleted)
            {
                user.IsOnboardingCompleted = true;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to mark onboarding complete for user {UserId}", userId);
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.InternalServerError, "Failed to update onboarding status.");
                }
            }

            return APIOperationResponse<bool>.Success(true);
        }
    }
}
