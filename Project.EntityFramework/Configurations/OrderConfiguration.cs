using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.Property(x => x.IsFromAllowance)
                .IsRequired();

            builder.Property(x => x.UsageDateFrom)
                .IsRequired();

            builder.Property(x => x.UsageTimeFrom)
                .IsRequired();

            builder.Property(x => x.UsageDateTo)
                .IsRequired();

            builder.Property(x => x.UsageTimeTo)
                .IsRequired();

            builder.Property(x => x.UsagePurpose)
                .IsRequired();

            builder.Property(x => x.UsageLocation)
                .IsRequired();

            builder.Property(x => x.AnnualDiscard)
                .IsRequired(false);

            builder.Property(x => x.NumberOfOfficer)
                .IsRequired(false);

            builder.Property(x => x.NumberOfOtherRank)
                .IsRequired(false);
        }
    }
}
