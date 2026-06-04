using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class RequestPurposeItemTypeConfiguration : IEntityTypeConfiguration<RequestPurposeItemType>
    {
        public void Configure(EntityTypeBuilder<RequestPurposeItemType> builder)
        {
            builder.ToTable("RequestPurposeItemTypes");

            builder.HasKey(x => new { x.RequestPurposeId, x.ItemType });

            builder.HasOne(x => x.RequestPurpose)
                .WithMany(x => x.RequestPurposeItemTypes)
                .HasForeignKey(x => x.RequestPurposeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
