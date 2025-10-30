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
        }
    }
}
