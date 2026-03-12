using MediatR;

namespace Ettad.User.Services.Events
{
    /// <summary>
    /// Raised when a user successfully logs out.
    /// Subscribers can perform session cleanup (e.g. clear announcement dismissals).
    /// </summary>
    public class UserLoggedOutEvent : INotification
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
