using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class PrimaryPurposConfiguration : IEntityTypeConfiguration<PrimaryPurpos>
    {
        public void Configure(EntityTypeBuilder<PrimaryPurpos> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("PrimaryPurposes");

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
