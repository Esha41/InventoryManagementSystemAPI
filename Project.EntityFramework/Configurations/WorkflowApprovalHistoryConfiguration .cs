using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.EntityFramework.Configurations
{
    internal class WorkflowApprovalHistoryConfiguration : IEntityTypeConfiguration<WorkflowApprovalHistory>
    {
        public void Configure(EntityTypeBuilder<WorkflowApprovalHistory> builder)
        {
            builder.ToTable("WorkflowApprovalHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OldRequestStatus).IsRequired();
            builder.Property(x => x.NewRequestStatus).IsRequired();
            builder.Property(x => x.ChangedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.WorkflowStep)
                   .WithMany(x => x.ApprovalHistories)
                   .HasForeignKey(x => x.WorkflowStepId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Comments).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.ChangedBy).HasMaxLength(200).IsRequired(false);
        }
    }
}
