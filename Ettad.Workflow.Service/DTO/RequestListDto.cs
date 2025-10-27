using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.DTO
{
    public class RequestListDto
    {
        public long Id { get; set; }
        public WorkflowType RequestType { get; set; }
        public string EmployeeName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string? Comments { get; set; }
    }

}
