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
                .IsUnique();

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique();

            // Seed data
            builder.HasData(
                new Unit
                {
                    Id = 1,
                    NameAr = "قطعة",
                    NameEn = "Piece",
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 2,
                    NameAr = "صندوق",
                    NameEn = "Box",
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 3,
                    NameAr = "طن",
                    NameEn = "Ton",
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 4,
                    NameAr = "كيلوغرام",
                    NameEn = "Kilogram",
                    IsDeleted = false
                },
                new Unit
                {
                    Id = 5,
                    NameAr = "حاوية",
                    NameEn = "Container",
                    IsDeleted = false
                }
            );
        }
    }
}
