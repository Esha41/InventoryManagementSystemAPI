using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Units");

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
                new Unit
                {
                    Id = 1,
                    NameAr = "غرام",
                    NameEn = "Gram",
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 2,
                    NameAr = "مليمتر",
                    NameEn = "Millimeter",
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 3,
                    NameAr = "قطعة",
                    NameEn = "Piece",
                    IsDeleted = false
                }
            );
        }
    }
}
