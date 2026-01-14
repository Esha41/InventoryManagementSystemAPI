using Ettad.Comman.Idenitity;
using Ettad.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ettad.Data.Entities
{
    public class LoginAttempt
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string? UserId { get; set; }
        public bool IsSuccessful { get; set; }
        public string? FailureReason { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime AttemptDate { get; set; }
        public LoginType LoginType { get; set; }

        // Navigation property
        public ApplicationUser? User { get; set; }
    }
}

