using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class BaseItemPrimaryPurposConfiguration : IEntityTypeConfiguration<BaseItemPrimaryPurpos>
    {
        public void Configure(EntityTypeBuilder<BaseItemPrimaryPurpos> builder)
        {
            builder.ToTable("BaseItemPrimaryPurposes");

            builder.HasKey(x => new { x.BaseItemId, x.PrimaryPurposId });

            builder.HasOne(x => x.BaseItem)
                .WithMany(x => x.BaseItemPrimaryPurposes)
                .HasForeignKey(x => x.BaseItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PrimaryPurpos)
                .WithMany()
                .HasForeignKey(x => x.PrimaryPurposId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
