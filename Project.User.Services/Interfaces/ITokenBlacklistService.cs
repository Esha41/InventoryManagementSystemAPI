using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    /// <summary>
    /// Service for managing blacklisted JWT tokens
    /// </summary>
    public interface ITokenBlacklistService
    {
        /// <summary>
        /// Adds a token to the blacklist
        /// </summary>
        /// <param name="tokenId">The unique JWT ID (jti claim)</param>
        /// <param name="userId">The user who owned the token</param>
        /// <param name="expiresAt">When the token naturally expires</param>
        /// <param name="reason">Reason for blacklisting (e.g., "User logout")</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task BlacklistTokenAsync(string tokenId, string userId, DateTime expiresAt, string reason = "User logout", CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Checks if a token is blacklisted
        /// </summary>
        /// <param name="tokenId">The unique JWT ID (jti claim) to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the token is blacklisted, false otherwise</returns>
        Task<bool> IsTokenBlacklistedAsync(string tokenId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Removes expired tokens from the blacklist
        /// This should be called periodically to clean up old entries
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task CleanupExpiredTokensAsync(CancellationToken cancellationToken = default);
    }
}

