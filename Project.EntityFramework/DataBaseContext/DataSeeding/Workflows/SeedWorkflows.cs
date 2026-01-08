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
                    WorkflowType.NormalOrder,
                    "Normal Order Workflow",
                    GetNormalOrderSteps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Normal Order workflow error: {ex.Message}");
                throw;
            }
        }

        public static async Task SeedNormalOrderForTrainingPurposeWorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.NormalOrderForTrainingPurpose,
                    "Normal Order for Training Purpose Workflow",
                    GetNormalOrderForTrainingPurposeSteps);
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

        public static async Task SeedNormalOrder_Weapon_WorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.NormalOrder_Weapon,
                    "Normal Order Workflow (Weapon)",
                    GetNormalOrder_Weapon_Steps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Normal Order workflow (Weapon) error: {ex.Message}");
                throw;
            }
        }

        public static async Task SeedNormalOrderForTrainingPurpose_Weapon_WorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.NormalOrderForTrainingPurpose_Weapon,
                    "Normal Order for Training Purpose Workflow (Weapon)",
                    GetNormalOrderForTrainingPurpose_Weapon_Steps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Normal Order workflow (Weapon) error: {ex.Message}");
                throw;
            }
        }

        public static async Task SeedOrderFromAllowance_Weapon_WorkflowAsync(ApplicationDbContext context)
        {
            try
            {
                await SeedWorkflowAsync(
                    context,
                    WorkflowType.OrderFromAllowance_Weapon,
                    "Order From Allowance Workflow (Weapon)",
                    GetOrderFromAllowance_Weapon_Steps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Order From Allowance workflow (Weapon) error: {ex.Message}");
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

        private static List<WorkflowStepSeedDefinition> GetNormalOrderForTrainingPurposeSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity", CanReturn: true),
                new(3, "Military Training Auditor (Military Training)", "Military Training", CanReturn: true),
                new(4, "Military Training Officer (Military Training)", "Military Training", CanReturn: true),
                new(5, "Head of Military Training (Military Training)", "Military Training", CanReturn: true),
                new(6, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(7, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(8, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(9, "Auditor of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(10, "Officer of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(11, "Chief of Operations (Military Operation)", "Military Operation", CanReturn: true),
                
                new(12, "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(13, "Deputy Chief of Staff for Operations (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(14, "Auditor of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(15, "Chief of Staff (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),

                new(16, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(17, "Auditor of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(18, "Head of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(19, "Depo Commander (Inventory)", "Inventory", CanReturn: true),
                new(20, "Auditor of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(21, "Head of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(22, "Depo Officer (Inventory)", "Inventory", CanReturn: true)
            };
        }

        private static List<WorkflowStepSeedDefinition> GetOrderFromAllowanceSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity", CanReturn: true),
                new(3, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(4, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(6, "Auditor of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(7, "Officer of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(8, "Chief of Operations (Military Operation)", "Military Operation", CanReturn: true),

                new(9, "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(10, "Deputy Chief of Staff for Operations (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(11, "Auditor of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(12, "Chief of Staff (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),

                new(13, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(14, "Auditor of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(15, "Head of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(16, "Depo Commander (Inventory)", "Inventory", CanReturn: true),
                new(17, "Auditor of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(18, "Head of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(19, "Depo Officer (Inventory)", "Inventory", CanReturn: true)
            };
        }

        private static List<WorkflowStepSeedDefinition> GetNormalOrderSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity", CanReturn: true),
                new(3, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(4, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(6, "Auditor of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(7, "Officer of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(8, "Chief of Operations (Military Operation)", "Military Operation", CanReturn: true),

                new(9, "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(10, "Deputy Chief of Staff for Operations (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(11, "Auditor of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(12, "Chief of Staff (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),

                new(13, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(14, "Auditor of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(15, "Head of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(16, "Depo Commander (Inventory)", "Inventory", CanReturn: true),
                new(17, "Auditor of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(18, "Head of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(19, "Depo Officer (Inventory)", "Inventory", CanReturn: true)
            };
        }

        private static List<WorkflowStepSeedDefinition> GetNormalOrderForTrainingPurpose_Weapon_Steps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity", CanReturn: true),
                new(3, "Military Training Auditor (Military Training)", "Military Training", CanReturn: true),
                new(4, "Military Training Officer (Military Training)", "Military Training", CanReturn: true),
                new(5, "Head of Military Training (Military Training)", "Military Training", CanReturn: true),
                new(6, "Auditor of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(7, "Head of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(8, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(9, "Auditor of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(10, "Officer of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(11, "Chief of Operations (Military Operation)", "Military Operation", CanReturn: true),

                new(12, "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(13, "Deputy Chief of Staff for Operations (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(14, "Auditor of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(15, "Chief of Staff (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),

                new(16, "Auditor of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(17, "Auditor of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(18, "Head of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(19, "Depo Commander (Inventory)", "Inventory", CanReturn: true),
                new(20, "Auditor of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(21, "Head of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(22, "Depo Officer (Inventory)", "Inventory", CanReturn: true)
            };
        }

        private static List<WorkflowStepSeedDefinition> GetOrderFromAllowance_Weapon_Steps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity", CanReturn: true),
                new(3, "Auditor of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(4, "Head of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(6, "Auditor of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(7, "Officer of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(8, "Chief of Operations (Military Operation)", "Military Operation", CanReturn: true),

                new(9, "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(10, "Deputy Chief of Staff for Operations (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(11, "Auditor of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(12, "Chief of Staff (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),

                new(13, "Auditor of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(14, "Auditor of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(15, "Head of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(16, "Depo Commander (Inventory)", "Inventory", CanReturn: true),
                new(17, "Auditor of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(18, "Head of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(19, "Depo Officer (Inventory)", "Inventory", CanReturn: true)
            };
        }

        private static List<WorkflowStepSeedDefinition> GetNormalOrder_Weapon_Steps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity", CanReturn: true),
                new(3, "Auditor of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(4, "Head of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                // new(000, "Head of Logistics (Directorate of Armament)", "Directorate of Armament"), this will only be notified
                new(6, "Auditor of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(7, "Officer of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(8, "Chief of Operations (Military Operation)", "Military Operation", CanReturn: true),

                new(9, "Auditor of Deputy of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(10, "Deputy Chief of Staff for Operations (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(11, "Auditor of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(12, "Chief of Staff (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),

                new(13, "Auditor of Weapons Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(14, "Auditor of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(15, "Head of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(16, "Depo Commander (Inventory)", "Inventory", CanReturn: true),
                new(17, "Auditor of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(18, "Head of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(19, "Depo Officer (Inventory)", "Inventory", CanReturn: true)
            };
        }

        private static List<WorkflowStepSeedDefinition> GetDiscardSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity", CanReturn: true),
                new(3, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(4, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(6, "Head of Logistics (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(7, "Auditor of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(8, "Head of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(9, "Head of Logistics (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(10, "Auditor of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(11, "Officer of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(12, "Chief of Operations (Military Operation)", "Military Operation", CanReturn: true),
                new(13, "Auditor of Chief of Staff Office (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(14, "Deputy Chief of Staff for Operations (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(15, "Chief of Staff (Chief of Staff Office)", "Chief of Staff Office", CanReturn: true),
                new(16, "Officer of Military Operation (Military Operation)", "Military Operation", CanReturn: true),
                new(17, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(18, "Auditor of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(19, "Depo Officer (Inventory)", "Inventory", CanReturn: true),
            };
        }

        private static List<WorkflowStepSeedDefinition> GetReturnSteps()
        {
            return new List<WorkflowStepSeedDefinition>
            {
                new(1, "Supply Officer (Order Requesting Entity)", "Order Requesting Entity"),
                new(2, "Requesting Entity Commander (Order Requesting Entity)", "Order Requesting Entity", CanReturn: true),
                new(3, "Auditor of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(4, "Head of Ammunition Division (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(5, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(6, "Head of Logistics (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(7, "Director of the Armament Entity (Directorate of Armament)", "Directorate of Armament", CanReturn: true),
                new(8, "Auditor of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(9, "Head of Audit Depo (Inventory)", "Inventory", CanReturn: true),
                new(10, "Auditor of Depo Division (Inventory)", "Inventory", CanReturn: true),
                new(11, "Depo Commander (Inventory)", "Inventory", CanReturn: true),
                new(12, "Depo Officer (Inventory)", "Inventory", CanReturn: true),
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
                CanReturn = definition.CanReturn,
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
            bool CanReturn = false,
            string? HigherApprovalRoleName = null,
            string? HigherApplicationEntityName = null);
    }
}
