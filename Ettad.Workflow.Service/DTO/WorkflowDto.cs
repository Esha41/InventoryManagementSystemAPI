using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.DTO
{
    public class WorkflowDto
    {
        public int Id { get; set; }
        public string WorkflowName { get; set; }
        public WorkflowType WorkflowType { get; set; }
        public RequesterType RequesterType { get; set; }
        public int OrganizationId { get; set; }
        public int? CompanyId { get; set; }
        public int? DepartementId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


        public virtual ICollection<WorkflowStepDto> WorkflowSteps { get; set; }
    }


}
