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
                
                // Seed ammunition data
                await SeedAmmunitionDataAsync(context);
                
                // Seed weapon data
                await SeedWeaponDataAsync(context);
                
                // Seed explosive data
                await SeedExplosiveDataAsync(context);

                // Seed inventory data based on seeded depots and ammunitions
                await SeedInventoryDataAsync(context);
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
                    BatchNo = "BATCH-001",
                    HccId = 1,
                    PartNo = "PN-WPN-001",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
                    Nsn = "1005-01-000-0001",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M4 Carbine",
                    ItemNo = "WPN-002",
                    BatchNo = "BATCH-002",
                    HccId = 2,
                    PartNo = "PN-WPN-002",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
                    Nsn = "1005-01-000-0002",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M249 Squad Automatic Weapon",
                    ItemNo = "WPN-003",
                    BatchNo = "BATCH-003",
                    HccId = 3,
                    PartNo = "PN-WPN-003",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
                    Nsn = "1005-01-000-0003",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M240B Machine Gun",
                    ItemNo = "WPN-004",
                    BatchNo = "BATCH-004",
                    HccId = 1,
                    PartNo = "PN-WPN-004",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
                    Nsn = "1005-01-000-0004",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M9 Pistol",
                    ItemNo = "WPN-005",
                    BatchNo = "BATCH-005",
                    HccId = 2,
                    PartNo = "PN-WPN-005",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
                    Nsn = "1005-01-000-0005",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M24 Sniper Weapon System",
                    ItemNo = "WPN-006",
                    BatchNo = "BATCH-006",
                    HccId = 3,
                    PartNo = "PN-WPN-006",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
                    Nsn = "1005-01-000-0006",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M2 Browning Machine Gun",
                    ItemNo = "WPN-007",
                    BatchNo = "BATCH-007",
                    HccId = 1,
                    PartNo = "PN-WPN-007",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
                    Nsn = "1005-01-000-0007",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M203 Grenade Launcher",
                    ItemNo = "WPN-008",
                    BatchNo = "BATCH-008",
                    HccId = 2,
                    PartNo = "PN-WPN-008",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
                    Nsn = "1005-01-000-0008",
                    ItemType = ItemType.Weapon,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Weapon
                {
                    Name = "M110 Semi-Automatic Sniper System",
                    ItemNo = "WPN-009",
                    BatchNo = "BATCH-009",
                    HccId = 3,
                    PartNo = "PN-WPN-009",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(10),
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
                    BatchNo = "BATCH-001",
                    HccId = 1,
                    PartNo = "PN-EXP-001",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(5),
                    Nsn = "1330-01-000-0001",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M84 Stun Grenade",
                    ItemNo = "EXP-002",
                    BatchNo = "BATCH-002",
                    HccId = 2,
                    PartNo = "PN-EXP-002",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(5),
                    Nsn = "1330-01-000-0002",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M18 Smoke Grenade",
                    ItemNo = "EXP-003",
                    BatchNo = "BATCH-003",
                    HccId = 3,
                    PartNo = "PN-EXP-003",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(5),
                    Nsn = "1330-01-000-0003",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "C4 Explosive",
                    ItemNo = "EXP-004",
                    BatchNo = "BATCH-004",
                    HccId = 1,
                    PartNo = "PN-EXP-004",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(3),
                    Nsn = "1330-01-000-0004",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M112 Demolition Charge",
                    ItemNo = "EXP-005",
                    BatchNo = "BATCH-005",
                    HccId = 2,
                    PartNo = "PN-EXP-005",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(3),
                    Nsn = "1330-01-000-0005",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M26A2 Fragmentation Grenade",
                    ItemNo = "EXP-006",
                    BatchNo = "BATCH-006",
                    HccId = 3,
                    PartNo = "PN-EXP-006",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(5),
                    Nsn = "1330-01-000-0006",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "AN-M14 TH3 Incendiary Grenade",
                    ItemNo = "EXP-007",
                    BatchNo = "BATCH-007",
                    HccId = 1,
                    PartNo = "PN-EXP-007",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(4),
                    Nsn = "1330-01-000-0007",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M72 LAW Rocket",
                    ItemNo = "EXP-008",
                    BatchNo = "BATCH-008",
                    HccId = 2,
                    PartNo = "PN-EXP-008",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(5),
                    Nsn = "1410-01-000-0008",
                    ItemType = ItemType.Explosive,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new Explosive
                {
                    Name = "M18A1 Claymore Mine",
                    ItemNo = "EXP-009",
                    BatchNo = "BATCH-009",
                    HccId = 3,
                    PartNo = "PN-EXP-009",
                    ReadyForIssue = true,
                    ExpiryDate = utcNow.AddYears(5),
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
            var utcNow = DateTime.UtcNow;

            // Get seeded items
            var ammunitions = await context.Ammunitions.Where(a => !a.IsDeleted).Take(3).ToListAsync();
            var weapons = await context.Weapons.Where(w => !w.IsDeleted).Take(2).ToListAsync();
            var explosives = await context.Explosives.Where(e => !e.IsDeleted).Take(2).ToListAsync();

            // Get depots (specifically the first one - Doha Central Depot)
            var depots = await context.Depots.Where(d => !d.IsDeleted).Take(2).ToListAsync();

            if (depots.Count == 0)
            {
                return; // No depots to seed
            }

            if (ammunitions.Count == 0 && weapons.Count == 0 && explosives.Count == 0)
            {
                return; // No items to seed
            }

            // Get first supplier, manufacturer, and country if they exist (make nullable)
            var firstSupplier = await context.Suppliers.Where(s => !s.IsDeleted).FirstOrDefaultAsync();
            var firstManufacturer = await context.Manufacturers.Where(m => !m.IsDeleted).FirstOrDefaultAsync();
            var firstCountry = await context.Countries.Where(c => !c.IsDeleted).FirstOrDefaultAsync();

            var inventories = new List<Inventory>();
            var random = new Random();

            // Create inventory for each depot
            foreach (var depot in depots)
            {
                // Check if inventory already exists for this depot
                var existingInventory = await context.Inventories
                    .Where(i => i.DepoId == depot.Id && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingInventory != null)
                {
                    continue; // Skip if inventory already exists for this depot
                }

                var inventoryDetails = new List<InventoryDetail>();

                // Add ammunition items
                foreach (var ammo in ammunitions)
                {
                    inventoryDetails.Add(new InventoryDetail
                    {
                        ItemId = ammo.Id,
                        Lot = 1,
                        ItemQuantity = random.Next(100, 1000),
                        SupplierId = firstSupplier?.Id,
                        ManufacturerId = firstManufacturer?.Id,
                        CountryId = firstCountry?.Id
                    });
                }

                // Add weapon items
                foreach (var weapon in weapons)
                {
                    inventoryDetails.Add(new InventoryDetail
                    {
                        ItemId = weapon.Id,
                        Lot = 1,
                        ItemQuantity = random.Next(10, 50),
                        SupplierId = firstSupplier?.Id,
                        ManufacturerId = firstManufacturer?.Id,
                        CountryId = firstCountry?.Id
                    });
                }

                // Add explosive items
                foreach (var explosive in explosives)
                {
                    inventoryDetails.Add(new InventoryDetail
                    {
                        ItemId = explosive.Id,
                        Lot = 1,
                        ItemQuantity = random.Next(20, 100),
                        SupplierId = firstSupplier?.Id,
                        ManufacturerId = firstManufacturer?.Id,
                        CountryId = firstCountry?.Id
                    });
                }

                if (inventoryDetails.Count > 0)
                {
                    var inventory = new Inventory
                    {
                        DepoId = depot.Id,
                        InvoiceNumber = $"INV-{depot.Code}-{DateTime.UtcNow:yyyyMMdd}",
                        InvoiceDate = utcNow.AddDays(-30),
                        RecievedDate = utcNow.AddDays(-25),
                        Notes = $"Initial inventory for {depot.NameEn}",
                        InventoryDetails = inventoryDetails,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    };

                    inventories.Add(inventory);
                }
            }

            if (inventories.Count > 0)
            {
                await context.Inventories.AddRangeAsync(inventories);
                await context.SaveChangesAsync();
            }
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
                        inventory.InventoryDetails.Add(new InventoryDetail
                        {
                            ItemId = ammo.Id,
                            Lot = ++lotSequence,
                            ItemQuantity = random.Next(150, 600),
                            SupplierId = null,
                            ManufacturerId = null,
                            CountryId = null,
                            IsLotEmpty = false
                        });
                    }

                    inventories.Add(inventory);
                }
            }

            await context.Inventories.AddRangeAsync(inventories);
            await context.SaveChangesAsync();
        }
    }
}
