using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class BaseItemConfiguration : IEntityTypeConfiguration<BaseItem>
    {
        public void Configure(EntityTypeBuilder<BaseItem> builder)
        {
            builder.ToTable("BaseItems");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ItemNo)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasIndex(x => x.ItemNo)
                .IsUnique();

            builder.Property(x => x.Lot)
                .IsRequired();

            builder.Property(x => x.ItemType)
                .IsRequired();

            builder.Property(x => x.BatchNo)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.PartNo)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(x => x.Hcc)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x =>  x.HccId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Supplier)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x =>  x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Country)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x =>  x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Manufacturer)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x =>  x.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
