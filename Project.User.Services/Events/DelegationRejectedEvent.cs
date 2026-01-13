using MediatR;

namespace Ettad.User.Services.Events
{
    public class DelegationRejectedEvent : INotification
    {
        public int DelegationId { get; set; }
        public string DelegatorUserId { get; set; }
        public string DelegateeUserId { get; set; }
        public string DelegateeName { get; set; }
    }
}
