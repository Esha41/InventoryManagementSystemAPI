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
    internal class WorkflowApprovalStepConfiguration : IEntityTypeConfiguration<WorkflowApprovalStep>
    {
        public void Configure(EntityTypeBuilder<WorkflowApprovalStep> builder)
        {
            builder.ToTable("WorkflowApprovalSteps");

            // Primary Key inherited from AuditEntity<int>
            builder.HasKey(x => x.Id);

            // Required properties
            builder.Property(x => x.WorkflowStepId)
                   .IsRequired();

            builder.Property(x => x.TargetRequestId)
                   .IsRequired();

            builder.Property(x => x.RequestType)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired();

            // Optional properties
            builder.Property(x => x.ApproverUserId)
                   .IsRequired(false);

            builder.Property(x => x.Comments)
                   .HasMaxLength(1000) // optional length
                   .IsRequired(false);

            builder.Property(x => x.IsCurrent)
                   .HasDefaultValue(false);

            builder.Property(x => x.IsDelegation)
                   .HasDefaultValue(0); // 0 = no delegation, 1 = delegated, etc.

            builder.Property(x => x.ApprovedDate)
                   .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.WorkflowStep)
                   .WithMany(x => x.ApprovalSteps)
                   .HasForeignKey(x => x.WorkflowStepId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Index for faster lookup per request
            builder.HasIndex(x => new { x.TargetRequestId, x.IsCurrent });
        }
    }
}
