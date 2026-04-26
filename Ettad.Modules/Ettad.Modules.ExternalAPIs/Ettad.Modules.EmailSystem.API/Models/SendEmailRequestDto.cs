using System.ComponentModel.DataAnnotations;

namespace Ettad.Modules.EmailSystem.API.Models
{
    public class SendEmailRequestDto
    {
        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public bool IsHtml { get; set; } = true;
    }
}

