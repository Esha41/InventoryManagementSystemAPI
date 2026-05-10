using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Dtos
{
    public class WorkflowDto
    {
        public long Id { get; set; }

        public string WorkflowName { get; set; }

       
        public WorkflowType WorkflowType { get; set; }
        public string WorkflowTypeName { get; set; } // Name from table

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSpecialOrReserved { get; set; }

        [JsonPropertyName("autoRejectTriggerMode")]
        public string? AutoRejectTriggerMode { get; set; }

        [JsonPropertyName("autoRejectTriggerRoleIds")]
        public List<string> AutoRejectTriggerRoleIds { get; set; } = new();

        [JsonPropertyName("autoRejectTriggerStepIds")]
        public List<long> AutoRejectTriggerStepIds { get; set; } = new();

        [JsonPropertyName("autoRejectResetOnReApproval")]
        public bool AutoRejectResetOnReApproval { get; set; } = true;

        // Steps in the workflow
        public List<WorkflowStepDto> WorkflowSteps { get; set; } = new();
    }



}
