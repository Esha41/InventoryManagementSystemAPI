using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class WorkflowStepRequesterQuantityNotificationConfiguration
        : IEntityTypeConfiguration<WorkflowStepRequesterQuantityNotification>
    {
        public void Configure(EntityTypeBuilder<WorkflowStepRequesterQuantityNotification> builder)
        {
            builder.ToTable("WorkflowStepRequesterQuantityNotifications");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Workflow)
                .WithMany()
                .HasForeignKey(x => x.WorkflowId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WorkflowStep)
                .WithMany()
                .HasForeignKey(x => x.WorkflowStepId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.WorkflowId);
            builder.HasIndex(x => x.WorkflowStepId).IsUnique();
        }
    }
}
