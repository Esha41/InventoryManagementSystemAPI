using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    public class ReportRoleConfiguration : IEntityTypeConfiguration<ReportRole>
    {
        public void Configure(EntityTypeBuilder<ReportRole> builder)
        {
            builder.ToTable("ReportRoles");

            builder.HasKey(rr => rr.Id);

            // Relationships
            builder.HasOne(rr => rr.Report)
                .WithMany()
                .HasForeignKey(rr => rr.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rr => rr.Role)
                .WithMany()
                .HasForeignKey(rr => rr.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint: one role per report (no duplicates)
            builder.HasIndex(rr => new { rr.ReportId, rr.RoleId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Indexes for faster lookups
            builder.HasIndex(rr => rr.ReportId)
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(rr => rr.RoleId)
                .HasFilter("[IsDeleted] = 0");

            // Soft delete filter
            builder.HasQueryFilter(rr => !rr.IsDeleted);
        }
    }
}
