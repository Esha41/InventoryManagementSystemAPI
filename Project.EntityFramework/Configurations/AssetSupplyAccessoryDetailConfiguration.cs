using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AssetSupplyAccessoryDetailConfiguration : IEntityTypeConfiguration<AssetSupplyAccessoryDetail>
    {
        public void Configure(EntityTypeBuilder<AssetSupplyAccessoryDetail> builder)
        {
            builder.ToTable("AssetSupplyAccessoryDetails");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DefaultQuantity).IsRequired();
            builder.Property(x => x.SuppliedQuantity).IsRequired();

            builder.HasOne(x => x.AssetSupplyDetail)
                .WithMany(d => d.AccessoryDetails)
                .HasForeignKey(x => x.AssetSupplyDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Accessory)
                .WithMany()
                .HasForeignKey(x => x.AccessoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.AssetSupplyDetailId);
            builder.HasIndex(x => new { x.AssetSupplyDetailId, x.AccessoryId }).IsUnique();
        }
    }
}
