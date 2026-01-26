namespace Ettad.Application.Common.Interfaces;

using System.Threading.Tasks;

public interface IDelegationAuthorizationService
{
    Task<bool> IsUserRestrictedAsync(string userId);
}
