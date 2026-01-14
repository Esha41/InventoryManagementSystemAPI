using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using System.Collections.Generic;
using Serilog;

namespace Ettad.EntityFramework.DataBaseContext.DataSeeding
{
    public static class ApplicationDbInitializer
    {
        /// <summary>
        /// Checks for pending migrations and applies them if any exist
        /// </summary>
        public static async Task ApplyPendingMigrationsAsync(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                
                if (!context.Database.IsSqlServer())
                {
                    Log.Information("Database is not SQL Server. Skipping migration check.");
                    return;
                }

                // Get pending migrations
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                var pendingMigrationsList = pendingMigrations.ToList();

                if (pendingMigrationsList.Any())
                {
                    Log.Information("Found {Count} pending migration(s): {Migrations}", 
                        pendingMigrationsList.Count, 
                        string.Join(", ", pendingMigrationsList));
                    
                    // Apply pending migrations
                    await context.Database.MigrateAsync();
                    
                    Log.Information("Successfully applied {Count} pending migration(s)", pendingMigrationsList.Count);
                }
                else
                {
                    Log.Information("No pending migrations found. Database is up to date.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while applying pending migrations");
                throw;
            }
        }

        public static async Task SeedDefaultDataAsync(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                
                Console.WriteLine("=== Starting Database Seeding ===");
                
                // Seed countries data
                await SeedCountriesDataAsync(context);
                Console.WriteLine("✓ Countries seeded");
                
                // Seed suppliers data
                await SeedSuppliersDataAsync(context);
                Console.WriteLine("✓ Suppliers seeded");
                
                // Seed manufacturers data
                await SeedManufacturersDataAsync(context);
                Console.WriteLine("✓ Manufacturers seeded");
                
                // Seed item type lookup data
                await SeedItemTypeLookupDataAsync(context);
                Console.WriteLine("✓ Item Types seeded");
                
                // Seed ammunition data
                await SeedAmmunitionDataAsync(context);
                Console.WriteLine("✓ Ammunition seeded");
                
                // Seed weapon data
                await SeedWeaponDataAsync(context);
                Console.WriteLine("✓ Weapons seeded");
                
                // Seed explosive data
                await SeedExplosiveDataAsync(context);
                Console.WriteLine("✓ Explosives seeded");
                
                // Seed inventory data based on seeded depots and ammunitions
                await SeedInventoryDataAsync(context);
                Console.WriteLine("✓ Inventory seeded");
                
                // Seed allowance data for all departments and all items
                await SeedAllowanceDataAsync(context);
                Console.WriteLine("✓ Allowances seeded");
                
                Console.WriteLine("=== Database Seeding Completed Successfully ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! ERROR SEEDING DATABASE: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw; // Re-throw to see the error
            }
        }

