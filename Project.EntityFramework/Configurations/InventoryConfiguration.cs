using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.InvoiceNumber)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.HasOne(x => x.Depo)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.DepoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
