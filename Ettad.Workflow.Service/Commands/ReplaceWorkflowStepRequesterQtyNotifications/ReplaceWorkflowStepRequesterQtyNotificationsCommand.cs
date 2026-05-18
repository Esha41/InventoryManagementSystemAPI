using Ettad.ResponseHandler.Models;
using MediatR;
using Ettad.Workflows.Service.Dtos;

namespace Ettad.Workflows.Service.Commands.ReplaceWorkflowStepRequesterQtyNotifications
{
    public class ReplaceWorkflowStepRequesterQtyNotificationsCommand : IRequest<APIOperationResponse<bool>>
    {
        public ReplaceWorkflowStepRequesterQtyNotificationsDto Dto { get; }

        public ReplaceWorkflowStepRequesterQtyNotificationsCommand(ReplaceWorkflowStepRequesterQtyNotificationsDto dto)
        {
            Dto = dto;
        }
    }
}
