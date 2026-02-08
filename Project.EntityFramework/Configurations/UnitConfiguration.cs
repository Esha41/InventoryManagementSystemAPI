using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Units");

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ItemType)
                .HasConversion<int>();

            // Composite unique indexes (like ItemTypeLookup pattern)
            builder.HasIndex(x => new { x.NameAr, x.ItemType })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(x => new { x.NameEn, x.ItemType })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Configure CreationDate to use database default - prevents EF Core from comparing it in seeded data
            builder.Property(x => x.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            // Seed data with ItemType
            builder.HasData(
                new Unit
                {
                    Id = 1,
                    NameAr = "غرام",
                    NameEn = "Gram",
                    ItemType = ItemType.Ammunition,
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 2,
                    NameAr = "مليمتر",
                    NameEn = "Millimeter",
                    ItemType = ItemType.Ammunition,
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 3,
                    NameAr = "قطعة",
                    NameEn = "Piece",
                    ItemType = ItemType.Ammunition,
                    IsDeleted = false
                },
                // Units for Weapon
                new Unit
                {
                    Id = 4,
                    NameAr = "غرام",
                    NameEn = "Gram",
                    ItemType = ItemType.Weapon,
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 5,
                    NameAr = "مليمتر",
                    NameEn = "Millimeter",
                    ItemType = ItemType.Weapon,
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 6,
                    NameAr = "قطعة",
                    NameEn = "Piece",
                    ItemType = ItemType.Weapon,
                    IsDeleted = false
                },
                // Units for Explosive
                new Unit
                {
                    Id = 7,
                    NameAr = "غرام",
                    NameEn = "Gram",
                    ItemType = ItemType.Explosive,
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 8,
                    NameAr = "متر",
                    NameEn = "Meter",
                    ItemType = ItemType.Explosive,
                    IsDeleted = false
                }
            );
        }
    }
}
