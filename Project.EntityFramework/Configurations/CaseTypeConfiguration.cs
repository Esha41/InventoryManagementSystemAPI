using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class CaseTypeConfiguration : IEntityTypeConfiguration<CaseType>
    {
        public void Configure(EntityTypeBuilder<CaseType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("CaseTypes");

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
                new CaseType
                {
                    Id = 1,
                    NameAr = "نحاسي",
                    NameEn = "Brass",
                    IsDeleted = false
                },
                new CaseType
                {
                    Id = 2,
                    NameAr = "فولاذي",
                    NameEn = "Steel",
                    IsDeleted = false
                },
                new CaseType
                {
                    Id = 3,
                    NameAr = "ألومنيوم",
                    NameEn = "Aluminum",
                    IsDeleted = false
                },
                new CaseType
                {
                    Id = 4,
                    NameAr = "بلاستيك",
                    NameEn = "Plastic",
                    IsDeleted = false
                },
                new CaseType
                {
                    Id = 5,
                    NameAr = "مختلط",
                    NameEn = "Composite",
                    IsDeleted = false
                }
            );
        }
    }
}
