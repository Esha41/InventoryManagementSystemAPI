using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using System;
using Ettad.CrossCutting.Comman.Time;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Command.DeleteWorkflow
{
    public class DeleteWorkflowCommand : IRequest<APIOperationResponse<bool>>
    {
        public long Id { get; }

        public DeleteWorkflowCommand(long id)
        {
            Id = id;
        }
    }

    public class DeleteWorkflowCommandHandler : IRequestHandler<DeleteWorkflowCommand, APIOperationResponse<bool>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<DeleteWorkflowCommandHandler> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeleteWorkflowCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            ILogger<DeleteWorkflowCommandHandler> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<bool>> Handle(DeleteWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Workflows
                    .Include(x => x.WorkflowSteps)
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted == false, cancellationToken);

                if (entity == null)
                {
                    return APIOperationResponse<bool>.NotFound($"Workflow with ID {request.Id} not found.");
                }

                // Soft delete the workflow
                entity.IsDeleted = true;
                entity.IsActive = false;
                entity.ModifiedBy = _currentUserService.UserName;
                entity.ModificationDate = _dateTimeProvider.Now;

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Workflow soft deletion completed successfully. Workflow ID: {WorkflowId}", request.Id);
                return APIOperationResponse<bool>.Success(true, "Workflow deleted successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while soft deleting workflow. Workflow ID: {WorkflowId}", request.Id);
                return APIOperationResponse<bool>.ServerError($"Workflow deletion failed: {e.Message}");
            }
        }
    }
}