        private static async Task SeedCountriesDataAsync(ApplicationDbContext context)
        {
            // Check if countries already exist
            if (await context.Countries.AnyAsync())
            {
                return; // Already seeded
            }

            var countries = new List<Country>
            {
                new Country { Code = "AF", NameEn = "Afghanistan", NameAr = "أفغانستان", IsDeleted = false },
                new Country { Code = "AL", NameEn = "Albania", NameAr = "ألبانيا", IsDeleted = false },
                new Country { Code = "DZ", NameEn = "Algeria", NameAr = "الجزائر", IsDeleted = false },
                new Country { Code = "AR", NameEn = "Argentina", NameAr = "الأرجنتين", IsDeleted = false },
                new Country { Code = "AU", NameEn = "Australia", NameAr = "أستراليا", IsDeleted = false },
                new Country { Code = "AT", NameEn = "Austria", NameAr = "النمسا", IsDeleted = false },
                new Country { Code = "BH", NameEn = "Bahrain", NameAr = "البحرين", IsDeleted = false },
                new Country { Code = "BD", NameEn = "Bangladesh", NameAr = "بنغلاديش", IsDeleted = false },
                new Country { Code = "BE", NameEn = "Belgium", NameAr = "بلجيكا", IsDeleted = false },
                new Country { Code = "BR", NameEn = "Brazil", NameAr = "البرازيل", IsDeleted = false },
                new Country { Code = "BG", NameEn = "Bulgaria", NameAr = "بلغاريا", IsDeleted = false },
                new Country { Code = "CA", NameEn = "Canada", NameAr = "كندا", IsDeleted = false },
                new Country { Code = "CN", NameEn = "China", NameAr = "الصين", IsDeleted = false },
                new Country { Code = "CO", NameEn = "Colombia", NameAr = "كولومبيا", IsDeleted = false },
                new Country { Code = "HR", NameEn = "Croatia", NameAr = "كرواتيا", IsDeleted = false },
                new Country { Code = "CZ", NameEn = "Czech Republic", NameAr = "جمهورية التشيك", IsDeleted = false },
                new Country { Code = "DK", NameEn = "Denmark", NameAr = "الدنمارك", IsDeleted = false },
                new Country { Code = "EG", NameEn = "Egypt", NameAr = "مصر", IsDeleted = false },
                new Country { Code = "FI", NameEn = "Finland", NameAr = "فنلندا", IsDeleted = false },
                new Country { Code = "FR", NameEn = "France", NameAr = "فرنسا", IsDeleted = false },
                new Country { Code = "DE", NameEn = "Germany", NameAr = "ألمانيا", IsDeleted = false },
                new Country { Code = "GR", NameEn = "Greece", NameAr = "اليونان", IsDeleted = false },
                new Country { Code = "HU", NameEn = "Hungary", NameAr = "المجر", IsDeleted = false },
                new Country { Code = "IN", NameEn = "India", NameAr = "الهند", IsDeleted = false },
                new Country { Code = "ID", NameEn = "Indonesia", NameAr = "إندونيسيا", IsDeleted = false },
                new Country { Code = "IR", NameEn = "Iran", NameAr = "إيران", IsDeleted = false },
                new Country { Code = "IQ", NameEn = "Iraq", NameAr = "العراق", IsDeleted = false },
                new Country { Code = "IE", NameEn = "Ireland", NameAr = "أيرلندا", IsDeleted = false },
                new Country { Code = "PS", NameEn = "Palestine", NameAr = "فلسطين", IsDeleted = false },
                new Country { Code = "IT", NameEn = "Italy", NameAr = "إيطاليا", IsDeleted = false },
                new Country { Code = "JP", NameEn = "Japan", NameAr = "اليابان", IsDeleted = false },
                new Country { Code = "JO", NameEn = "Jordan", NameAr = "الأردن", IsDeleted = false },
                new Country { Code = "KW", NameEn = "Kuwait", NameAr = "الكويت", IsDeleted = false },
                new Country { Code = "MY", NameEn = "Malaysia", NameAr = "ماليزيا", IsDeleted = false },
                new Country { Code = "MX", NameEn = "Mexico", NameAr = "المكسيك", IsDeleted = false },
                new Country { Code = "NL", NameEn = "Netherlands", NameAr = "هولندا", IsDeleted = false },
                new Country { Code = "NZ", NameEn = "New Zealand", NameAr = "نيوزيلندا", IsDeleted = false },
                new Country { Code = "NO", NameEn = "Norway", NameAr = "النرويج", IsDeleted = false },
                new Country { Code = "OM", NameEn = "Oman", NameAr = "عمان", IsDeleted = false },
                new Country { Code = "PK", NameEn = "Pakistan", NameAr = "باكستان", IsDeleted = false },
                new Country { Code = "PL", NameEn = "Poland", NameAr = "بولندا", IsDeleted = false },
                new Country { Code = "PT", NameEn = "Portugal", NameAr = "البرتغال", IsDeleted = false },
                new Country { Code = "QA", NameEn = "Qatar", NameAr = "قطر", IsDeleted = false },
                new Country { Code = "RO", NameEn = "Romania", NameAr = "رومانيا", IsDeleted = false },
                new Country { Code = "RU", NameEn = "Russia", NameAr = "روسيا", IsDeleted = false },
                new Country { Code = "SA", NameEn = "Saudi Arabia", NameAr = "المملكة العربية السعودية", IsDeleted = false },
                new Country { Code = "SG", NameEn = "Singapore", NameAr = "سنغافورة", IsDeleted = false },
                new Country { Code = "ZA", NameEn = "South Africa", NameAr = "جنوب أفريقيا", IsDeleted = false },
                new Country { Code = "KR", NameEn = "South Korea", NameAr = "كوريا الجنوبية", IsDeleted = false },
                new Country { Code = "SD", NameEn = "Sudan", NameAr = "السودان", IsDeleted = false },
                new Country { Code = "ES", NameEn = "Spain", NameAr = "إسبانيا", IsDeleted = false },
                new Country { Code = "SE", NameEn = "Sweden", NameAr = "السويد", IsDeleted = false },
                new Country { Code = "CH", NameEn = "Switzerland", NameAr = "سويسرا", IsDeleted = false },
                new Country { Code = "TW", NameEn = "Taiwan", NameAr = "تايوان", IsDeleted = false },
                new Country { Code = "TH", NameEn = "Thailand", NameAr = "تايلاند", IsDeleted = false },
                new Country { Code = "TR", NameEn = "Turkey", NameAr = "تركيا", IsDeleted = false },
                new Country { Code = "AE", NameEn = "United Arab Emirates", NameAr = "الإمارات العربية المتحدة", IsDeleted = false },
                new Country { Code = "GB", NameEn = "United Kingdom", NameAr = "المملكة المتحدة", IsDeleted = false },
                new Country { Code = "US", NameEn = "United States", NameAr = "الولايات المتحدة الأمريكية", IsDeleted = false },
                new Country { Code = "VN", NameEn = "Vietnam", NameAr = "فيتنام", IsDeleted = false },
                new Country { Code = "YE", NameEn = "Yemen", NameAr = "اليمن", IsDeleted = false }
            };

            await context.Countries.AddRangeAsync(countries);
            await context.SaveChangesAsync();
        }

