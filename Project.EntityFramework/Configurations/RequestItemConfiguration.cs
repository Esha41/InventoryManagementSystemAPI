using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class RequestItemConfiguration : IEntityTypeConfiguration<RequestItem>
    {
        public void Configure(EntityTypeBuilder<RequestItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("RequestItems");

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.Notes)
                .IsRequired(false);

            builder.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Request)
                .WithMany(x => x.RequestItems)
                .HasForeignKey(x => x.RequestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
