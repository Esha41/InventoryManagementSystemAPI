using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AmmunitionConfiguration : IEntityTypeConfiguration<Ammunition>
    {
        public void Configure(EntityTypeBuilder<Ammunition> builder)
        {
            builder.ToTable("Ammunitions");

            builder.Property(x => x.BulletDiameter)
                .IsRequired();

            builder.Property(x => x.CaseLength)
                .IsRequired();

            builder.HasOne(x => x.NatureOption)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.NatureOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PrimaryPurpos)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PrimaryPurposId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProjectileColor)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ProjectileColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProjectailMaterial)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ProjectailMaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CaseType)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CaseTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Propellant)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PropellantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Compatibility)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CompatibilityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.HazardDivision)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.HazardDivisionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CaseLengthUnit)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.CaseLengthUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BulletDiameterUnit)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.BulletDiameterUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.HasData(
                new Ammunition
                {
                    Id = 1,
                    Name = "5.56x45mm NATO",
                    ItemNo = "AMM-001",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-001",
                    HccId = 1,
                    PartNo = "PN-556-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(5),
                    BulletDiameter = 5.56m,
                    BulletDiameterUnitId = 1,
                    CaseLength = 45.0m,
                    CaseLengthUnitId = 1,
                    IsLinked = false,
                    Primer = "Boxer",
                    TotalWeight = 12.0m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-000-0001",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    CreationDate = seedDate,
                    IsDeleted = false
                },
                new Ammunition
                {
                    Id = 2,
                    Name = "7.62x51mm NATO",
                    ItemNo = "AMM-002",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-002",
                    HccId = 2,
                    PartNo = "PN-762-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(5),
                    BulletDiameter = 7.62m,
                    BulletDiameterUnitId = 2,
                    CaseLength = 51.0m,
                    CaseLengthUnitId = 2,
                    IsLinked = false,
                    Primer = "Berdan",
                    TotalWeight = 24.0m,
                    NatureOptionId = 2,
                    Nsn = "1305-01-000-0002",
                    PrimaryPurposId = 2,
                    ProjectileColorId = 2,
                    ProjectailMaterialId = 2,
                    CaseTypeId = 2,
                    PropellantId = 2,
                    CompatibilityId = 2,
                    HazardDivisionId = 2,
                    CreationDate = seedDate,
                    IsDeleted = false
                },
                new Ammunition
                {
                    Id = 3,
                    Name = "9x19mm Parabellum",
                    ItemNo = "AMM-003",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-003",
                    HccId = 3,
                    PartNo = "PN-9MM-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(4),
                    BulletDiameter = 9.0m,
                    BulletDiameterUnitId = 3,
                    CaseLength = 19.0m,
                    CaseLengthUnitId = 3,
                    IsLinked = false,
                    Primer = "Boxer",
                    TotalWeight = 7.5m,
                    NatureOptionId = 3,
                    Nsn = "1305-01-000-0003",
                    PrimaryPurposId = 3,
                    ProjectileColorId = 3,
                    ProjectailMaterialId = 3,
                    CaseTypeId = 3,
                    PropellantId = 3,
                    CompatibilityId = 3,
                    HazardDivisionId = 3,
                    CreationDate = seedDate,
                    IsDeleted = false
                },
                new Ammunition
                {
                    Id = 4,
                    Name = ".50 BMG",
                    ItemNo = "AMM-004",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-004",
                    HccId = 1,
                    PartNo = "PN-50BMG-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(6),
                    BulletDiameter = 12.7m,
                    BulletDiameterUnitId = 1,
                    CaseLength = 99.0m,
                    CaseLengthUnitId = 1,
                    IsLinked = false,
                    Primer = "Berdan",
                    TotalWeight = 115.0m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-000-0004",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    CreationDate = seedDate,
                    IsDeleted = false
                },
                new Ammunition
                {
                    Id = 5,
                    Name = ".308 Winchester",
                    ItemNo = "AMM-005",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-005",
                    HccId = 2,
                    PartNo = "PN-308-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(5),
                    BulletDiameter = 7.62m,
                    BulletDiameterUnitId = 2,
                    CaseLength = 51.0m,
                    CaseLengthUnitId = 2,
                    IsLinked = false,
                    Primer = "Boxer",
                    TotalWeight = 23.0m,
                    NatureOptionId = 2,
                    Nsn = "1305-01-000-0005",
                    PrimaryPurposId = 2,
                    ProjectileColorId = 2,
                    ProjectailMaterialId = 2,
                    CaseTypeId = 2,
                    PropellantId = 2,
                    CompatibilityId = 2,
                    HazardDivisionId = 2,
                    CreationDate = seedDate,
                    IsDeleted = false
                },
                new Ammunition
                {
                    Id = 6,
                    Name = ".45 ACP",
                    ItemNo = "AMM-006",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-006",
                    HccId = 3,
                    PartNo = "PN-45ACP-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(4),
                    BulletDiameter = 11.43m,
                    BulletDiameterUnitId = 3,
                    CaseLength = 23.0m,
                    CaseLengthUnitId = 3,
                    IsLinked = false,
                    Primer = "Boxer",
                    TotalWeight = 15.0m,
                    NatureOptionId = 3,
                    Nsn = "1305-01-000-0006",
                    PrimaryPurposId = 3,
                    ProjectileColorId = 3,
                    ProjectailMaterialId = 3,
                    CaseTypeId = 3,
                    PropellantId = 3,
                    CompatibilityId = 3,
                    HazardDivisionId = 3,
                    CreationDate = seedDate,
                    IsDeleted = false
                },
                new Ammunition
                {
                    Id = 7,
                    Name = "12.7x108mm",
                    ItemNo = "AMM-007",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-007",
                    HccId = 1,
                    PartNo = "PN-127-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(6),
                    BulletDiameter = 12.7m,
                    BulletDiameterUnitId = 1,
                    CaseLength = 108.0m,
                    CaseLengthUnitId = 1,
                    IsLinked = false,
                    Primer = "Berdan",
                    TotalWeight = 130.0m,
                    NatureOptionId = 1,
                    Nsn = "1305-01-000-0007",
                    PrimaryPurposId = 1,
                    ProjectileColorId = 1,
                    ProjectailMaterialId = 1,
                    CaseTypeId = 1,
                    PropellantId = 1,
                    CompatibilityId = 1,
                    HazardDivisionId = 1,
                    CreationDate = seedDate,
                    IsDeleted = false
                },
                new Ammunition
                {
                    Id = 8,
                    Name = "5.45x39mm",
                    ItemNo = "AMM-008",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-008",
                    HccId = 2,
                    PartNo = "PN-545-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(5),
                    BulletDiameter = 5.45m,
                    BulletDiameterUnitId = 2,
                    CaseLength = 39.0m,
                    CaseLengthUnitId = 2,
                    IsLinked = false,
                    Primer = "Berdan",
                    TotalWeight = 10.5m,
                    NatureOptionId = 2,
                    Nsn = "1305-01-000-0008",
                    PrimaryPurposId = 2,
                    ProjectileColorId = 2,
                    ProjectailMaterialId = 2,
                    CaseTypeId = 2,
                    PropellantId = 2,
                    CompatibilityId = 2,
                    HazardDivisionId = 2,
                    CreationDate = seedDate,
                    IsDeleted = false
                },
                new Ammunition
                {
                    Id = 9,
                    Name = ".40 S&W",
                    ItemNo = "AMM-009",
                    ItemType = ItemType.Ammunition,
                    BatchNo = "BATCH-2024-009",
                    HccId = 3,
                    PartNo = "PN-40SW-001",
                    ReadyForIssue = true,
                    ExpiryDate = seedDate.AddYears(4),
                    BulletDiameter = 10.16m,
                    BulletDiameterUnitId = 3,
                    CaseLength = 21.6m,
                    CaseLengthUnitId = 3,
                    IsLinked = false,
                    Primer = "Boxer",
                    TotalWeight = 11.0m,
                    NatureOptionId = 3,
                    Nsn = "1305-01-000-0009",
                    PrimaryPurposId = 3,
                    ProjectileColorId = 3,
                    ProjectailMaterialId = 3,
                    CaseTypeId = 3,
                    PropellantId = 3,
                    CompatibilityId = 3,
                    HazardDivisionId = 3,
                    CreationDate = seedDate,
                    IsDeleted = false
                }
            );
        }
    }
}
