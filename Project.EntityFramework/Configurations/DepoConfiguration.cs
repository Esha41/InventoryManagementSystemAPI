using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class DepoConfiguration : IEntityTypeConfiguration<Depo>
    {
        public void Configure(EntityTypeBuilder<Depo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Depos");

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
    internal class HccConfiguration : IEntityTypeConfiguration<Hcc>
    {
        public void Configure(EntityTypeBuilder<Hcc> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Hccs");

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
