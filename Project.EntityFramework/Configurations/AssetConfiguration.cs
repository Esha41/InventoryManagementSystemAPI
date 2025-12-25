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

            builder.Property(x => x.Location)
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.Property(x => x.Status)
                .IsRequired(false)
                .HasConversion<int>();

            builder.Property(x => x.AssetTag)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.PurchaseDate)
                .IsRequired(false);

            builder.Property(x => x.WarrantyExpiryDate)
                .IsRequired(false);

            builder.Property(x => x.Condition)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.PurchasePrice)
                .IsRequired(false)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(5000);

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

            builder.HasOne(x => x.Department)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Custodian)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CustodianId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
