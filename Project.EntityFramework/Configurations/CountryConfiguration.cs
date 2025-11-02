using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Countries");

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

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Seed data
            builder.HasData(
                new Country
                {
                    Id = 1,
                    Code = "SA",
                    NameAr = "المملكة العربية السعودية",
                    NameEn = "Saudi Arabia",
                    IsDeleted = false
                },
                new Country
                {
                    Id = 2,
                    Code = "US",
                    NameAr = "الولايات المتحدة الأمريكية",
                    NameEn = "United States",
                    IsDeleted = false
                },
                new Country
                {
                    Id = 3,
                    Code = "UK",
                    NameAr = "المملكة المتحدة",
                    NameEn = "United Kingdom",
                    IsDeleted = false
                },
                new Country
                {
                    Id = 4,
                    Code = "FR",
                    NameAr = "فرنسا",
                    NameEn = "France",
                    IsDeleted = false
                },
                new Country
                {
                    Id = 5,
                    Code = "DE",
                    NameAr = "ألمانيا",
                    NameEn = "Germany",
                    IsDeleted = false
                }
            );
        }
    }
}

