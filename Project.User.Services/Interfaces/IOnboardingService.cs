using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;

namespace Ettad.User.Services.Interfaces
{
    public interface IOnboardingService
    {
        Task<APIOperationResponse<OnboardingStatusDto>> GetStatusAsync();
        Task<APIOperationResponse<bool>> CompleteAsync();
    }
}
