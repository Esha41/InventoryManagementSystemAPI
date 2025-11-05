using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.DTO
{
    public class WorkflowDto
    {
        public int Id { get; set; }

        public string WorkflowName { get; set; }

       
        public int WorkflowType { get; set; }
        public string WorkflowTypeName { get; set; } // Name from table

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSpecialOrReserved { get; set; }

        // Steps in the workflow
        public List<WorkflowStepDto> WorkflowSteps { get; set; } = new();
    }



}
