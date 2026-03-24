using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class SupplyDetailConfiguration : IEntityTypeConfiguration<SupplyDetail>
    {
        public void Configure(EntityTypeBuilder<SupplyDetail> builder)
        {
            builder.ToTable("SupplyDetails");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Lot)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.Notes)
                .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.Supply)
                .WithMany(x => x.SupplyDetails)
                .IsRequired()
                .HasForeignKey(x => x.SupplyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
