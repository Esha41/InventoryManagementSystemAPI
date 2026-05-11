using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations;

internal class WorkflowAutoRejectTriggerStepConfiguration : IEntityTypeConfiguration<WorkflowAutoRejectTriggerStep>
{
    public void Configure(EntityTypeBuilder<WorkflowAutoRejectTriggerStep> builder)
    {
        builder.ToTable("WorkflowAutoRejectTriggerSteps");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkflowStepId)
            .IsRequired();

        builder.HasOne(x => x.Trigger)
            .WithMany(x => x.TriggerSteps)
            .HasForeignKey(x => x.WorkflowAutoRejectTriggerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.WorkflowStep)
            .WithMany()
            .HasForeignKey(x => x.WorkflowStepId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.WorkflowStepId);

        builder.HasIndex(x => new { x.WorkflowAutoRejectTriggerId, x.WorkflowStepId }).IsUnique();
    }
}
