using Ettad.Data.Enums;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement.Interfaces;
using Ettad.Workflows.Service.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ettad.RequestManagement.Service.EventHandlers
{
    public class WorkflowStepApprovedEventHandler : INotificationHandler<WorkflowStepApprovedEvent>
    {
        private readonly ISupplyService _supplyService;
        private readonly ILogger<WorkflowStepApprovedEventHandler> _logger;

        public WorkflowStepApprovedEventHandler(
            ISupplyService supplyService,
            ILogger<WorkflowStepApprovedEventHandler> logger)
        {
            _supplyService = supplyService;
            _logger = logger;
        }

        public async Task Handle(WorkflowStepApprovedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Check Request Type
                if (notification.RequestType != RequestType.Order)
                {
                    return;
                }

                // 2. Check Role
                // The role name format is "RoleName (EntityName)"
                const string targetRoleName = "Head of Ammunition Division (Directorate of Armament)";
                if (notification.ApplicationRoleName != targetRoleName)
                {
                    return;
                }

                // 3. Check if draft supply already exists
                var existingSupplyResult = await _supplyService.GetByOrderIdAsync(notification.TargetRequestId);
                if (existingSupplyResult.Succeeded && existingSupplyResult.Data != null)
                {
                    // Supply already exists (whether draft or submitted). 
                    // Requirement: "if the order has no draft supply already".
                    // If a supply exists, we probably shouldn't create a new one automatically.
                    return;
                }

                // 4. Get Suggestion
                var suggestionResult = await _supplyService.GetSupplySuggestionAsync(notification.TargetRequestId);
                if (!suggestionResult.Succeeded || suggestionResult.Data == null)
                {
                    _logger.LogWarning("Failed to get supply suggestion for Order {OrderId}: {Message}", notification.TargetRequestId, suggestionResult.Message);
                    return;
                }

                var suggestion = suggestionResult.Data;

                // 5. Map to CreateSupplyDto
                var createSupplyDto = new CreateSupplyDto
                {
                    OrderId = notification.TargetRequestId,
                    SupplyDetails = suggestion.ItemSuggestions
                        .SelectMany(item => item.LotSuggestions)
                        .Select(lot => new CreateSupplyDetailDto
                        {
                            ItemId = lot.ItemId,
                            Lot = lot.Lot,
                            Quantity = lot.SuggestedQuantity,
                            Notes = "System Suggested"
                        })
                        .ToList()
                };

                if (!createSupplyDto.SupplyDetails.Any())
                {
                    _logger.LogInformation("No supply details suggested for Order {OrderId}. Skipping draft supply creation.", notification.TargetRequestId);
                    return;
                }

                // 6. Create Draft Supply
                var createResult = await _supplyService.CreateAsync(createSupplyDto);
                if (!createResult.Succeeded)
                {
                    _logger.LogError("Failed to auto-create draft supply for Order {OrderId}: {Message}", notification.TargetRequestId, createResult.Message);
                }
                else
                {
                    _logger.LogInformation("Successfully auto-created draft supply {SupplyId} for Order {OrderId}", createResult.Data, notification.TargetRequestId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling WorkflowStepApprovedEvent for Order {OrderId}", notification.TargetRequestId);
            }
        
         }
    }
}

