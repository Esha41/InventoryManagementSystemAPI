using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Manufacturers");

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
                new Manufacturer
                {
                    Id = 1,
                    NameAr = "مصنع الذخائر الملكي",
                    NameEn = "Royal Ordnance Factory",
                    IsDeleted = false
                },
                new Manufacturer
                {
                    Id = 2,
                    NameAr = "شركة رايثيون",
                    NameEn = "Raytheon Company",
                    IsDeleted = false
                },
                new Manufacturer
                {
                    Id = 3,
                    NameAr = "مؤسسة الصناعات العسكرية الوطنية",
                    NameEn = "National Military Industries",
                    IsDeleted = false
                },
                new Manufacturer
                {
                    Id = 4,
                    NameAr = "شركة لوكهيد مارتن",
                    NameEn = "Lockheed Martin",
                    IsDeleted = false
                },
                new Manufacturer
                {
                    Id = 5,
                    NameAr = "مجموعة بي إيه إي سيستمز",
                    NameEn = "BAE Systems",
                    IsDeleted = false
                }
            );
        }
    }
}
