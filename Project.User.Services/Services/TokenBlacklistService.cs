using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Ettad.User.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.User.Services.Services
{
    /// <summary>
    /// Service for managing blacklisted JWT tokens
    /// </summary>
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly ICrossCuttingRepository<BlacklistedToken> _blacklistedTokenRepository;
        private readonly ILogger<TokenBlacklistService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public TokenBlacklistService(
            ICrossCuttingRepository<BlacklistedToken> blacklistedTokenRepository,
            ILogger<TokenBlacklistService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _blacklistedTokenRepository = blacklistedTokenRepository ?? throw new ArgumentNullException(nameof(blacklistedTokenRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
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
                var existingToken = await _blacklistedTokenRepository.FindOneAsync(bt => bt.TokenId == tokenId);

                if (existingToken != null)
                {
                    _logger.LogInformation("Token {TokenId} is already blacklisted", tokenId);
                    return;
                }

                var blacklistedToken = new BlacklistedToken
                {
                    TokenId = tokenId,
                    UserId = userId,
                    BlacklistedAt = _dateTimeProvider.Now,
                    ExpiresAt = expiresAt,
                    Reason = reason
                };

                await _blacklistedTokenRepository.AddAsync(blacklistedToken);

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
                var now = _dateTimeProvider.Now;
                var isBlacklisted = await _blacklistedTokenRepository
                    .Find(bt => bt.TokenId == tokenId && bt.ExpiresAt > now)
                    .AnyAsync(cancellationToken);

                if (isBlacklisted)
                {
                    _logger.LogWarning("Blacklisted token detected: {TokenId}", tokenId);
                }

                return isBlacklisted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if token is blacklisted. TokenId: {TokenId}", tokenId);
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
                var now = _dateTimeProvider.Now;
                var expiredTokens = await _blacklistedTokenRepository
                    .Find(bt => bt.ExpiresAt <= now)
                    .ToListAsync(cancellationToken);

                if (expiredTokens.Any())
                {
                    foreach (var token in expiredTokens)
                    {
                        await _blacklistedTokenRepository.DeleteAsync(token);
                    }

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
