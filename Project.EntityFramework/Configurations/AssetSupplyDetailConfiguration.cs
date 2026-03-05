using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AssetSupplyDetailConfiguration : IEntityTypeConfiguration<AssetSupplyDetail>
    {
        public void Configure(EntityTypeBuilder<AssetSupplyDetail> builder)
        {
            builder.ToTable("AssetSupplyDetails");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SequenceNo)
                .IsRequired();

            builder.Property(x => x.ConditionOnSupply)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(x => x.IsDelivered)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DeliveredDate)
                .IsRequired(false);

            builder.Property(x => x.CustodianId)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(x => x.AssetSupplyId);
            builder.HasIndex(x => x.AssetId);
            builder.HasIndex(x => x.ItemId);
            builder.HasIndex(x => x.CustodianId);

            // Relationships
            builder.HasOne(x => x.AssetSupply)
                .WithMany(s => s.SupplyDetails)
                .IsRequired()
                .HasForeignKey(x => x.AssetSupplyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Asset)
                .WithMany(a => a.SupplyDetails)
                .IsRequired()
                .HasForeignKey(x => x.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Custodian)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CustodianId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

