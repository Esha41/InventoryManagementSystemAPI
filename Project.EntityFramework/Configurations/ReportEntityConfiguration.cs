using Ettad.Data.Entities;
using Ettad.Data.Enums;
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

            builder.Property(r => r.ReportName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.ReportStatusId)
                .IsRequired()
                .HasDefaultValue(ReportStatuses.Draft);

            builder.Property(r => r.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.Description)
                .HasMaxLength(1000);

            builder.Property(r => r.LayoutData)
                .HasColumnType("varbinary(max)");

            builder.Property(r => r.ReportParameters)
                .HasColumnType("nvarchar(max)");

            // Audit fields configuration
            builder.Property(r => r.CreationDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(r => r.CreatedBy)
                .HasMaxLength(450); // Standard length for user IDs

            builder.Property(r => r.ModificationDate)
                .IsRequired(false);

            builder.Property(r => r.ModifiedBy)
                .HasMaxLength(450)
                .IsRequired(false);

            builder.Property(r => r.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.DeletionDate)
                .IsRequired(false);

            builder.Property(r => r.DeletedBy)
                .HasMaxLength(450)
                .IsRequired(false);

            // Indexes for faster lookups
            builder.HasIndex(r => r.Url)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(r => r.ReportName)
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(r => r.ReportStatusId)
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(r => r.CreatedBy)
                .HasFilter("[IsDeleted] = 0");

            // Soft delete filter
            builder.HasQueryFilter(r => !r.IsDeleted);
        }
    }
}
