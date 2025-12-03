using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class BaseItemConfiguration : IEntityTypeConfiguration<BaseItem>
    {
        public void Configure(EntityTypeBuilder<BaseItem> builder)
        {
            builder.ToTable("BaseItems");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ItemNo)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasIndex(x => x.ItemNo)
                .IsUnique();

            builder.Property(x => x.ItemType)
                .IsRequired();

            builder.Property(x => x.PartNo)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Price)
                .IsRequired(false)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.MinimumQuantity)
                .IsRequired(false);

            builder.HasOne(x => x.Hcc)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x =>  x.HccId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
