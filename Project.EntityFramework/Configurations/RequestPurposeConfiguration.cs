using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class RequestPurposeConfiguration : IEntityTypeConfiguration<RequestPurpose>
    {
        public void Configure(EntityTypeBuilder<RequestPurpose> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("RequestPurposes");

            builder.Property(x => x.RequestType)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameAr)
                .IsUnique();

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique();
        }
    }
}
