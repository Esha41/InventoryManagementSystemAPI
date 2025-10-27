//using Ettad.Data.Enums;
//using Ettad.Workflows.Service.DTO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ettad.EntityFramework.DataBaseContext;
//using Ettad.Workflows.Service.Interface;
//using Microsoft.EntityFrameworkCore;

//namespace Ettad.Workflows.Service.Strategy
//{
//    public class OvertimeRequestHandler : IRequestHandler
//    {
//        private readonly ApplicationDbContext _context;
//        public WorkflowType Type => WorkflowType.Overtime;

//        public OvertimeRequestHandler(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<RequestListDto>> GetRequestsAsync()
//        {
//            return await _context.OverTimeRequests
//                .Include(x => x.EmployeeEmployment.Employee)
//                .Select(x => new RequestListDto
//                {
//                    Id = x.Id,
//                    RequestType = WorkflowType.Overtime,
//                    EmployeeName = x.EmployeeEmployment.Employee.FirstName,
//                    RequestDate = x.RequestDate,
//                    Status = x.Status.ToString(),
//                    Comments = null
//                }).ToListAsync();
//        }
//        public async Task<RequestDetailsDto?> GetRequestDetailsAsync(long requestId)
//        {
//            return await _context.OverTimeRequests
//                .Include(x => x.EmployeeEmployment.Employee)
//                .Where(x => x.Id == requestId)
//                .Select(x => new RequestDetailsDto
//                {
//                    Id = x.Id,
//                    RequestType = WorkflowType.Overtime,
//                    EmployeeName = x.EmployeeEmployment.Employee.FirstName,
//                    EmployeeCode = x.EmployeeEmployment.Employee.EmployeeCode,
//                    RequestDate = x.RequestDate,
//                    Status = x.Status.ToString(),
//                    RequestedHours = x.OverTimeHours,
//                    ApprovedHours = x.ApprovedOvertimeHours,
//                    Comments = null
//                }).FirstOrDefaultAsync();
//        }

//    }

//}
