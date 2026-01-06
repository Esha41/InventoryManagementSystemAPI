using Ettad.Data.Entities;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.User.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.User.Services.Implementation
{
    /// <summary>
    /// Service for managing blacklisted JWT tokens
    /// </summary>
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TokenBlacklistService> _logger;

        public TokenBlacklistService(
            ApplicationDbContext context,
            ILogger<TokenBlacklistService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a token to the blacklist
        /// </summary>
        public async Task BlacklistTokenAsync(
            string tokenId, 
            string userId, 
            DateTime expiresAt, 
            string reason = "User logout", 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tokenId))
                throw new ArgumentException("Token ID cannot be null or empty", nameof(tokenId));
            
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            try
            {
                // Check if token is already blacklisted
                var existingToken = await _context.BlacklistedTokens
                    .FirstOrDefaultAsync(bt => bt.TokenId == tokenId, cancellationToken);

                if (existingToken != null)
                {
                    _logger.LogInformation("Token {TokenId} is already blacklisted", tokenId);
                    return;
                }

                var blacklistedToken = new BlacklistedToken
                {
                    TokenId = tokenId,
                    UserId = userId,
                    BlacklistedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    Reason = reason
                };

                _context.BlacklistedTokens.Add(blacklistedToken);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Token blacklisted successfully. TokenId: {TokenId}, UserId: {UserId}, Reason: {Reason}", 
                    tokenId, userId, reason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error blacklisting token. TokenId: {TokenId}, UserId: {UserId}", 
                    tokenId, userId);
                throw;
            }
        }

        /// <summary>
        /// Checks if a token is blacklisted
        /// </summary>
        public async Task<bool> IsTokenBlacklistedAsync(string tokenId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tokenId))
                return false;

            try
            {
                var isBlacklisted = await _context.BlacklistedTokens
                    .AnyAsync(bt => bt.TokenId == tokenId && bt.ExpiresAt > DateTime.UtcNow, cancellationToken);

                if (isBlacklisted)
                {
                    _logger.LogWarning("Blacklisted token detected: {TokenId}", tokenId);
                }

                return isBlacklisted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if token is blacklisted. TokenId: {TokenId}", tokenId);
                // In case of error, allow the token (fail open) - better than blocking all users
                // The token will still be validated by standard JWT validation
                return false;
            }
        }

        /// <summary>
        /// Removes expired tokens from the blacklist
        /// This should be called periodically to clean up old entries
        /// </summary>
        public async Task CleanupExpiredTokensAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var now = DateTime.UtcNow;
                var expiredTokens = await _context.BlacklistedTokens
                    .Where(bt => bt.ExpiresAt <= now)
                    .ToListAsync(cancellationToken);

                if (expiredTokens.Any())
                {
                    _context.BlacklistedTokens.RemoveRange(expiredTokens);
                    await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Cleaned up {Count} expired blacklisted tokens", 
                        expiredTokens.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up expired blacklisted tokens");
                throw;
            }
        }
    }
}

