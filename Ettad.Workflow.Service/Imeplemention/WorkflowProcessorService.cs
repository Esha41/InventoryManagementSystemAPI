//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Ettad.Workflows.Service.Interface;
//using Ettad.Data.Entities.Workflows;
//using Ettad.Data.Enums;
//using Ettad.EntityFramework.DataBaseContext;

//namespace Ettad.Workflows.Service.Imeplemention
//{

 

//    public class WorkflowProcessorService : IWorkflowProcessorService
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<WorkflowProcessorService> _logger;

//        public WorkflowProcessorService(ApplicationDbContext context, ILogger<WorkflowProcessorService> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        public async Task StartWorkflowAsync(int requestId, WorkflowType requestType)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                var requestInfo = await GetRequestInfoAsync(requestId, requestType);
//                if (requestInfo == null)
//                    throw new InvalidOperationException($"Request not found for type: {requestType}, ID: {requestId}");
//                var workflow = await GetActiveWorkflowAsync(requestInfo.Value.DepartmentId, requestInfo.Value.CompanyId,requestType);
//                if (workflow == null)
//                    throw new InvalidOperationException("No active workflow found for the given department or organization.");
//                await UpdateRequestWorkflowIdAsync(requestId, requestType, workflow.Id);

//                var firstStep = await GetFirstWorkflowStepAsync(workflow.Id);
//                if (firstStep == null)
//                    throw new InvalidOperationException("Workflow has no steps defined.");

//                await CreateWorkflowHistoryAsync(firstStep, requestId, requestType);

//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();

//                _logger.LogInformation($"? Workflow started successfully for {requestType} #{requestId}, Workflow ID: {workflow.Id}");
//            }
//            catch (Exception ex)
//            {
//                await transaction.RollbackAsync();
//                _logger.LogError(ex, $"? Failed to start workflow for {requestType} #{requestId}");
//                throw;
//            }
//        }

//        #region ?? Private Helper Methods
//        // to do
//        private async Task<(long DepartmentId, long CompanyId)?> GetRequestInfoAsync(int requestId, WorkflowType requestType)
//        {
//            //switch (requestType)
//            //{
//            //    case WorkflowType.Leave:
//            //        var leave = await _context.LeaveRequests.FindAsync(requestId);
//            //        return leave == null ? null : (leave.EmployeeEmployment.DepartmentId, leave.EmployeeEmployment.CompanyId);

//            //    case WorkflowType.Permission:
//            //        var permission = await _context.PermissionRequests.FindAsync(requestId);
//            //        return permission == null ? null : (permission.EmployeeEmployment.DepartmentId, permission.EmployeeEmployment.CompanyId);

//            //    case WorkflowType.Overtime:
//            //        var overtime = await _context.OverTimeRequests.FindAsync(requestId);
//            //        return overtime == null ? null : (overtime.EmployeeEmployment.DepartmentId, overtime.EmployeeEmployment.CompanyId);

//            //    default:
//                    throw new ArgumentException($"Unknown request type: {requestType}");
//          //  }
//        }

//        private async Task< Ettad.Data.Entities.Workflows.Workflow?> GetActiveWorkflowAsync(long departmentId, long companyId,WorkflowType workflowType)
//        {
//            return await _context.Workflows
//                .Include(w => w.WorkflowSteps)
//                .Where(w => w.IsActive &&
//                            (w.DepartementId == departmentId && w.WorkflowType== workflowType && w.CompanyId == companyId))
//                .OrderBy(w => w.Id)
//                .FirstOrDefaultAsync();
//        }

//        private async Task UpdateRequestWorkflowIdAsync(int requestId, WorkflowType requestType, int workflowId)
//        {
//            // to do
//            //switch (requestType)
//            //{
//            //    case WorkflowType.Leave:
//            //        var leave = await _context.LeaveRequests.FindAsync(requestId);
//            //        if (leave != null) leave.WorkflowId = workflowId;
//            //        break;

//            //    case WorkflowType.Permission:
//            //        var permission = await _context.PermissionRequests.FindAsync(requestId);
//            //        if (permission != null) permission.WorkflowId = workflowId;
//            //        break;

//            //    case WorkflowType.Overtime:
//            //        var overtime = await _context.OverTimeRequests.FindAsync(requestId);
//            //        if (overtime != null) overtime.WorkflowId = workflowId;
//            //        break;

//            //    default:
//                    throw new ArgumentException($"Invalid request type: {requestType}");
//            //}
//        }

//        private async Task<WorkflowStep?> GetFirstWorkflowStepAsync(int workflowId)
//        {
//            return await _context.WorkflowSteps
//                .Where(s => s.WorkflowId == workflowId)
//                .OrderBy(s => s.StepOrder)
//                .FirstOrDefaultAsync();
//        }

//        private async Task CreateWorkflowHistoryAsync(WorkflowStep firstStep, int requestId, WorkflowType requestType)
//        {
//            var history = new WorkflowApprovalHistory
//            {
//                WorkflowStepId = firstStep.Id,
//                TargetRequestId = requestId,
//                RequestType = requestType,
//                ApproverEmployeeId = firstStep.ApproverEmployeeId,
//                Status = ApprovalStatus.Pending,
              
//            };

//            await _context.WorkflowApprovalHistory.AddAsync(history);
//        }

//        #endregion
//    }

//}
