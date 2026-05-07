using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AssetBulkDeletionJobConfiguration : IEntityTypeConfiguration<AssetBulkDeletionJob>
    {
        public void Configure(EntityTypeBuilder<AssetBulkDeletionJob> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("AssetBulkDeletionJobs");

            builder.Property(x => x.HangfireJobId).HasMaxLength(200);
            builder.Property(x => x.InitiatedByUserId).HasMaxLength(450).IsRequired();
            builder.Property(x => x.JobStatus).IsRequired();
            builder.Property(x => x.Scope).IsRequired();
            builder.Property(x => x.ExplicitAssetIdsJson).IsRequired(false);

            builder.Property(x => x.Message).HasMaxLength(4000).IsRequired(false);

            builder.HasIndex(x => x.InitiatedByUserId);
            builder.HasIndex(x => x.CreatedUtc);

            builder.HasIndex(x => new { x.JobStatus, x.CreatedUtc });
        }
    }
}
