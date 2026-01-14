using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class HazardDivisionConfiguration : IEntityTypeConfiguration<HazardDivision>
    {
        public void Configure(EntityTypeBuilder<HazardDivision> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("HazardDivisions");

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
                new HazardDivision
                {
                    Id = 1,
                    NameAr = "القسم 1.1 - مواد متفجرة",
                    NameEn = "Division 1.1 - Explosives",
                    IsDeleted = false
                },
                new HazardDivision
                {
                    Id = 2,
                    NameAr = "القسم 1.2 - مواد قابلة للانفجار",
                    NameEn = "Division 1.2 - Projection Hazard",
                    IsDeleted = false
                },
                new HazardDivision
                {
                    Id = 3,
                    NameAr = "القسم 1.3 - مواد قابلة للاشتعال",
                    NameEn = "Division 1.3 - Fire Hazard",
                    IsDeleted = false
                },
                new HazardDivision
                {
                    Id = 4,
                    NameAr = "القسم 1.4 - مواد منخفضة المخاطر",
                    NameEn = "Division 1.4 - Minor Hazard",
                    IsDeleted = false
                },
                new HazardDivision
                {
                    Id = 5,
                    NameAr = "القسم 1.5 - مواد غير حساسة",
                    NameEn = "Division 1.5 - Very Insensitive",
                    IsDeleted = false
                }
            );
        }
    }
}
