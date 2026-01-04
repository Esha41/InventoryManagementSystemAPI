using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ettad.EntityFramework.DataBaseContext.DataSeeding.Workflows
{
    public static class SeedWorkflows
    {
        private const string SystemUser = "SYSTEM";

        public static async Task SeedNormalOrderWorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.NoramlOrder,
                    "Normal Order Workflow",
                    GetNormalOrderSteps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Normal Order workflow error: {ex.Message}");
                throw;
            }
        }

        public static async Task SeedNoramlOrderForTrainingPurposeWorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.NoramlOrderForTrainingPurpose,
                    "Noraml Order for Training Purpose Workflow",
                    GetNoramlOrderForTrainingPurposeSteps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Normal Order workflow error: {ex.Message}");
                throw;
            }
        }

        public static async Task SeedOrderFromAllowanceWorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.OrderFromAllowance,
                    "Order From Allowance Workflow",
                    GetOrderFromAllowanceSteps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Order From Allowance workflow error: {ex.Message}");
                throw;
            }
        }
      
        public static async Task SeedDiscardWorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.Discard,
                    "Discard Workflow",
                    GetDiscardSteps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Discard workflow error: {ex.Message}");
                throw;
            }
        }
    
        public static async Task SeedReturnWorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.Return,
                    "Return Workflow",
                    GetReturnSteps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Return workflow error: {ex.Message}");
                throw;
            }
        }

        private static List<WorkflowStepSeedDefinition> GetNoramlOrderForTrainingPurposeSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity"),
                new(3, "Military Training Auditor (Military Training)", "Military Training"),
                new(4, "Military Training Officer (Military Training)", "Military Training"),
                new(5, "Head of Military Training (Military Training)", "Military Training"),
                new(6, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(7, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(8, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament"),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(9, "Auditor of Military Operation (Military Operation)", "Military Operation"),
                new(10, "Officer of Military Operation (Military Operation)", "Military Operation"),
                new(11, "Chief of Operations (Military Operation)", "Military Operation"),
                
                new (
                    12,
                    "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)",
                    "Chief of Staff Office",
                    MustApprove: true,
                    RequireHigherApproval: true,
                    ReserveQty: false,
                    HigherApprovalRoleName: "Deputy Chief of Staff for Operations (Chief of Staff Office)",
                    HigherApplicationEntityName: "Chief of Staff Office"),

                new (
                    13,
                    "Auditor of Chief of Staff Office (Chief of Staff Office)",
                    "Chief of Staff Office",
                    MustApprove: true,
                    RequireHigherApproval: true,
                    ReserveQty: false,
                    HigherApprovalRoleName: "Chief of Staff (Chief of Staff Office)",
                    HigherApplicationEntityName: "Chief of Staff Office"),

                new(14, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(15, "Auditor of Audit Depo (Inventory)", "Inventory"),
                new(16, "Head of Audit Depo (Inventory)", "Inventory"),
                new(17, "Depo Commander (Inventory)", "Inventory"),
                new(18, "Auditor of Depo Division (Inventory)", "Inventory"),
                new(19, "Head of Depo Division (Inventory)", "Inventory"),
                new(20, "Depo Officer (Inventory)", "Inventory")
            };
        }

        private static List<WorkflowStepSeedDefinition> GetOrderFromAllowanceSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity"),
                new(3, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(4, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament"),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(6, "Auditor of Military Operation (Military Operation)", "Military Operation"),
                new(7, "Officer of Military Operation (Military Operation)", "Military Operation"),
                new(8, "Chief of Operations (Military Operation)", "Military Operation"),

                new (
                    9,
                    "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)",
                    "Chief of Staff Office",
                    MustApprove: true,
                    RequireHigherApproval: true,
                    ReserveQty: false,
                    HigherApprovalRoleName: "Deputy Chief of Staff for Operations (Chief of Staff Office)",
                    HigherApplicationEntityName: "Chief of Staff Office"),

                new (
                    10,
                    "Auditor of Chief of Staff Office (Chief of Staff Office)",
                    "Chief of Staff Office",
                    MustApprove: true,
                    RequireHigherApproval: true,
                    ReserveQty: false,
                    HigherApprovalRoleName: "Chief of Staff (Chief of Staff Office)",
                    HigherApplicationEntityName: "Chief of Staff Office"),

                new(11, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(12, "Auditor of Audit Depo (Inventory)", "Inventory"),
                new(13, "Head of Audit Depo (Inventory)", "Inventory"),
                new(14, "Depo Commander (Inventory)", "Inventory"),
                new(15, "Auditor of Depo Division (Inventory)", "Inventory"),
                new(16, "Head of Depo Division (Inventory)", "Inventory"),
                new(17, "Depo Officer (Inventory)", "Inventory")
            };
        }

        private static List<WorkflowStepSeedDefinition> GetNormalOrderSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity"),
                new(3, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(4, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament"),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(6, "Auditor of Military Operation (Military Operation)", "Military Operation"),
                new(7, "Officer of Military Operation (Military Operation)", "Military Operation"),
                new(8, "Chief of Operations (Military Operation)", "Military Operation"),

                new (
                    9,
                    "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)",
                    "Chief of Staff Office",
                    MustApprove: true,
                    RequireHigherApproval: true,
                    ReserveQty: false,
                    HigherApprovalRoleName: "Deputy Chief of Staff for Operations (Chief of Staff Office)",
                    HigherApplicationEntityName: "Chief of Staff Office"),

                new (
                    10,
                    "Auditor of Chief of Staff Office (Chief of Staff Office)",
                    "Chief of Staff Office",
                    MustApprove: true,
                    RequireHigherApproval: true,
                    ReserveQty: false,
                    HigherApprovalRoleName: "Chief of Staff (Chief of Staff Office)",
                    HigherApplicationEntityName: "Chief of Staff Office"),

                new(11, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(12, "Auditor of Audit Depo (Inventory)", "Inventory"),
                new(13, "Head of Audit Depo (Inventory)", "Inventory"),
                new(14, "Depo Commander (Inventory)", "Inventory"),
                new(15, "Auditor of Depo Division (Inventory)", "Inventory"),
                new(16, "Head of Depo Division (Inventory)", "Inventory"),
                new(17, "Depo Officer (Inventory)", "Inventory")
            };
        }

        private static List<WorkflowStepSeedDefinition> GetDiscardSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity"),
                new(3, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(4, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament"),
                new(6, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"),
                new(7, "Auditor of Audit Depo (Inventory)", "Inventory"),
                new(8, "Head of Audit Depo (Inventory)", "Inventory"),
                new(9, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"),
                new(10, "Auditor of Military Operation (Military Operation)", "Military Operation"),
                new(11, "Officer of Military Operation (Military Operation)", "Military Operation"),
                new(12, "Chief of Operations (Military Operation)", "Military Operation"),
                new(13, "Auditor of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office"),
                new(14, "Deputy Chief of Staff for Operations (Chief of Staff Office)", "Chief of Staff Office"),
                new(15, "Chief of Staff (Chief of Staff Office)", "Chief of Staff Office"),
                new(16, "Officer of Military Operation (Military Operation)", "Military Operation"),
                new(17, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament"),
                new(18, "Auditor of Depo Division (Inventory)", "Inventory"),
                new(19, "Depo Officer (Inventory)", "Inventory"),
            };
        }

        private static List<WorkflowStepSeedDefinition> GetReturnSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity"),
                new(3, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(4, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament"),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament"),
                new(6, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"),
                new(7, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament"),
                new(8, "Auditor of Audit Depo (Inventory)", "Inventory"),
                new(9, "Head of Audit Depo (Inventory)", "Inventory"),
                new(10, "Auditor of Depo Division (Inventory)", "Inventory"),
                new(11, "Depo Commander (Inventory)", "Inventory"),
                new(12, "Depo Officer (Inventory)", "Inventory"),
            };
        }

        private static async Task SeedWorkflowAsync(
            ApplicationDbContext context,
            WorkflowType workflowType,
            string workflowName,
            Func<List<WorkflowStepSeedDefinition>> getDefinitions)
        {
            var workflow = await context.Workflows
                .FirstOrDefaultAsync(w => w.WorkflowType == workflowType && w.IsActive && !w.IsDeleted);

            if (workflow != null)
            {
                return;
            }

            workflow = new Workflow
            {
                WorkflowName = workflowName,
                WorkflowType = workflowType,
                IsActive = true,
                IsDeleted = false,
                IsSpecialOrReserved = false,
                CreatedBy = SystemUser,
                CreationDate = DateTime.UtcNow
            };

            await context.Workflows.AddAsync(workflow);
            await context.SaveChangesAsync();

            var definitions = getDefinitions();

            var roleNames = definitions
                .Select(d => d.RoleName)
                .Concat(definitions.Where(d => !string.IsNullOrWhiteSpace(d.HigherApprovalRoleName))
                    .Select(d => d.HigherApprovalRoleName!))
                .Distinct()
                .ToList();
            var roles = await context.Roles
                .Where(role => roleNames.Contains(role.Name))
                .ToDictionaryAsync(role => role.Name);

            var missingRoles = roleNames.Except(roles.Keys).ToList();
            if (missingRoles.Any())
            {
                throw new InvalidOperationException(
                    $"Cannot seed {workflowName} because these roles are missing: {string.Join(", ", missingRoles)}");
            }

            var entityNames = definitions.Select(d => d.EntityName).Distinct().ToList();
            var entities = await context.ApplicationEntities
                .Where(entity => entityNames.Contains(entity.NameEn))
                .ToDictionaryAsync(entity => entity.NameEn, entity => entity.Id);

            var missingEntities = entityNames.Except(entities.Keys).ToList();
            if (missingEntities.Any())
            {
                throw new InvalidOperationException(
                    $"Cannot seed {workflowName} because these entities are missing: {string.Join(", ", missingEntities)}");
            }

            var utcNow = DateTime.UtcNow;
            var steps = definitions.Select(definition => new WorkflowStep
            {
                WorkflowId = workflow.Id,
                StepOrder = definition.StepOrder,
                ApplicationRoleId = roles[definition.RoleName].Id,
                ApplicationEntityId = entities[definition.EntityName],
                MustApprove = definition.MustApprove,
                RequireHigherApproval = definition.RequireHigherApproval,
                HigherApprovalRoleId = definition.HigherApprovalRoleName != null
                    ? roles[definition.HigherApprovalRoleName].Id
                    : null,
                HigherApplicationEntityId = definition.HigherApplicationEntityName != null
                    ? entities[definition.HigherApplicationEntityName]
                    : null,
                ReserveQty = definition.ReserveQty,
                CreatedBy = SystemUser,
                CreationDate = utcNow
            }).ToList();

            await context.WorkflowSteps.AddRangeAsync(steps);
            await context.SaveChangesAsync();
        }

        private record WorkflowStepSeedDefinition(
            int StepOrder,
            string RoleName,
            string EntityName,
            bool MustApprove = true,
            bool RequireHigherApproval = false,
            bool ReserveQty = false,
            string? HigherApprovalRoleName = null,
            string? HigherApplicationEntityName = null);
    }
}
