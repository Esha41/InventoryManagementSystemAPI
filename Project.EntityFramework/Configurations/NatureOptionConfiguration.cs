using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class NatureOptionConfiguration : IEntityTypeConfiguration<NatureOption>
    {
        public void Configure(EntityTypeBuilder<NatureOption> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("NatureOptions");

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameAr)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Configure CreationDate to use database default - prevents EF Core from comparing it in seeded data
            builder.Property(x => x.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            // Seed data
            builder.HasData(
                new NatureOption
                {
                    Id = 1,
                    NameAr = "ذخيرة حية",
                    NameEn = "Live Ammunition",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 2,
                    NameAr = "صوتي",
                    NameEn = "Sonic",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 3,
                    NameAr = "مشرح",
                    NameEn = "Fragmentation",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 4,
                    NameAr = "كاشف",
                    NameEn = "Detector",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 5,
                    NameAr = "مائت",
                    NameEn = "Inert",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 6,
                    NameAr = "متفجر",
                    NameEn = "Explosive",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 7,
                    NameAr = "خارق",
                    NameEn = "Armor-Piercing",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 8,
                    NameAr = "دخاني",
                    NameEn = "Smoke",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 9,
                    NameAr = "انارة",
                    NameEn = "Illuminating",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 10,
                    NameAr = "حارق",
                    NameEn = "Incendiary",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 11,
                    NameAr = "خارق حارق",
                    NameEn = "Armor-Piercing Incendiary",
                    IsDeleted = false
                }
            );
        }
    }
}

