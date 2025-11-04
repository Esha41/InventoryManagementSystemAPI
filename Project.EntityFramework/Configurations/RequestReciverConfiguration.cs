using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class RequestReciverConfiguration : IEntityTypeConfiguration<RequestReciver>
    {
        public void Configure(EntityTypeBuilder<RequestReciver> builder)
        {
            builder.ToTable("RequestRecivers");

            builder.Property(x => x.ReciverIdNo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ReciverName)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(x => x.Rank)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.RankId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

