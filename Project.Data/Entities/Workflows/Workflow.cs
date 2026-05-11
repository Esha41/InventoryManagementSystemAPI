using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Workflows
{
    public class Workflow : AuditEntity<long>
    {
     
        [Required]
        [MaxLength(100)]
        public string WorkflowName { get; set; }

        [Required]
        public WorkflowType WorkflowType { get; set; } 

        [Required]
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public bool IsSpecialOrReserved { get; set; } = false;

        // Navigation properties
        public virtual ICollection<WorkflowStep> WorkflowSteps { get; set; }
        public virtual WorkflowAutoRejectTrigger? AutoRejectTrigger { get; set; }
    }
}
