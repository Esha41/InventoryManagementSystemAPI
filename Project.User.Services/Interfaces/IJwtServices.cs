using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    public interface IJwtServices
    {
        string GenerateRefreshToken();
        Task<AuthenticatedResponse> GenerateJWTokenAsync(string userId);
        Task<AuthenticatedResponse> GenerateAzureJWTokenAsync(LoginWithAzureInformation information);
        Task<AuthenticatedResponse> RefreshAsync(UserRefreshToken userRefreshToken);
    }

}
