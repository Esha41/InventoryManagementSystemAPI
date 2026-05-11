using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations;

internal class WorkflowAutoRejectTriggerConfiguration : IEntityTypeConfiguration<WorkflowAutoRejectTrigger>
{
    public void Configure(EntityTypeBuilder<WorkflowAutoRejectTrigger> builder)
    {
        builder.ToTable("WorkflowAutoRejectTriggers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Mode)
            .IsRequired();

        builder.Property<bool>("ResetOnReApproval")
            .HasColumnName("ResetOnReApproval")
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(x => x.Workflow)
            .WithOne(x => x.AutoRejectTrigger)
            .HasForeignKey<WorkflowAutoRejectTrigger>(x => x.WorkflowId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.WorkflowId).IsUnique();
    }
}
