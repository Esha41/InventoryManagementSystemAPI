//using Ettad.Data.Enums;
//using Ettad.Workflows.Service.DTO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ettad.Workflows.Service.Interface;
//using Ettad.EntityFramework.DataBaseContext;
//using Microsoft.EntityFrameworkCore;

//namespace Ettad.Workflows.Service.Strategy
//{
//    public class LeaveRequestHandler : IRequestHandler
//    {
//        private readonly ApplicationDbContext _context;
//        public WorkflowType Type => WorkflowType.Leave;

//        public LeaveRequestHandler(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<RequestListDto>> GetRequestsAsync()
//        {
//            return await _context.LeaveRequests
//                .Include(x => x.EmployeeEmployment.Employee)
//                .Select(x => new RequestListDto
//                {
//                    Id = x.Id,
//                    RequestType = WorkflowType.Leave,
//                    EmployeeName = x.EmployeeEmployment.Employee.FirstName,
//                    RequestDate = x.RequestDate,
//                    Status = x.Status.ToString(),
//                    Comments = x.Comments
//                }).ToListAsync();
//        }
//        public async Task<RequestDetailsDto?> GetRequestDetailsAsync(long requestId)
//        {
//            return await _context.LeaveRequests
//                .Include(x => x.EmployeeEmployment.Employee)
//                .Include(x => x.LeaveType)
//                .Where(x => x.Id == requestId)
//                .Select(x => new RequestDetailsDto
//                {
//                    Id = x.Id,
//                    RequestType = WorkflowType.Leave,
//                    EmployeeName = x.EmployeeEmployment.Employee.FirstName,
//                    EmployeeCode = x.EmployeeEmployment.Employee.EmployeeCode,
//                    RequestDate = x.RequestDate,
//                    Status = x.Status.ToString(),
//                    LeaveDays = x.NumberOfDays,
//                    LeaveType = x.LeaveType.Name,
//                    Comments = x.Comments,
//                    AttachmentUrl = x.AttachmentURL
//                }).FirstOrDefaultAsync();
//        }

//    }

//}
