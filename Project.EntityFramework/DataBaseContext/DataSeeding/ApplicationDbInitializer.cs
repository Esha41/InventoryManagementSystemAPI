using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Data.Entities;
using Ettad.Data.Enums;

namespace Ettad.EntityFramework.DataBaseContext.DataSeeding
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedDefaultDataAsync(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                
                // Seed ammunition data
                await SeedAmmunitionDataAsync(context);
            }
            catch (Exception)
            {
                // swallow for startup; logs handled by outer try/catch
            }
        }

        private static async Task SeedAmmunitionDataAsync(ApplicationDbContext context)
        {
            // Check if ammunition already exists
            if (await context.Ammunitions.AnyAsync())
            {
                return; // Already seeded
            }

            var utcNow = DateTime.UtcNow;

            var ammunitions = new[]
            {
                new Ammunition
                {
                    Name = "9mm NATO Ball Ammunition",
                    ItemNo = "AMM-001",
                    BatchNo = "BATCH-001",
                    HccId = 1,
                    PartNo = "PN-001",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(2),
                    BulletDiameter = 9.01m,
                    BulletDiameterUnitId = 1,
                    CaseLength = 19.15m,
                    CaseLengthUnitId = 1,
                    IsLinked = false,
                    Primer = "CCI No. 500",
                    TotalWeight = 0.012m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-000-0003",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "7.62mm NATO Match Ammunition",
                    ItemNo = "AMM-002",
                    BatchNo = "BATCH-002",
                    HccId = 2,
                    PartNo = "PN-002",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(3),
                    BulletDiameter = 7.82m,
                    BulletDiameterUnitId = 2,
                    CaseLength = 51.05m,
                    CaseLengthUnitId = 2,
                    IsLinked = true,
                    Primer = "Federal 210M",
                    TotalWeight = 0.024m,
                    NatureOptionId = 2,
                    Nsn = "1305-01-000-0002",
                    PrimaryPurposId = 2,
                    ProjectileColorId = 2,
                    ProjectailMaterialId = 2,
                    CaseTypeId = 2,
                    PropellantId = 2,
                    CompatibilityId = 2,
                    HazardDivisionId = 2,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "5.56mm NATO Tracer Round",
                    ItemNo = "AMM-003",
                    BatchNo = "BATCH-003",
                    HccId = 3,
                    PartNo = "PN-003",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(1),
                    BulletDiameter = 5.70m,
                    BulletDiameterUnitId = 3,
                    CaseLength = 44.70m,
                    CaseLengthUnitId = 3,
                    IsLinked = false,
                    Primer = "Winchester Small Rifle",
                    TotalWeight = 0.012m,
                    NatureOptionId = 3,
                    Nsn = "1305-01-000-0001",
                    PrimaryPurposId = 3,
                    ProjectileColorId = 3,
                    ProjectailMaterialId = 3,
                    CaseTypeId = 3,
                    PropellantId = 3,
                    CompatibilityId = 3,
                    HazardDivisionId = 3,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                }
            };

            await context.Ammunitions.AddRangeAsync(ammunitions);
            await context.SaveChangesAsync();
        }
    }
}
