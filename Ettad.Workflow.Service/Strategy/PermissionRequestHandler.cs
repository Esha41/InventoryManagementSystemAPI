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
//    public class PermissionRequestHandler : IRequestHandler
//    {
//        private readonly ApplicationDbContext _context;
//        public WorkflowType Type => WorkflowType.Permission;

//        public PermissionRequestHandler(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<RequestListDto>> GetRequestsAsync()
//        {
//            return await _context.PermissionRequests
//                .Include(x => x.EmployeeEmployment.Employee)
//                .Select(x => new RequestListDto
//                {
//                    Id = x.Id,
//                    RequestType = WorkflowType.Permission,
//                    EmployeeName = x.EmployeeEmployment.Employee.FirstName,
//                    RequestDate = x.RequestDate,
//                    Status = x.Status.ToString(),
//                    Comments = x.Comments
//                }).ToListAsync();
//        }
//        public async Task<RequestDetailsDto?> GetRequestDetailsAsync(long requestId)
//        {
//            return await _context.PermissionRequests
//                .Include(x => x.EmployeeEmployment.Employee)
//                .Select(x => new RequestDetailsDto
//                {
//                    Id = x.Id,
//                    RequestType = WorkflowType.Permission,
//                    EmployeeName = x.EmployeeEmployment.Employee.FirstName,
//                    EmployeeCode = x.EmployeeEmployment.Employee.EmployeeCode,
//                    RequestDate = x.RequestDate,
//                    Status = x.Status.ToString(),
//                    FromDate = x.FromDate,
//                    ToDate = x.ToDate,
//                    FromTime = x.FromTime,
//                    ToTime = x.ToTime,
//                    Comments = x.Comments,
//                    AttachmentUrl = x.AttachmentURL
//                }).FirstOrDefaultAsync(x => x.Id == requestId)
//;
//        }

//    }

//}
