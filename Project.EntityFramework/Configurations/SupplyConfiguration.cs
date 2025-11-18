using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class SupplyConfiguration : IEntityTypeConfiguration<Supply>
    {
        public void Configure(EntityTypeBuilder<Supply> builder)
        {
            builder.ToTable("Supplies");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SupplyDate)
                .IsRequired(false);

            builder.Property(x => x.RecieverName)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(x => x.RecieverMilitaryId)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.Notes)
                .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.Order)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReceiverRank)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ReceiverRankId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
