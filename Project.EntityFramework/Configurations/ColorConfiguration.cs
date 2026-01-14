using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Colors");

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
                new Color
                {
                    Id = 1,
                    NameAr = "أخضر",
                    NameEn = "Green",
                    IsDeleted = false
                },
                new Color
                {
                    Id = 2,
                    NameAr = "أسود",
                    NameEn = "Black",
                    IsDeleted = false
                },
                new Color
                {
                    Id = 3,
                    NameAr = "أصفر",
                    NameEn = "Yellow",
                    IsDeleted = false
                },
                new Color
                {
                    Id = 4,
                    NameAr = "أحمر",
                    NameEn = "Red",
                    IsDeleted = false
                },
                new Color
                {
                    Id = 5,
                    NameAr = "رمادي",
                    NameEn = "Gray",
                    IsDeleted = false
                }
            );
        }
    }
}
