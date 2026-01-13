using MediatR;

namespace Ettad.User.Services.Events
{
    public class DelegationCreatedEvent : INotification
    {
        public int DelegationId { get; set; }
        public string DelegatorUserId { get; set; }
        public string DelegateeUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}
