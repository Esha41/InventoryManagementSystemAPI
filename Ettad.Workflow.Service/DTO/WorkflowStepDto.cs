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

        // Replaced ApproverType / ApproverEmployeeId
        public string ApplicationRoleId { get; set; }          // required role
        public string ApplicationRoleName { get; set; }        // role name for display
        public long ApplicationEntityId { get; set; }             // required role for this step
            // entity being approved

        public bool MustApprove { get; set; } = true;

        public bool RequireHigherApproval { get; set; } = false;
        public string? HigherApprovalRoleId { get; set; }

        public long? HigherApplicationEntityId { get; set; }

        public bool ReserveQty { get; set; } = false;

        public bool CanSkip { get; set; } = false;
        public List<int> AllowedSkipTargetIds { get; set; } = new List<int>();

        // Approval steps for this workflow step
        public List<WorkflowApprovalStepDto> ApprovalSteps { get; set; } = new();
    }

    public class WorkflowStepCreateDto
    {
        public int Id { get; set; }

        public int WorkflowId { get; set; }

        public int StepOrder { get; set; }

        // Instead of ApproverType / ApproverEmployeeId
        public string ApplicationRoleId { get; set; }  // required role for this step

        public int ApplicationEntityId { get; set; }   // reference to the entity being approved

        public bool MustApprove { get; set; } = true;

        public bool RequireHigherApproval { get; set; } = false;

        public string? HigherApprovalRoleId { get; set; }

        public long? HigherApplicationEntityId { get; set; }

        public bool ReserveQty { get; set; } = false;
    }



}
