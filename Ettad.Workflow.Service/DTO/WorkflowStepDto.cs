using Ettad.Data.Enums;
using Ettad.User.Services.DTO;
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
        public string ApplicationRoleName { get; set; }        // role name for display (English)
        public string? ApplicationRoleNameAr { get; set; }    // role name for display (Arabic)
        public long ApplicationEntityId { get; set; }             // required role for this step
            // entity being approved

        public bool MustApprove { get; set; } = true;

        public bool RequireHigherApproval { get; set; } = false;
        public string? HigherApprovalRoleId { get; set; }

        public long? HigherApplicationEntityId { get; set; }

        public bool ReserveQty { get; set; } = false;

        public bool CanSkip { get; set; } = false;
        public bool CanReturn { get; set; } = false;
        public List<WorkflowStepTransitionDto> Transitions { get; set; } = new List<WorkflowStepTransitionDto>();

        // Approval steps for this workflow step
        public List<WorkflowApprovalStepDto> ApprovalSteps { get; set; } = new();
    }

    public class WorkflowStepTransitionDto
    {
        public int Id { get; set; }
        public int SourceWorkflowStepId { get; set; }
        public int TargetWorkflowStepId { get; set; }
        
        // Target step details
        public TargetStepDetailsDto TargetStep { get; set; }
    }

    public class TargetStepDetailsDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public int StepOrder { get; set; }
        
        // Role information
        public RoleDto ApplicationRole { get; set; }
        public long ApplicationEntityId { get; set; }
        
        // Higher approval role information
        public bool RequireHigherApproval { get; set; }
        public RoleDto? HigherApprovalRole { get; set; }
        public long? HigherApplicationEntityId { get; set; }
        
        public bool MustApprove { get; set; }
        public bool ReserveQty { get; set; }
        public bool CanSkip { get; set; }
        public bool CanReturn { get; set; }
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

        public bool CanReturn { get; set; } = false;
    }



}
