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
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

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
}
