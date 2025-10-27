using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.DTO
{
    public class WorkflowApprovalHistoryDto
    {
        public int Id { get; set; }
        public int WorkflowStepId { get; set; }
        public int TargetRequestId { get; set; }
        public WorkflowType RequestType { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public int IsDelagation { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public ApprovalStatus Status { get; set; }
        public string Comments { get; set; }
  

        //public virtual WorkflowStepDto WorkflowStep { get; set; }
    }

}
