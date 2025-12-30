namespace Ettad.User.Services.Interfaces
{
    /// <summary>
    /// Interface for custom CAPTCHA generation and validation service
    /// </summary>
    public interface ICaptchaService
    {
        /// <summary>
        /// Generates a new CAPTCHA code and returns it along with a unique identifier
        /// </summary>
        /// <returns>Tuple containing captchaId and captchaCode</returns>
        (string CaptchaId, string CaptchaCode) GenerateCaptcha();

        /// <summary>
        /// Validates a CAPTCHA code against the stored value
        /// </summary>
        /// <param name="captchaId">The unique identifier for the CAPTCHA</param>
        /// <param name="userInput">The user's input to validate</param>
        /// <returns>True if CAPTCHA is valid, false otherwise</returns>
        bool ValidateCaptcha(string? captchaId, string? userInput);
    }
}

