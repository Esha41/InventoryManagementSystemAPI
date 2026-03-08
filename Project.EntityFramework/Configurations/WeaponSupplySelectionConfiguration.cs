using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class WeaponSupplySelectionConfiguration : IEntityTypeConfiguration<WeaponSupplySelection>
    {
        public void Configure(EntityTypeBuilder<WeaponSupplySelection> builder)
        {
            builder.ToTable("WeaponSupplySelections");
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.OrderId);

            builder.HasOne(x => x.Order)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Depot)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.DepotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Batch)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
