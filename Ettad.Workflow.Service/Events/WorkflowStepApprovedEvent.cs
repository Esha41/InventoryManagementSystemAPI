using Ettad.Data.Enums;
using MediatR;

namespace Ettad.Workflows.Service.Events
{
    public class WorkflowStepApprovedEvent : INotification
    {
        public int WorkflowApprovalStepId { get; set; }
        public int WorkflowStepId { get; set; }
        public long TargetRequestId { get; set; }
        public RequestType RequestType { get; set; }
        public string ApproverUserId { get; set; }
        public string ApplicationRoleId { get; set; }
        public string ApplicationRoleName { get; set; }
    }
}

