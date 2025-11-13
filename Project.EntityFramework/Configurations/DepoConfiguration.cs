using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class DepoConfiguration : IEntityTypeConfiguration<Depot>
    {
        public void Configure(EntityTypeBuilder<Depot> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Depots");

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

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
                new Depot
                {
                    Id = 1,
                    Code = "DEP-001",
                    NameAr = "مستودع الدوحة المركزي",
                    NameEn = "Doha Central Depot",
                    Location = "Doha",
                    Latitude = 25.2854m,
                    Longitude = 51.5310m,
                    IsDeleted = false
                },
                new Depot
                {
                    Id = 2,
                    Code = "DEP-002",
                    NameAr = "مستودع الريان الغربي",
                    NameEn = "Al Rayyan West Depot",
                    Location = "Al Rayyan",
                    Latitude = 25.2866m,
                    Longitude = 51.4244m,
                    IsDeleted = false
                },
                new Depot
                {
                    Id = 3,
                    Code = "DEP-003",
                    NameAr = "مستودع الخور الشمالي",
                    NameEn = "Al Khor North Depot",
                    Location = "Al Khor",
                    Latitude = 25.6800m,
                    Longitude = 51.5059m,
                    IsDeleted = false
                },
                new Depot
                {
                    Id = 4,
                    Code = "DEP-004",
                    NameAr = "مستودع الوكرة الجنوبي",
                    NameEn = "Al Wakrah South Depot",
                    Location = "Al Wakrah",
                    Latitude = 25.1657m,
                    Longitude = 51.6034m,
                    IsDeleted = false
                }
            );
        }
    }
}
