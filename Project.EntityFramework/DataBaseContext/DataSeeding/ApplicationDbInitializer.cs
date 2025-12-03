using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using System.Collections.Generic;

namespace Ettad.EntityFramework.DataBaseContext.DataSeeding
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedDefaultDataAsync(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                
                // Seed countries data
                await SeedCountriesDataAsync(context);
                
                // Seed suppliers data
                await SeedSuppliersDataAsync(context);
                
                // Seed manufacturers data
                await SeedManufacturersDataAsync(context);
                
                // Seed ammunition data
                await SeedAmmunitionDataAsync(context);
                
                // Seed weapon data
                await SeedWeaponDataAsync(context);
                
                // Seed explosive data
                await SeedExplosiveDataAsync(context);
                
                // Seed inventory data based on seeded depots and ammunitions
                await SeedInventoryDataAsync(context);
                
                // Seed allowance data for all departments and all items
                await SeedAllowanceDataAsync(context);
            }
            catch (Exception)
            {
                // swallow for startup; logs handled by outer try/catch
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

            var utcNow = DateTime.UtcNow;

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

            var utcNow = DateTime.UtcNow;

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
                    HccId = 1,
                    PartNo = "PN-001",
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
                    HccId = 2,
                    PartNo = "PN-002",
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
                    HccId = 3,
                    PartNo = "PN-003",
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

        private static async Task SeedWeaponDataAsync(ApplicationDbContext context)
        {
            // Check if weapons already exist
            if (await context.Weapons.AnyAsync())
            {
                return; // Already seeded
            }

            var utcNow = DateTime.UtcNow;

            var weapons = new[]
            {
                new Weapon
                {
                    Name = "M16A4 Assault Rifle",
                    ItemNo = "WPN-001",
                    HccId = 1,
                    PartNo = "PN-WPN-001",
                    Nsn = "1005-01-000-0001",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M4 Carbine",
                    ItemNo = "WPN-002",
                    HccId = 2,
                    PartNo = "PN-WPN-002",
                    Nsn = "1005-01-000-0002",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M249 Squad Automatic Weapon",
                    ItemNo = "WPN-003",
                    HccId = 3,
                    PartNo = "PN-WPN-003",
                    Nsn = "1005-01-000-0003",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M240B Machine Gun",
                    ItemNo = "WPN-004",
                    HccId = 1,
                    PartNo = "PN-WPN-004",
                    Nsn = "1005-01-000-0004",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M9 Pistol",
                    ItemNo = "WPN-005",
                    HccId = 2,
                    PartNo = "PN-WPN-005",
                    Nsn = "1005-01-000-0005",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M24 Sniper Weapon System",
                    ItemNo = "WPN-006",
                    HccId = 3,
                    PartNo = "PN-WPN-006",
                    Nsn = "1005-01-000-0006",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M2 Browning Machine Gun",
                    ItemNo = "WPN-007",
                    HccId = 1,
                    PartNo = "PN-WPN-007",
                    Nsn = "1005-01-000-0007",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M203 Grenade Launcher",
                    ItemNo = "WPN-008",
                    HccId = 2,
                    PartNo = "PN-WPN-008",
                    Nsn = "1005-01-000-0008",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M110 Semi-Automatic Sniper System",
                    ItemNo = "WPN-009",
                    HccId = 3,
                    PartNo = "PN-WPN-009",
                    Nsn = "1005-01-000-0009",
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

            var utcNow = DateTime.UtcNow;

            var explosives = new[]
            {
                new Explosive
                {
                    Name = "M67 Fragmentation Grenade",
                    ItemNo = "EXP-001",
                    HccId = 1,
                    PartNo = "PN-EXP-001",
                    Nsn = "1330-01-000-0001",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M84 Stun Grenade",
                    ItemNo = "EXP-002",
                    HccId = 2,
                    PartNo = "PN-EXP-002",
                    Nsn = "1330-01-000-0002",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M18 Smoke Grenade",
                    ItemNo = "EXP-003",
                    HccId = 3,
                    PartNo = "PN-EXP-003",
                    Nsn = "1330-01-000-0003",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "C4 Explosive",
                    ItemNo = "EXP-004",
                    HccId = 1,
                    PartNo = "PN-EXP-004",
                    Nsn = "1330-01-000-0004",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M112 Demolition Charge",
                    ItemNo = "EXP-005",
                    HccId = 2,
                    PartNo = "PN-EXP-005",
                    Nsn = "1330-01-000-0005",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M26A2 Fragmentation Grenade",
                    ItemNo = "EXP-006",
                    HccId = 3,
                    PartNo = "PN-EXP-006",
                    Nsn = "1330-01-000-0006",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "AN-M14 TH3 Incendiary Grenade",
                    ItemNo = "EXP-007",
                    HccId = 1,
                    PartNo = "PN-EXP-007",
                    Nsn = "1330-01-000-0007",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M72 LAW Rocket",
                    ItemNo = "EXP-008",
                    HccId = 2,
                    PartNo = "PN-EXP-008",
                    Nsn = "1410-01-000-0008",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M18A1 Claymore Mine",
                    ItemNo = "EXP-009",
                    HccId = 3,
                    PartNo = "PN-EXP-009",
                    Nsn = "1345-01-000-0009",
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

            // Ensure we have depots and ammunitions to seed inventories
            var depots = await context.Depots.Where(d => !d.IsDeleted).ToListAsync();
            var ammunitions = await context.Ammunitions.ToListAsync();
            var suppliers = await context.Suppliers.Where(s => !s.IsDeleted).ToListAsync();
            var manufacturers = await context.Manufacturers.Where(m => !m.IsDeleted).ToListAsync();
            var countries = await context.Countries.Where(c => !c.IsDeleted).ToListAsync();

            if (!depots.Any() || !ammunitions.Any())
            {
                return;
            }

            var utcNow = DateTime.UtcNow;
            var random = new Random(2025);
            var inventories = new List<Inventory>();
            int invoiceSequence = 1;
            int lotSequence = 1000;

            foreach (var depot in depots)
            {
                // Create multiple inventories per depot
                var inventoriesPerDepot = Math.Min(3, Math.Max(2, ammunitions.Count / 2));

                for (int i = 0; i < inventoriesPerDepot; i++)
                {
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

                    // Take a random subset of ammunitions for this inventory
                    var ammoSelection = ammunitions
                        .OrderBy(_ => random.Next())
                        .Take(Math.Min(3, ammunitions.Count))
                        .ToList();

                    foreach (var ammo in ammoSelection)
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
                            ItemQuantity = random.Next(150, 600),
                            SupplierId = supplier?.Id,
                            ManufacturerId = manufacturer?.Id,
                            CountryId = country?.Id,
                            ExpiryDate = expiryDate,
                            IsLotEmpty = false
                        });
                    }

                    inventories.Add(inventory);
                }
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

            // Get all Ammunition items only
            var ammunitions = await context.Ammunitions
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            if (!ammunitions.Any())
            {
                return; // No ammunition items to create allowances for
            }

            // Define different quantities for each ammunition item by ItemNo
            var ammunitionQuantities = new Dictionary<string, int>
            {
                { "AMM-001", 150 }, // 9mm NATO Ball Ammunition - 150 units
                { "AMM-002", 200 }, // 7.62mm NATO Match Ammunition - 200 units
                { "AMM-003", 120 }  // 5.56mm NATO Tracer Round - 120 units
            };

            var utcNow = DateTime.UtcNow;
            var currentYear = utcNow.Year;
            var allowanceItems = new List<AllowanceItem>();

            // Create allowances for each department and each ammunition item
            foreach (var department in departments)
            {
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
            }

            await context.AllowanceItems.AddRangeAsync(allowanceItems);
            await context.SaveChangesAsync();
        }
    }
}
