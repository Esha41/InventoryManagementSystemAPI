using Ettad.Data.Entities.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class ReportEntityConfiguration : IEntityTypeConfiguration<ReportEntity>
    {
        public void Configure(EntityTypeBuilder<ReportEntity> builder)
        {
            builder.ToTable("Reports");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Description)
                .HasMaxLength(1000);

            builder.Property(r => r.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Draft");

            builder.Property(r => r.LayoutData)
                .HasColumnType("varbinary(max)");

            builder.Property(r => r.IsPublic)
                .HasDefaultValue(false);

            builder.Property(r => r.CreatedDate)
                .IsRequired();

            // Index for faster lookups
            builder.HasIndex(r => r.Url)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(r => r.Name);

            // Soft delete filter
            builder.HasQueryFilter(r => !r.IsDeleted);
        }
    }
}
