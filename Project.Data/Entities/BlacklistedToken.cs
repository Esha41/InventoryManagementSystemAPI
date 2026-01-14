using System;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Represents a JWT token that has been invalidated (e.g., after user logout)
    /// Tokens are stored here until their expiration time passes
    /// </summary>
    public class BlacklistedToken
    {
        public long Id { get; set; }
        
        /// <summary>
        /// The unique JWT ID (jti claim) from the token
        /// </summary>
        public string TokenId { get; set; }
        
        /// <summary>
        /// The user ID who owned this token
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// When the token was blacklisted (typically on logout)
        /// </summary>
        public DateTime BlacklistedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// When the token expires - used for automatic cleanup
        /// After this time, the token is naturally invalid and doesn't need to be checked
        /// </summary>
        public DateTime ExpiresAt { get; set; }
        
        /// <summary>
        /// Reason for blacklisting (e.g., "User logout", "Security: Password changed")
        /// </summary>
        public string? Reason { get; set; }
    }
}

