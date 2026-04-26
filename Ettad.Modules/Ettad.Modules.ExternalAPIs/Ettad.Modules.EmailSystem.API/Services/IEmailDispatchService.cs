using Ettad.Modules.EmailSystem.API.Models;
using Ettad.ResponseHandler.Models;

namespace Ettad.Modules.EmailSystem.API.Services
{
    public interface IEmailDispatchService
    {
        Task<APIOperationResponse<bool>> SendAsync(SendEmailRequestDto request, CancellationToken cancellationToken = default);
    }
}

