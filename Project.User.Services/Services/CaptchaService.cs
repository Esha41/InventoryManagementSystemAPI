using Ettad.User.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Ettad.User.Services.Services
{
    /// <summary>
    /// Custom CAPTCHA service that generates and validates text-based CAPTCHA codes
    /// </summary>
    public class CaptchaService : ICaptchaService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CaptchaService> _logger;
        private const int CAPTCHA_LENGTH = 5;
        private const int CAPTCHA_EXPIRATION_MINUTES = 5;
        private const string CAPTCHA_CACHE_PREFIX = "Captcha_";
        
        // Characters to use in CAPTCHA (excluding confusing characters like 0, O, I, l)
        private static readonly char[] CAPTCHA_CHARS = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
        private static readonly Random _random = new Random();

        public CaptchaService(
            IMemoryCache memoryCache,
            ILogger<CaptchaService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public (string CaptchaId, string CaptchaCode) GenerateCaptcha()
        {
            // Generate a unique ID for this CAPTCHA
            var captchaId = Guid.NewGuid().ToString("N");

            // Generate random CAPTCHA code
            var captchaCode = GenerateRandomCode(CAPTCHA_LENGTH);

            // Store in memory cache with expiration
            var cacheKey = $"{CAPTCHA_CACHE_PREFIX}{captchaId}";
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CAPTCHA_EXPIRATION_MINUTES),
                SlidingExpiration = null // Don't extend expiration on access
            };

            _memoryCache.Set(cacheKey, captchaCode, cacheOptions);

            _logger.LogDebug("Generated CAPTCHA with ID: {CaptchaId}", captchaId);

            return (captchaId, captchaCode);
        }

        public bool ValidateCaptcha(string captchaId, string userInput)
        {
            // If no ID or input provided, validation fails
            if (string.IsNullOrWhiteSpace(captchaId) || string.IsNullOrWhiteSpace(userInput))
            {
                _logger.LogWarning("CAPTCHA validation failed - missing captchaId or userInput");
                return false;
            }

            // Retrieve stored CAPTCHA code from cache
            var cacheKey = $"{CAPTCHA_CACHE_PREFIX}{captchaId}";
            if (!_memoryCache.TryGetValue(cacheKey, out string storedCode))
            {
                _logger.LogWarning("CAPTCHA validation failed - CAPTCHA not found or expired. CaptchaId: {CaptchaId}", captchaId);
                return false;
            }

            // Compare user input with stored code (case-insensitive, trim whitespace)
            var isValid = string.Equals(
                storedCode?.Trim(), 
                userInput.Trim(), 
                StringComparison.OrdinalIgnoreCase);

            if (isValid)
            {
                // Remove CAPTCHA from cache after successful validation (one-time use)
                _memoryCache.Remove(cacheKey);
                _logger.LogDebug("CAPTCHA validation successful. CaptchaId: {CaptchaId}", captchaId);
            }
            else
            {
                _logger.LogWarning("CAPTCHA validation failed - code mismatch. CaptchaId: {CaptchaId}", captchaId);
            }

            return isValid;
        }

        /// <summary>
        /// Generates a random CAPTCHA code using the allowed characters
        /// </summary>
        private string GenerateRandomCode(int length)
        {
            var code = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                var randomIndex = _random.Next(CAPTCHA_CHARS.Length);
                code.Append(CAPTCHA_CHARS[randomIndex]);
            }
            return code.ToString();
        }
    }
}

