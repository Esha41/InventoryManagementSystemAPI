using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class HccConfiguration : IEntityTypeConfiguration<Hcc>
    {
        public void Configure(EntityTypeBuilder<Hcc> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Hcc");

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

            // Seed data
            builder.HasData(
                new Hcc
                {
                    Id = 1,
                    NameAr = "HCC-A1",
                    NameEn = "HCC-A1",
                    IsDeleted = false
                },
                new Hcc
                {
                    Id = 2,
                    NameAr = "HCC-B2",
                    NameEn = "HCC-B2",
                    IsDeleted = false
                },
                new Hcc
                {
                    Id = 3,
                    NameAr = "HCC-C3",
                    NameEn = "HCC-C3",
                    IsDeleted = false
                },
                new Hcc
                {
                    Id = 4,
                    NameAr = "HCC-D4",
                    NameEn = "HCC-D4",
                    IsDeleted = false
                },
                new Hcc
                {
                    Id = 5,
                    NameAr = "HCC-E5",
                    NameEn = "HCC-E5",
                    IsDeleted = false
                }
            );
        }
    }
    internal class NsNConfiguration : IEntityTypeConfiguration<Nsn>
    {
        public void Configure(EntityTypeBuilder<Nsn> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Nsn");

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

            // Seed data
            builder.HasData(
                new Nsn
                {
                    Id = 1,
                    NameAr = "NSN-1005-01-123-4567",
                    NameEn = "NSN-1005-01-123-4567",
                    IsDeleted = false
                },
                new Nsn
                {
                    Id = 2,
                    NameAr = "NSN-1010-01-234-5678",
                    NameEn = "NSN-1010-01-234-5678",
                    IsDeleted = false
                },
                new Nsn
                {
                    Id = 3,
                    NameAr = "NSN-1015-01-345-6789",
                    NameEn = "NSN-1015-01-345-6789",
                    IsDeleted = false
                },
                new Nsn
                {
                    Id = 4,
                    NameAr = "NSN-1020-01-456-7890",
                    NameEn = "NSN-1020-01-456-7890",
                    IsDeleted = false
                },
                new Nsn
                {
                    Id = 5,
                    NameAr = "NSN-1025-01-567-8901",
                    NameEn = "NSN-1025-01-567-8901",
                    IsDeleted = false
                }
            );
        }
    }
}
