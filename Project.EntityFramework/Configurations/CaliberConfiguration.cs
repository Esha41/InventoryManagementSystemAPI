using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class CaliberConfiguration : IEntityTypeConfiguration<Caliber>
    {
        public void Configure(EntityTypeBuilder<Caliber> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Calibers");

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ItemType)
                .IsRequired()
                .HasConversion<int>();

            builder.HasIndex(x => new { x.NameAr, x.ItemType })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(x => new { x.NameEn, x.ItemType })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
