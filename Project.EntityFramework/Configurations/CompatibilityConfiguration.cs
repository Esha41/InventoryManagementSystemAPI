using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class CompatibilityConfiguration : IEntityTypeConfiguration<Compatibility>
    {
        public void Configure(EntityTypeBuilder<Compatibility> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Compatibilities");

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
                new Compatibility
                {
                    Id = 1,
                    NameAr = "المجموعة أ",
                    NameEn = "Group A",
                    IsDeleted = false
                },
                new Compatibility
                {
                    Id = 2,
                    NameAr = "المجموعة ب",
                    NameEn = "Group B",
                    IsDeleted = false
                },
                new Compatibility
                {
                    Id = 3,
                    NameAr = "المجموعة ج",
                    NameEn = "Group C",
                    IsDeleted = false
                },
                new Compatibility
                {
                    Id = 4,
                    NameAr = "المجموعة د",
                    NameEn = "Group D",
                    IsDeleted = false
                },
                new Compatibility
                {
                    Id = 5,
                    NameAr = "المجموعة هـ",
                    NameEn = "Group E",
                    IsDeleted = false
                }
            );
        }
    }
}
