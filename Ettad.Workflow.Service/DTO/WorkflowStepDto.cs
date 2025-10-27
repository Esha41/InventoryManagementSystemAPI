using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.DTO
{
    public class WorkflowStepDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public int StepOrder { get; set; }
        public ApproverType? ApproverType { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public bool MustApprove { get; set; }


        public virtual ICollection<WorkflowApprovalHistoryDto> ApprovalHistories { get; set; }
    }
    public class WorkflowStepCreateDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public int StepOrder { get; set; }
        public ApproverType? ApproverType { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public bool MustApprove { get; set; }


    }


}
