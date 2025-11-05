using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Workflows
{
    public class Workflow : AuditEntity<int>
    {
     
        [Required]
        [MaxLength(100)]
        public string WorkflowName { get; set; }

        [Required]
        public int WorkflowType { get; set; } 

        [Required]
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public bool IsSpecialOrReserved { get; set; } = false;

        // Navigation properties
        public virtual ICollection<WorkflowStep> WorkflowSteps { get; set; }
    }
}
