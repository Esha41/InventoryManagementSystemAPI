using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ProjectailMaterialConfiguration : IEntityTypeConfiguration<ProjectailMaterial>
    {
        public void Configure(EntityTypeBuilder<ProjectailMaterial> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("ProjectailMaterials");

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
                new ProjectailMaterial
                {
                    Id = 1,
                    NameAr = "فولاذ",
                    NameEn = "Steel",
                    IsDeleted = false
                },
                new ProjectailMaterial
                {
                    Id = 2,
                    NameAr = "نحاس",
                    NameEn = "Brass",
                    IsDeleted = false
                },
                new ProjectailMaterial
                {
                    Id = 3,
                    NameAr = "رصاص",
                    NameEn = "Lead",
                    IsDeleted = false
                },
                new ProjectailMaterial
                {
                    Id = 4,
                    NameAr = "تنغستن",
                    NameEn = "Tungsten",
                    IsDeleted = false
                },
                new ProjectailMaterial
                {
                    Id = 5,
                    NameAr = "يورانيوم منضب",
                    NameEn = "Depleted Uranium",
                    IsDeleted = false
                }
            );
        }
    }
}
