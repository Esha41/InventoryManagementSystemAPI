using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ClassificationConfiguration : IEntityTypeConfiguration<Classification>
    {
        public void Configure(EntityTypeBuilder<Classification> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Classifications");

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameAr)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}

