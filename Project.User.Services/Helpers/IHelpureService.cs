using Ettad.Application.Common.Models;
using System.Security.Claims;

namespace Ettad.Services.Helpers
{
    public interface IHelpureService
    {
        public Task<string> GetUserAsync(ClaimsPrincipal user);
        public Task<EmailConfiguration?> GetEmailConfigrationAsync(int organizationId);

    }
}
