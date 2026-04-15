using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ReturnTrackingLineConfiguration : IEntityTypeConfiguration<ReturnTrackingLine>
    {
        public void Configure(EntityTypeBuilder<ReturnTrackingLine> builder)
        {
            builder.ToTable("ReturnTrackingLines");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Lot)
                .IsRequired(false)
                .HasMaxLength(64);

            builder.Property(x => x.BatchNumber)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.SerialNumber)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(5000);

            builder.HasIndex(x => x.ReturnId);
            builder.HasIndex(x => x.RequestId);
            builder.HasIndex(x => x.DepotId);
            builder.HasIndex(x => x.AssetId);
            builder.HasIndex(x => x.InventoryDetailId);

            builder.HasOne(x => x.Return)
                .WithMany()
                .HasForeignKey(x => x.ReturnId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Request)
                .WithMany()
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Depot)
                .WithMany()
                .HasForeignKey(x => x.DepotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RequestItem)
                .WithMany()
                .HasForeignKey(x => x.RequestItemId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Asset)
                .WithMany()
                .HasForeignKey(x => x.AssetId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.InventoryDetail)
                .WithMany()
                .HasForeignKey(x => x.InventoryDetailId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
