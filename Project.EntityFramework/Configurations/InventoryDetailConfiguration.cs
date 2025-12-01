using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class InventoryDetailConfiguration : IEntityTypeConfiguration<InventoryDetail>
    {
        public void Configure(EntityTypeBuilder<InventoryDetail> builder)
        {
            builder.ToTable("InventoryDetails");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Lot)
                .IsRequired();

            builder.Property(x => x.ItemQuantity)
                .IsRequired();

            builder.Property(x => x.BatchNo)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.IsLotEmpty)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(x => x.Inventory)
                .WithMany(x => x.InventoryDetails)
                .IsRequired()
                .HasForeignKey(x =>  x.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Supplier)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Manufacturer)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Country)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
