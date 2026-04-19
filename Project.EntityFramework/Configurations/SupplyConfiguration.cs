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

            builder.Property(x => x.SubmissionStatus)
                .IsRequired();

            builder.Property(x => x.FulfillmentStatus)
                .IsRequired();

            builder.Property(x => x.Notes)
                .IsRequired(false);

            // Indexes
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.ReceiverEmployeeId);

            // Relationships
            builder.HasOne(x => x.Order)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReceiverEmployee)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ReceiverEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
