using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.DTO
{
    public class RequestDetailsDto
    {
        public long Id { get; set; }
        public WorkflowType RequestType { get; set; }
        public string EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }

        public DateTime RequestDate { get; set; }
        public string Status { get; set; }

        // Leave Specific
        public double? LeaveDays { get; set; }
        public string? LeaveType { get; set; }

        // Permission Specific
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }

        // Overtime Specific
        public double? RequestedHours { get; set; }
        public double? ApprovedHours { get; set; }

        public string? Comments { get; set; }
        public string? AttachmentUrl { get; set; }
    }

}
