using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.EntityFramework.Configurations
{
    internal class WorkflowStepTransitionConfiguration : IEntityTypeConfiguration<WorkflowStepTransition>
    {
        public void Configure(EntityTypeBuilder<WorkflowStepTransition> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.SourceWorkflowStep)
                .WithMany(x => x.Transitions)
                .HasForeignKey(x => x.SourceWorkflowStepId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes to avoid cycles

            builder.HasOne(x => x.TargetWorkflowStep)
                .WithMany()
                .HasForeignKey(x => x.TargetWorkflowStepId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

