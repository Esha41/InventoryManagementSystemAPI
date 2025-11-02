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
                new Depot
                {
                    Id = 1,
                    NameAr = "مستودع الرياض المركزي",
                    NameEn = "Riyadh Central Depot",
                    Location = "Riyadh",
                    Latitude = 24.7136m,
                    Longitude = 46.6753m,
                    IsDeleted = false
                },
                new Depot
                {
                    Id = 2,
                    NameAr = "مستودع جدة الغربي",
                    NameEn = "Jeddah West Depot",
                    Location = "Jeddah",
                    Latitude = 21.5433m,
                    Longitude = 39.1728m,
                    IsDeleted = false
                },
                new Depot
                {
                    Id = 3,
                    NameAr = "مستودع الدمام الشرقي",
                    NameEn = "Dammam East Depot",
                    Location = "Dammam",
                    Latitude = 26.4207m,
                    Longitude = 50.0888m,
                    IsDeleted = false
                },
                new Depot
                {
                    Id = 4,
                    NameAr = "مستودع الطائف الجنوبي",
                    NameEn = "Taif South Depot",
                    Location = "Taif",
                    Latitude = 21.2703m,
                    Longitude = 40.4150m,
                    IsDeleted = false
                }
            );
        }
    }
}
