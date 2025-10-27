using Moujam.Casiher.Comman.Base;
using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Data.Entities.Workflows
{
    public class Workflow : AuditEntity<int>
    {
     
        [Required]
        [MaxLength(100)]
        public string WorkflowName { get; set; }

        [Required]
        public WorkflowType WorkflowType { get; set; } // Using enum instead of string

        [Required]
        public RequesterType RequesterType { get; set; } // Using enum instead of int

        [Required]
        public int OrganizationId { get; set; }

        public int? CompanyId { get; set; }

        public int? DepartementId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        
        // Navigation properties
        public virtual ICollection<WorkflowStep> WorkflowSteps { get; set; }
    }
}