        private static async Task SeedSuppliersDataAsync(ApplicationDbContext context)
        {
            // Check if suppliers already exist
            if (await context.Suppliers.AnyAsync())
            {
                return; // Already seeded
            }

            var utcNow = DateTime.Now;

            var suppliers = new[]
            {
                new Supplier { NameEn = "Global Defense Supplies", NameAr = "إمدادات الدفاع العالمية", IsDeleted = false },
                new Supplier { NameEn = "Military Equipment Corporation", NameAr = "شركة المعدات العسكرية", IsDeleted = false },
                new Supplier { NameEn = "Ammunition Supply International", NameAr = "الإمدادات الدولية للذخيرة", IsDeleted = false },
                new Supplier { NameEn = "Defense Logistics Group", NameAr = "مجموعة الدفاع اللوجستية", IsDeleted = false },
                new Supplier { NameEn = "Strategic Arms Distributors", NameAr = "موزعو الأسلحة الاستراتيجية", IsDeleted = false },
                new Supplier { NameEn = "Tactical Equipment Suppliers", NameAr = "موردو المعدات التكتيكية", IsDeleted = false },
                new Supplier { NameEn = "International Munitions Company", NameAr = "شركة الذخائر الدولية", IsDeleted = false },
                new Supplier { NameEn = "Defense Procurement Services", NameAr = "خدمات المشتريات الدفاعية", IsDeleted = false },
                new Supplier { NameEn = "Arms & Ammunition Trading", NameAr = "تجارة الأسلحة والذخيرة", IsDeleted = false },
                new Supplier { NameEn = "Military Supply Chain Solutions", NameAr = "حلول سلسلة الإمدادات العسكرية", IsDeleted = false, CreationDate = utcNow, CreatedBy = "SYSTEM" }
            };

            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedManufacturersDataAsync(ApplicationDbContext context)
        {
            // Check if manufacturers already exist
            if (await context.Manufacturers.AnyAsync())
            {
                return; // Already seeded
            }

            var utcNow = DateTime.Now;

            var manufacturers = new[]
            {
                new Manufacturer { NameEn = "Lockheed Martin", NameAr = "لوكهيد مارتن", IsDeleted = false },
                new Manufacturer { NameEn = "Raytheon Technologies", NameAr = "رايثيون تكنولوجيز", IsDeleted = false },
                new Manufacturer { NameEn = "BAE Systems", NameAr = "بي إيه إي سيستمز", IsDeleted = false },
                new Manufacturer { NameEn = "Northrop Grumman", NameAr = "نورثروب جرومان", IsDeleted = false },
                new Manufacturer { NameEn = "General Dynamics", NameAr = "جنرال ديناميكس", IsDeleted = false },
                new Manufacturer { NameEn = "Boeing Defense", NameAr = "بوينغ للدفاع", IsDeleted = false },
                new Manufacturer { NameEn = "Rheinmetall", NameAr = "راينميتال", IsDeleted = false },
                new Manufacturer { NameEn = "Thales Group", NameAr = "مجموعة تاليس", IsDeleted = false },
                new Manufacturer { NameEn = "Leonardo", NameAr = "ليوناردو", IsDeleted = false },
                new Manufacturer { NameEn = "Honeywell Aerospace", NameAr = "هانيويل للطيران", IsDeleted = false },
                new Manufacturer { NameEn = "Textron Systems", NameAr = "تكسترون سيستمز", IsDeleted = false },
                new Manufacturer { NameEn = "L3Harris Technologies", NameAr = "إل 3 هاريس تكنولوجيز", IsDeleted = false },
                new Manufacturer { NameEn = "FN Herstal", NameAr = "إف إن هيرستال", IsDeleted = false },
                new Manufacturer { NameEn = "Heckler & Koch", NameAr = "هيكلر وكوخ", IsDeleted = false },
                new Manufacturer { NameEn = "Remington Arms", NameAr = "ريمينغتون أرمز", IsDeleted = false, CreationDate = utcNow, CreatedBy = "SYSTEM" }
            };

            await context.Manufacturers.AddRangeAsync(manufacturers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedItemTypeLookupDataAsync(ApplicationDbContext context)
        {
            // Check if item types already exist
            if (await context.ItemTypes.AnyAsync())
            {
                return; // Already seeded
            }

            var itemTypes = new[]
            {
                // Ammunition Types
                new ItemTypeLookup { NameEn = "Ball", NameAr = "كروي", ItemType = ItemType.Ammunition, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Tracer", NameAr = "تتبع", ItemType = ItemType.Ammunition, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Armor Piercing", NameAr = "خارق للدروع", ItemType = ItemType.Ammunition, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Incendiary", NameAr = "حارق", ItemType = ItemType.Ammunition, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Blank", NameAr = "فارغ", ItemType = ItemType.Ammunition, IsDeleted = false },
                
                // Weapon Types
                new ItemTypeLookup { NameEn = "Rifle", NameAr = "بندقية", ItemType = ItemType.Weapon, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Pistol", NameAr = "مسدس", ItemType = ItemType.Weapon, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Machine Gun", NameAr = "رشاش", ItemType = ItemType.Weapon, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Sniper Rifle", NameAr = "بندقية قنص", ItemType = ItemType.Weapon, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Shotgun", NameAr = "بندقية صيد", ItemType = ItemType.Weapon, IsDeleted = false },
                
                // Explosive Types
                new ItemTypeLookup { NameEn = "Grenade", NameAr = "قنبلة يدوية", ItemType = ItemType.Explosive, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Mine", NameAr = "لغم", ItemType = ItemType.Explosive, IsDeleted = false },
                new ItemTypeLookup { NameEn = "C4", NameAr = "سي 4", ItemType = ItemType.Explosive, IsDeleted = false },
                new ItemTypeLookup { NameEn = "TNT", NameAr = "تي إن تي", ItemType = ItemType.Explosive, IsDeleted = false },
                new ItemTypeLookup { NameEn = "Rocket", NameAr = "صاروخ", ItemType = ItemType.Explosive, IsDeleted = false }
            };

            await context.ItemTypes.AddRangeAsync(itemTypes);
            await context.SaveChangesAsync();
        }

        private static async Task SeedAmmunitionDataAsync(ApplicationDbContext context)
        {
            // Check if ammunition already exists
            if (await context.Ammunitions.AnyAsync())
            {
                return; // Already seeded
            }

            var utcNow = DateTime.Now;

            var ammunitions = new[]
            {
                new Ammunition
                {
                    Name = "9mm NATO Ball Ammunition",
                    ItemNo = "AMM-001",
                    AmmunitionType = AmmunitionType.Small,
                    PartNo = "PN-9MM-001",
                    BulletDiameter = 9.01m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Small Pistol",
                    TotalWeight = 0.012m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-527-3103",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 0.65m,
                    MinimumQuantity = 500,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "5.56mm M855 Ball (Green Tip)",
                    ItemNo = "AMM-002",
                    AmmunitionType = AmmunitionType.Small,
                    PartNo = "PN-556-M855",
                    BulletDiameter = 5.70m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Small Rifle",
                    TotalWeight = 0.012m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-231-3242",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 2,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 0.75m,
                    MinimumQuantity = 1000,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "5.56mm M856 Tracer (Red Tip)",
                    ItemNo = "AMM-003",
                    AmmunitionType = AmmunitionType.Small,
                    PartNo = "PN-556-M856",
                    BulletDiameter = 5.70m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Small Rifle",
                    TotalWeight = 0.011m,
                    NatureOptionId = 2,
                    Nsn = "1305-01-231-3243",
                    PrimaryPurposId = 3,
                    ProjectileColorId = 3,
                    ProjectailMaterialId = 2,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 0.95m,
                    MinimumQuantity = 500,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "7.62mm M80 Ball",
                    ItemNo = "AMM-004",
                    AmmunitionType = AmmunitionType.Medium,
                    PartNo = "PN-762-M80",
                    BulletDiameter = 7.82m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Large Rifle",
                    TotalWeight = 0.025m,
                    NatureOptionId = 1,
                    Nsn = "1305-00-903-0430",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 2,
                    PropellantId = 2,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 1.25m,
                    MinimumQuantity = 800,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "7.62mm M62 Tracer",
                    ItemNo = "AMM-005",
                    AmmunitionType = AmmunitionType.Medium,
                    PartNo = "PN-762-M62",
                    BulletDiameter = 7.82m,
                    BulletDiameterUnitId = 1,
                    IsLinked = true,
                    Primer = "Large Rifle",
                    TotalWeight = 0.026m,
                    NatureOptionId = 2,
                    Nsn = "1305-00-903-0431",
                    PrimaryPurposId = 3,
                    ProjectileColorId = 3,
                    ProjectailMaterialId = 2,
                    CaseTypeId = 2,
                    PropellantId = 2,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 1.45m,
                    MinimumQuantity = 600,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "7.62mm M118LR Sniper",
                    ItemNo = "AMM-006",
                    AmmunitionType = AmmunitionType.Medium,
                    PartNo = "PN-762-M118LR",
                    BulletDiameter = 7.82m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Large Rifle Match",
                    TotalWeight = 0.028m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-419-1687",
                    PrimaryPurposId = 2,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 2,
                    PropellantId = 2,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 2.50m,
                    MinimumQuantity = 300,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = ".50 BMG M33 Ball",
                    ItemNo = "AMM-007",
                    AmmunitionType = AmmunitionType.Large,
                    PartNo = "PN-50BMG-M33",
                    BulletDiameter = 12.98m,
                    BulletDiameterUnitId = 1,
                    IsLinked = true,
                    Primer = "Large Rifle Magnum",
                    TotalWeight = 0.114m,
                    NatureOptionId = 1,
                    Nsn = "1305-00-179-6329",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 3,
                    PropellantId = 3,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 5.50m,
                    MinimumQuantity = 200,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = ".50 BMG M2 AP (Armor Piercing)",
                    ItemNo = "AMM-008",
                    AmmunitionType = AmmunitionType.Large,
                    PartNo = "PN-50BMG-M2AP",
                    BulletDiameter = 12.98m,
                    BulletDiameterUnitId = 1,
                    IsLinked = true,
                    Primer = "Large Rifle Magnum",
                    TotalWeight = 0.116m,
                    NatureOptionId = 1,
                    Nsn = "1305-00-179-6330",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 3,
                    CaseTypeId = 3,
                    PropellantId = 3,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 7.25m,
                    MinimumQuantity = 150,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = ".45 ACP Ball M1911",
                    ItemNo = "AMM-009",
                    AmmunitionType = AmmunitionType.Small,
                    PartNo = "PN-45ACP-M1911",
                    BulletDiameter = 11.43m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Large Pistol",
                    TotalWeight = 0.021m,
                    NatureOptionId = 1,
                    Nsn = "1305-00-179-6331",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 0.85m,
                    MinimumQuantity = 400,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "5.56mm Mk 262 Mod 1 (Match)",
                    ItemNo = "AMM-010",
                    AmmunitionType = AmmunitionType.Small,
                    PartNo = "PN-556-MK262",
                    BulletDiameter = 5.70m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Small Rifle Match",
                    TotalWeight = 0.013m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-534-5544",
                    PrimaryPurposId = 2,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 1.50m,
                    MinimumQuantity = 300,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "7.62x39mm Ball (AK-47)",
                    ItemNo = "AMM-011",
                    AmmunitionType = AmmunitionType.Medium,
                    PartNo = "PN-762x39-BALL",
                    BulletDiameter = 7.92m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Large Rifle",
                    TotalWeight = 0.016m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-234-5678",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 2,
                    PropellantId = 2,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 0.95m,
                    MinimumQuantity = 700,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "12 Gauge 00 Buckshot",
                    ItemNo = "AMM-012",
                    AmmunitionType = AmmunitionType.Large,
                    PartNo = "PN-12GA-00BUCK",
                    BulletDiameter = 18.5m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Large Shotgun",
                    TotalWeight = 0.034m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-345-6789",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 3,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 1.20m,
                    MinimumQuantity = 250,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = ".338 Lapua Magnum",
                    ItemNo = "AMM-013",
                    AmmunitionType = AmmunitionType.Large,
                    PartNo = "PN-338-LAPUA",
                    BulletDiameter = 8.58m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Large Rifle Magnum",
                    TotalWeight = 0.034m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-456-7890",
                    PrimaryPurposId = 2,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 3,
                    PropellantId = 3,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 4.50m,
                    MinimumQuantity = 150,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "40mm M433 HEDP Grenade",
                    ItemNo = "AMM-014",
                    AmmunitionType = AmmunitionType.Large,
                    PartNo = "PN-40MM-M433",
                    BulletDiameter = 40.0m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Electric",
                    TotalWeight = 0.230m,
                    NatureOptionId = 3,
                    Nsn = "1310-01-195-5648",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 2,
                    ProjectailMaterialId = 2,
                    CaseTypeId = 3,
                    PropellantId = 3,
                    CompatibilityId = 2,
                    HazardDivisionId = 2,
                    Price = 45.00m,
                    MinimumQuantity = 50,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Ammunition
                {
                    Name = "5.7x28mm SS190 AP",
                    ItemNo = "AMM-015",
                    AmmunitionType = AmmunitionType.Small,
                    PartNo = "PN-57-SS190",
                    BulletDiameter = 5.70m,
                    BulletDiameterUnitId = 1,
                    IsLinked = false,
                    Primer = "Small Pistol",
                    TotalWeight = 0.006m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-567-8901",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 3,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    Price = 1.85m,
                    MinimumQuantity = 300,
                    ItemType = ItemType.Ammunition,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                }
            };

            await context.Ammunitions.AddRangeAsync(ammunitions);
            await context.SaveChangesAsync();
        }

        private static async Task SeedWeaponDataAsync(ApplicationDbContext context)
        {
            // Check if weapons already exist
            if (await context.Weapons.AnyAsync())
            {
                return; // Already seeded
            }

            var utcNow = DateTime.Now;

            var weapons = new[]
            {
                new Weapon
                {
                    Name = "M16A4 Assault Rifle",
                    ItemNo = "WPN-001",
                    Caliber = "5.56x45mm NATO",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1997,
                    Model = "M16A4",
                    PartNo = "PN-WPN-001",
                    Nsn = "1005-01-000-0001",
                    Price = 1000.00m,
                    MinimumQuantity = 10,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M4 Carbine",
                    ItemNo = "WPN-002",
                    Caliber = "5.56x45mm NATO",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1994,
                    Model = "M4",
                    PartNo = "PN-WPN-002",
                    Nsn = "1005-01-000-0002",
                    Price = 850.00m,
                    MinimumQuantity = 15,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M249 Squad Automatic Weapon",
                    ItemNo = "WPN-003",
                    Caliber = "5.56x45mm NATO",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1984,
                    Model = "M249 SAW",
                    PartNo = "PN-WPN-003",
                    Nsn = "1005-01-000-0003",
                    Price = 5000.00m,
                    MinimumQuantity = 5,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M240B Machine Gun",
                    ItemNo = "WPN-004",
                    Caliber = "7.62x51mm NATO",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1977,
                    Model = "M240B",
                    PartNo = "PN-WPN-004",
                    Nsn = "1005-01-000-0004",
                    Price = 6000.00m,
                    MinimumQuantity = 5,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M9 Pistol",
                    ItemNo = "WPN-005",
                    Caliber = "9x19mm Parabellum",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1985,
                    Model = "M9",
                    PartNo = "PN-WPN-005",
                    Nsn = "1005-01-000-0005",
                    Price = 650.00m,
                    MinimumQuantity = 20,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M24 Sniper Weapon System",
                    ItemNo = "WPN-006",
                    Caliber = "7.62x51mm NATO",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1988,
                    Model = "M24 SWS",
                    PartNo = "PN-WPN-006",
                    Nsn = "1005-01-000-0006",
                    Price = 4000.00m,
                    MinimumQuantity = 8,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M2 Browning Machine Gun",
                    ItemNo = "WPN-007",
                    Caliber = ".50 BMG (12.7x99mm)",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1933,
                    Model = "M2HB",
                    PartNo = "PN-WPN-007",
                    Nsn = "1005-01-000-0007",
                    Price = 10000.00m,
                    MinimumQuantity = 3,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M203 Grenade Launcher",
                    ItemNo = "WPN-008",
                    Caliber = "40x46mm",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1969,
                    Model = "M203",
                    PartNo = "PN-WPN-008",
                    Nsn = "1005-01-000-0008",
                    Price = 2000.00m,
                    MinimumQuantity = 10,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M110 Semi-Automatic Sniper System",
                    ItemNo = "WPN-009",
                    Caliber = "7.62x51mm NATO",
                    CaliberUnitId = 1,
                    YearOfManufacture = 2007,
                    Model = "M110 SASS",
                    PartNo = "PN-WPN-009",
                    Nsn = "1005-01-000-0009",
                    Price = 5000.00m,
                    MinimumQuantity = 8,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "Remington 870 Shotgun",
                    ItemNo = "WPN-010",
                    Caliber = "12 Gauge",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1951,
                    Model = "870",
                    PartNo = "PN-WPN-010",
                    Nsn = "1005-01-000-0010",
                    Price = 450.00m,
                    MinimumQuantity = 15,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "MP5 Submachine Gun",
                    ItemNo = "WPN-011",
                    Caliber = "9x19mm Parabellum",
                    CaliberUnitId = 1,
                    YearOfManufacture = 1966,
                    Model = "MP5A3",
                    PartNo = "PN-WPN-011",
                    Nsn = "1005-01-000-0011",
                    Price = 1200.00m,
                    MinimumQuantity = 12,
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                }
            };

            await context.Weapons.AddRangeAsync(weapons);
            await context.SaveChangesAsync();
        }

        private static async Task SeedExplosiveDataAsync(ApplicationDbContext context)
        {
            // Check if explosives already exist
            if (await context.Explosives.AnyAsync())
            {
                return; // Already seeded
            }

            var utcNow = DateTime.Now;

            var explosives = new[]
            {
                new Explosive
                {
                    Name = "M67 Fragmentation Grenade",
                    ItemNo = "EXP-001",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0284",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-001",
                    Nsn = "1330-01-000-0001",
                    Price = 75.00m,
                    MinimumQuantity = 50,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M84 Stun Grenade",
                    ItemNo = "EXP-002",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0014",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-002",
                    Nsn = "1330-01-000-0002",
                    Price = 60.00m,
                    MinimumQuantity = 50,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M18 Smoke Grenade",
                    ItemNo = "EXP-003",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0015",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-003",
                    Nsn = "1330-01-000-0003",
                    Price = 45.00m,
                    MinimumQuantity = 50,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "C4 Explosive",
                    ItemNo = "EXP-004",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0056",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-004",
                    Nsn = "1330-01-000-0004",
                    Price = 300.00m,
                    MinimumQuantity = 20,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M112 Demolition Charge",
                    ItemNo = "EXP-005",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0048",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-005",
                    Nsn = "1330-01-000-0005",
                    Price = 225.00m,
                    MinimumQuantity = 25,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M26A2 Fragmentation Grenade",
                    ItemNo = "EXP-006",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0285",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-006",
                    Nsn = "1330-01-000-0006",
                    Price = 90.00m,
                    MinimumQuantity = 40,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "AN-M14 TH3 Incendiary Grenade",
                    ItemNo = "EXP-007",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0009",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-007",
                    Nsn = "1330-01-000-0007",
                    Price = 115.00m,
                    MinimumQuantity = 30,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M72 LAW Rocket",
                    ItemNo = "EXP-008",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0180",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-008",
                    Nsn = "1410-01-000-0008",
                    Price = 1500.00m,
                    MinimumQuantity = 10,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M18A1 Claymore Mine",
                    ItemNo = "EXP-009",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0137",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-009",
                    Nsn = "1345-01-000-0009",
                    Price = 300.00m,
                    MinimumQuantity = 20,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M67 Training Grenade",
                    ItemNo = "EXP-010",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0110",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-010",
                    Nsn = "1330-01-000-0010",
                    Price = 25.00m,
                    MinimumQuantity = 100,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M183 Demolition Charge Assembly",
                    ItemNo = "EXP-011",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0118",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-011",
                    Nsn = "1375-01-000-0011",
                    Price = 850.00m,
                    MinimumQuantity = 5,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M6 Electric Blasting Cap",
                    ItemNo = "EXP-012",
                    Unit = ExplosiveUnit.Gram,
                    UNNumber = "UN0030",
                    HazardDivisionId = 1,
                    PartNo = "PN-EXP-012",
                    Nsn = "1375-01-000-0012",
                    Price = 15.00m,
                    MinimumQuantity = 200,
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                }
            };

            await context.Explosives.AddRangeAsync(explosives);
            await context.SaveChangesAsync();
        }

        private static async Task SeedInventoryDataAsync(ApplicationDbContext context)
        {
            // Avoid reseeding if inventories already exist
            if (await context.Inventories.AnyAsync())
            {
                return;
            }

            // Ensure we have depots and items to seed inventories
            var depots = await context.Depots.Where(d => !d.IsDeleted).ToListAsync();
            var ammunitions = await context.Ammunitions.ToListAsync();
            var weapons = await context.Weapons.ToListAsync();
            var explosives = await context.Explosives.ToListAsync();
            var suppliers = await context.Suppliers.Where(s => !s.IsDeleted).ToListAsync();
            var manufacturers = await context.Manufacturers.Where(m => !m.IsDeleted).ToListAsync();
            var countries = await context.Countries.Where(c => !c.IsDeleted).ToListAsync();

            if (!depots.Any() || (!ammunitions.Any() && !weapons.Any() && !explosives.Any()))
            {
                return;
            }

            var utcNow = DateTime.Now;
            var random = new Random(2025);
            var inventories = new List<Inventory>();
            int invoiceSequence = 1;
            int lotSequence = 1000;

            foreach (var depot in depots)
            {
                // Create ONE comprehensive inventory per depot with ALL items
                var inventory = new Inventory
                {
                    DepoId = depot.Id,
                    InvoiceNumber = $"INV-{invoiceSequence:0000}",
                    InvoiceDate = utcNow.AddDays(-random.Next(30, 120)),
                    RecievedDate = utcNow.AddDays(-random.Next(5, 60)),
                    Notes = $"Seeded inventory for depot {depot.Code}",
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM",
                    InventoryDetails = new List<InventoryDetail>()
                };

                invoiceSequence++;

                // Add ALL ammunitions to this depot
                if (ammunitions.Any())
                {
                    foreach (var ammo in ammunitions)
                    {
                        // Generate expiry date: between 1 to 5 years from received date
                        var receivedDate = inventory.RecievedDate;
                        var expiryDate = receivedDate?.AddYears(random.Next(1, 6)).AddDays(random.Next(0, 365));

                        // Randomly select supplier, manufacturer, and country if available
                        var supplier = suppliers.Any() ? suppliers[random.Next(suppliers.Count)] : null;
                        var manufacturer = manufacturers.Any() ? manufacturers[random.Next(manufacturers.Count)] : null;
                        var country = countries.Any() ? countries[random.Next(countries.Count)] : null;

                        inventory.InventoryDetails.Add(new InventoryDetail
                        {
                            ItemId = ammo.Id,
                            Lot = ++lotSequence,
                            ItemQuantity = random.Next(5000, 25000), // Realistic military stockpile
                            SupplierId = supplier?.Id,
                            ManufacturerId = manufacturer?.Id,
                            CountryId = country?.Id,
                            ExpiryDate = expiryDate,
                            IsLotEmpty = false
                        });
                    }
                }

                // Add ALL weapons to this depot
                if (weapons.Any())
                {
                    foreach (var weapon in weapons)
                    {
                        // Generate expiry date: weapons typically don't expire, but we can set a maintenance/inspection date
                        var receivedDate = inventory.RecievedDate;
                        var expiryDate = receivedDate?.AddYears(random.Next(5, 15)).AddDays(random.Next(0, 365));

                        // Randomly select supplier, manufacturer, and country if available
                        var supplier = suppliers.Any() ? suppliers[random.Next(suppliers.Count)] : null;
                        var manufacturer = manufacturers.Any() ? manufacturers[random.Next(manufacturers.Count)] : null;
                        var country = countries.Any() ? countries[random.Next(countries.Count)] : null;

                        inventory.InventoryDetails.Add(new InventoryDetail
                        {
                            ItemId = weapon.Id,
                            Lot = ++lotSequence,
                            ItemQuantity = random.Next(100, 500), // Realistic military stockpile
                            SupplierId = supplier?.Id,
                            ManufacturerId = manufacturer?.Id,
                            CountryId = country?.Id,
                            ExpiryDate = expiryDate,
                            IsLotEmpty = false
                        });
                    }
                }

                // Add ALL explosives to this depot
                if (explosives.Any())
                {
                    foreach (var explosive in explosives)
                    {
                        // Generate expiry date: between 2 to 7 years from received date
                        var receivedDate = inventory.RecievedDate;
                        var expiryDate = receivedDate?.AddYears(random.Next(2, 8)).AddDays(random.Next(0, 365));

                        // Randomly select supplier, manufacturer, and country if available
                        var supplier = suppliers.Any() ? suppliers[random.Next(suppliers.Count)] : null;
                        var manufacturer = manufacturers.Any() ? manufacturers[random.Next(manufacturers.Count)] : null;
                        var country = countries.Any() ? countries[random.Next(countries.Count)] : null;

                        inventory.InventoryDetails.Add(new InventoryDetail
                        {
                            ItemId = explosive.Id,
                            Lot = ++lotSequence,
                            ItemQuantity = random.Next(1000, 5000), // Realistic military stockpile
                            SupplierId = supplier?.Id,
                            ManufacturerId = manufacturer?.Id,
                            CountryId = country?.Id,
                            ExpiryDate = expiryDate,
                            IsLotEmpty = false
                        });
                    }
                }

                inventories.Add(inventory);
            }

            await context.Inventories.AddRangeAsync(inventories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedAllowanceDataAsync(ApplicationDbContext context)
        {
            // Check if allowances already exist
            if (await context.AllowanceItems.AnyAsync())
            {
                return; // Already seeded
            }

            // Get all departments
            var departments = await context.Departments
                .Where(d => !d.IsDeleted)
                .ToListAsync();

            if (!departments.Any())
            {
                return; // No departments to seed allowances for
            }

            // Get all items (Ammunition, Weapons, Explosives)
            var ammunitions = await context.Ammunitions
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            var weapons = await context.Weapons
                .Where(w => !w.IsDeleted)
                .ToListAsync();

            var explosives = await context.Explosives
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            if (!ammunitions.Any() && !weapons.Any() && !explosives.Any())
            {
                return; // No items to create allowances for
            }

            // Define different quantities for each ammunition item by ItemNo
            var ammunitionQuantities = new Dictionary<string, int>
            {
                { "AMM-001", 15000 },  // 9mm NATO Ball Ammunition
                { "AMM-002", 25000 },  // 5.56mm M855 Ball (Green Tip)
                { "AMM-003", 12000 },  // 5.56mm M856 Tracer (Red Tip)
                { "AMM-004", 20000 },  // 7.62mm M80 Ball
                { "AMM-005", 15000 },  // 7.62mm M62 Tracer
                { "AMM-006", 8000 },   // 7.62mm M118LR Sniper
                { "AMM-007", 5000 },   // .50 BMG M33 Ball
                { "AMM-008", 3000 },   // .50 BMG M2 AP
                { "AMM-009", 10000 },  // .45 ACP Ball M1911
                { "AMM-010", 7000 },   // 5.56mm Mk 262 Mod 1
                { "AMM-011", 18000 },  // 7.62x39mm Ball (AK-47)
                { "AMM-012", 6000 },   // 12 Gauge 00 Buckshot
                { "AMM-013", 4000 },   // .338 Lapua Magnum
                { "AMM-014", 1500 },   // 40mm M433 HEDP Grenade
                { "AMM-015", 8000 }    // 5.7x28mm SS190 AP
            };

            // Define different quantities for each weapon item by ItemNo
            var weaponQuantities = new Dictionary<string, int>
            {
                { "WPN-001", 500 },   // M16A4 Assault Rifle
                { "WPN-002", 600 },   // M4 Carbine
                { "WPN-003", 250 },   // M249 Squad Automatic Weapon
                { "WPN-004", 200 },   // M240B Machine Gun
                { "WPN-005", 800 },   // M9 Pistol
                { "WPN-006", 300 },   // M24 Sniper Weapon System
                { "WPN-007", 150 },   // M2 Browning Machine Gun
                { "WPN-008", 400 },   // M203 Grenade Launcher
                { "WPN-009", 250 },   // M110 Semi-Automatic Sniper System
                { "WPN-010", 450 },   // Remington 870 Shotgun
                { "WPN-011", 350 }    // MP5 Submachine Gun
            };

            // Define different quantities for each explosive item by ItemNo
            var explosiveQuantities = new Dictionary<string, int>
            {
                { "EXP-001", 5000 },  // M67 Fragmentation Grenade
                { "EXP-002", 4000 },  // M84 Stun Grenade
                { "EXP-003", 4500 },  // M18 Smoke Grenade
                { "EXP-004", 2000 },  // C4 Explosive
                { "EXP-005", 2500 },  // M112 Demolition Charge
                { "EXP-006", 3500 },  // M26A2 Fragmentation Grenade
                { "EXP-007", 3000 },  // AN-M14 TH3 Incendiary Grenade
                { "EXP-008", 1000 },  // M72 LAW Rocket
                { "EXP-009", 1500 },  // M18A1 Claymore Mine
                { "EXP-010", 8000 },  // M67 Training Grenade
                { "EXP-011", 500 },   // M183 Demolition Charge Assembly
                { "EXP-012", 10000 }  // M6 Electric Blasting Cap
            };

            var utcNow = DateTime.Now;
            var currentYear = utcNow.Year;
            var allowanceItems = new List<AllowanceItem>();

            // Create allowances for each department and each item type
            foreach (var department in departments)
            {
                // Ammunition allowances
                foreach (var ammunition in ammunitions)
                {
                    // Get quantity from dictionary, default to 100 if not found
                    var quantity = ammunitionQuantities.TryGetValue(ammunition.ItemNo, out int qty) 
                        ? qty 
                        : 100;

                    allowanceItems.Add(new AllowanceItem
                    {
                        ItemId = ammunition.Id,
                        DepartmentId = department.Id,
                        Year = currentYear,
                        Quantity = quantity,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    });
                }

                // Weapon allowances
                foreach (var weapon in weapons)
                {
                    // Get quantity from dictionary, default to 10 if not found
                    var quantity = weaponQuantities.TryGetValue(weapon.ItemNo, out int qty) 
                        ? qty 
                        : 10;

                    allowanceItems.Add(new AllowanceItem
                    {
                        ItemId = weapon.Id,
                        DepartmentId = department.Id,
                        Year = currentYear,
                        Quantity = quantity,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    });
                }

                // Explosive allowances
                foreach (var explosive in explosives)
                {
                    // Get quantity from dictionary, default to 50 if not found
                    var quantity = explosiveQuantities.TryGetValue(explosive.ItemNo, out int qty) 
                        ? qty 
                        : 50;

                    allowanceItems.Add(new AllowanceItem
                    {
                        ItemId = explosive.Id,
                        DepartmentId = department.Id,
                        Year = currentYear,
                        Quantity = quantity,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    });
                }
            }

            await context.AllowanceItems.AddRangeAsync(allowanceItems);
            await context.SaveChangesAsync();
        }
    }
}
