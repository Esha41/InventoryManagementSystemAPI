using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class ReportStatusConfiguration : IEntityTypeConfiguration<ReportStatus>
    {
        public void Configure(EntityTypeBuilder<ReportStatus> builder)
        {
            builder.ToTable("ReportStatuses");

            builder.HasKey(x => x.Id);

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

            // Configure CreationDate to use database default - prevents EF Core from comparing it in seeded data
            builder.Property(x => x.CreationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

            // Seed data
            builder.HasData(
                new ReportStatus
                {
                    Id = 1,
                    NameAr = "مسودة",
                    NameEn = "Draft"
                },
                new ReportStatus
                {
                    Id = 2,
                    NameAr = "منشور",
                    NameEn = "Published"
                },
                new ReportStatus
                {
                    Id = 3,
                    NameAr = "غير نشط",
                    NameEn = "Inactive"
                }
            );
        }
    }
}
