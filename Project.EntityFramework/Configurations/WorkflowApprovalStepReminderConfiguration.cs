using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations;

internal class WorkflowApprovalStepReminderConfiguration : IEntityTypeConfiguration<WorkflowApprovalStepReminder>
{
    public void Configure(EntityTypeBuilder<WorkflowApprovalStepReminder> builder)
    {
        builder.ToTable("WorkflowApprovalStepReminders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LeadDays)
            .IsRequired();

        builder.Property(x => x.SentAt)
            .IsRequired();

        builder.HasIndex(x => new { x.WorkflowApprovalStepId, x.LeadDays })
            .IsUnique();

        builder.HasOne(x => x.WorkflowApprovalStep)
            .WithMany(x => x.Reminders)
            .HasForeignKey(x => x.WorkflowApprovalStepId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
