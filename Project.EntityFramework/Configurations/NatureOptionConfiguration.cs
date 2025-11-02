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
                .IsUnique();

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique();

            // Seed data
            builder.HasData(
                new NatureOption
                {
                    Id = 1,
                    NameAr = "قتالية",
                    NameEn = "Combat",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 2,
                    NameAr = "تدريبية",
                    NameEn = "Training",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 3,
                    NameAr = "تعليمية",
                    NameEn = "Educational",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 4,
                    NameAr = "وهمية",
                    NameEn = "Dummy",
                    IsDeleted = false
                },
                new NatureOption
                {
                    Id = 5,
                    NameAr = "عرض",
                    NameEn = "Display",
                    IsDeleted = false
                }
            );
        }
    }
}

