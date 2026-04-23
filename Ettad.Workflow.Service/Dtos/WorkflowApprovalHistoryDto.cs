using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Dtos
{
    public class WorkflowApprovalHistoryDto
    {
        public int Id { get; set; }

        // Reference to parent workflow step
        public int WorkflowStepId { get; set; }

        public RequestStatus OldRequestStatus { get; set; }

        public RequestStatus NewRequestStatus { get; set; }

        public string? Comments { get; set; }

        public string? ChangedBy { get; set; }

        public DateTime ChangedAt { get; set; }
    }
}
