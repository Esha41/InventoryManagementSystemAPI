using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Assets");

            builder.Property(x => x.SerialNumber)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.HasIndex(x => x.SerialNumber)
                .IsUnique()
                .HasFilter("[SerialNumber] IS NOT NULL AND [IsDeleted] = 0");

            builder.Property(x => x.RFID)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.HasIndex(x => x.RFID)
                .HasFilter("[RFID] IS NOT NULL AND [IsDeleted] = 0");

            builder.Property(x => x.Status)
                .IsRequired(false)
                .HasConversion<int>();

            builder.Property(x => x.PurchaseDate)
                .IsRequired(false);

            builder.Property(x => x.WarrantyExpiryDate)
                .IsRequired(false);

            builder.Property(x => x.PurchasePrice)
                .IsRequired(false)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(5000);

            builder.Property(x => x.IsAssigned)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CurrentAssignmentId)
                .IsRequired(false);

            // Index for quick lookup of assigned/unassigned assets
            builder.HasIndex(x => x.IsAssigned);

            // Index for finding assets by item type
            builder.HasIndex(x => x.ItemId);

            // Foreign key relationships
            builder.HasOne(x => x.Item)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Depot)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.DepotId)
                .OnDelete(DeleteBehavior.Restrict);

            // Self-referencing relationship for current assignment
            builder.HasOne(x => x.CurrentAssignment)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CurrentAssignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Batch)
                .WithMany(x => x.Assets)
                .IsRequired()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasIndex(x => x.BatchId);

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

            builder.HasOne(x => x.PrimaryPurpos)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PrimaryPurposId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
