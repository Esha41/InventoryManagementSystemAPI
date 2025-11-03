using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class PrimaryPurposConfiguration : IEntityTypeConfiguration<PrimaryPurpos>
    {
        public void Configure(EntityTypeBuilder<PrimaryPurpos> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("PrimaryPurposes");

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

            // Seed data
            builder.HasData(
                new PrimaryPurpos
                {
                    Id = 1,
                    NameAr = "قتالي",
                    NameEn = "Combat",
                    IsDeleted = false
                },
                new PrimaryPurpos
                {
                    Id = 2,
                    NameAr = "تدريبي",
                    NameEn = "Training",
                    IsDeleted = false
                },
                new PrimaryPurpos
                {
                    Id = 3,
                    NameAr = "دفاعي",
                    NameEn = "Defense",
                    IsDeleted = false
                },
                new PrimaryPurpos
                {
                    Id = 4,
                    NameAr = "استطلاعي",
                    NameEn = "Reconnaissance",
                    IsDeleted = false
                },
                new PrimaryPurpos
                {
                    Id = 5,
                    NameAr = "هجومي",
                    NameEn = "Offensive",
                    IsDeleted = false
                }
            );
        }
    }
}
