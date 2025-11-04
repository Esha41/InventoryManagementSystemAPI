using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Data.Entities;

namespace Ettad.EntityFramework.DataBaseContext.DataSeeding
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedDefaultDataAsync(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                await EnsureLookupSeedAsync(context);
                await EnsureAmmunitionSeedAsync(context);
            }
            catch (Exception)
            {
                // swallow for startup; logs handled by outer try/catch
            }
        }

        private static async Task EnsureLookupSeedAsync(ApplicationDbContext context)
        {
            // Minimal required lookups for Ammunition to satisfy FK constraints
            if (!await context.Units.AnyAsync())
            {
                await context.Units.AddRangeAsync(
                    new Unit { NameEn = "mm", NameAr = "مم" },
                    new Unit { NameEn = "cm", NameAr = "سم" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Nsn.AnyAsync())
            {
                await context.Nsn.AddRangeAsync(
                    new Nsn { NameEn = "NSN-001", NameAr = "NSN-001" },
                    new Nsn { NameEn = "NSN-002", NameAr = "NSN-002" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Hcc.AnyAsync())
            {
                await context.Hcc.AddAsync(new Hcc { NameEn = "HCC-A", NameAr = "HCC-A" });
                await context.SaveChangesAsync();
            }

            if (!await context.CaseTypes.AnyAsync())
            {
                await context.CaseTypes.AddAsync(new CaseType { NameEn = "Standard", NameAr = "قياسي" });
                await context.SaveChangesAsync();
            }

            if (!await context.Propellants.AnyAsync())
            {
                await context.Propellants.AddAsync(new Propellant { NameEn = "Propellant-A", NameAr = "دافع-أ" });
                await context.SaveChangesAsync();
            }

            if (!await context.Compatibilities.AnyAsync())
            {
                await context.Compatibilities.AddAsync(new Compatibility { NameEn = "General", NameAr = "عام" });
                await context.SaveChangesAsync();
            }

            if (!await context.HazardDivisions.AnyAsync())
            {
                await context.HazardDivisions.AddAsync(new HazardDivision { NameEn = "HD 1.1", NameAr = "HD 1.1" });
                await context.SaveChangesAsync();
            }
        }

        private static async Task EnsureAmmunitionSeedAsync(ApplicationDbContext context)
        {
            if (await context.Ammunitions.AnyAsync()) return;

            var unitMm = await context.Units.FirstAsync();
            var unitCm = await context.Units.Skip(1).FirstOrDefaultAsync() ?? unitMm;
            var nsn1 = await context.Nsn.FirstAsync();
            var nsn2 = await context.Nsn.Skip(1).FirstOrDefaultAsync() ?? nsn1;
            var hcc = await context.Hcc.FirstAsync();
            var caseType = await context.CaseTypes.FirstAsync();
            var propellant = await context.Propellants.FirstAsync();
            var compatibility = await context.Compatibilities.FirstAsync();
            var hazard = await context.HazardDivisions.FirstAsync();

            var ammo1 = new Ammunition
            {
                Name = "Cartridge 5.56mm",
                ItemNo = "CAR-556-STD",
                BatchNo = "BATCH-A1",
                HccId = hcc.Id,
                PartNo = "P-556-01",
                ReadyForIssue = true,
                ExpiryDate = DateTime.UtcNow.AddYears(2),
                BulletDiameter = 5.56m,
                BulletDiameterUnitId = unitMm.Id,
                CaseLength = 45m,
                CaseLengthUnitId = unitMm.Id,
                IsLinked = false,
                Primer = "Boxer",
                TotalWeight = 12.3m,
                NsnId = nsn1.Id,
                CaseTypeId = caseType.Id,
                PropellantId = propellant.Id,
                CompatibilityId = compatibility.Id,
                HazardDivisionId = hazard.Id
            };

            var ammo2 = new Ammunition
            {
                Name = "Cartridge 9mm",
                ItemNo = "CAR-9MM-STD",
                BatchNo = "BATCH-B2",
                HccId = hcc.Id,
                PartNo = "P-9-02",
                ReadyForIssue = true,
                ExpiryDate = DateTime.UtcNow.AddYears(3),
                BulletDiameter = 9m,
                BulletDiameterUnitId = unitMm.Id,
                CaseLength = 19m,
                CaseLengthUnitId = unitMm.Id,
                IsLinked = false,
                Primer = "Boxer",
                TotalWeight = 8.0m,
                NsnId = nsn2.Id,
                CaseTypeId = caseType.Id,
                PropellantId = propellant.Id,
                CompatibilityId = compatibility.Id,
                HazardDivisionId = hazard.Id
            };

            await context.Ammunitions.AddRangeAsync(ammo1, ammo2);
            await context.SaveChangesAsync();
        }
    }
}

//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;

//namespace Ettad.EntityFramework.DataBaseContext.DataSeeding
//{
//    public static class ApplicationDbInitializer
//    {
//        public static async Task SeedDefaultDataAsync(IServiceProvider services)
//        {
//                try
//                {
//                    var context = services.GetRequiredService<ApplicationDbContext>();

//                    await SeedTranslationsAsync(context);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("An error occurred while seeding the database.");
//                    Console.WriteLine(ex.Message);
//                }
//        }

//        private static async Task SeedTranslationsAsync(ApplicationDbContext context)
//        {
//            var englishTranslation = await context.OrganizationTranslation
//                .FirstOrDefaultAsync(t => t.LanguageCode == "en" && t.IsDefault);

//            if (englishTranslation == null)
//            {
//                englishTranslation = new OrganizationTranslation
//                {
//                    LanguageCode = "en",
//                    IsDefault = true,
//                    JsonData = TranslationData.English,
//                    CreationDate = DateTime.UtcNow
//                };
//                await context.OrganizationTranslation.AddAsync(englishTranslation);
//            }
//            else
//            {
//                englishTranslation.JsonData = TranslationData.English;
//            }

//            var arabicTranslation = await context.OrganizationTranslation
//                .FirstOrDefaultAsync(t => t.LanguageCode == "ar" && t.IsDefault);

//            if (arabicTranslation == null)
//            {
//                arabicTranslation = new OrganizationTranslation
//                {
//                    LanguageCode = "ar",
//                    IsDefault = true,
//                    JsonData = TranslationData.Arabic,
//                    CreationDate = DateTime.UtcNow
//                };
//                await context.OrganizationTranslation.AddAsync(arabicTranslation);
//            }
//            else
//            {
//                arabicTranslation.JsonData = TranslationData.Arabic;
//            }

//            await context.SaveChangesAsync();
//        }
//    }
//}
