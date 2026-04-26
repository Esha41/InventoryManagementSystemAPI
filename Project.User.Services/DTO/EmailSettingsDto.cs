using System.ComponentModel.DataAnnotations;

namespace Ettad.User.Services.DTO
{
    public class EmailSettingsDto
    {
        public bool EnableEmailNotifications { get; set; }
        
        [Required(ErrorMessage = "Host is required")]
        public string Host { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Port is required")]
        public int Port { get; set; }
        
        public bool EnableSSL { get; set; }
        
        [Required(ErrorMessage = "Sender Name is required")]
        public string SenderName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Account Username is required")]
        public string AccountUsername { get; set; } = string.Empty;

    
        public string? AccountPassword { get; set; }
    }
}

