using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations;

internal class WorkflowAutoRejectTriggerRoleConfiguration : IEntityTypeConfiguration<WorkflowAutoRejectTriggerRole>
{
    public void Configure(EntityTypeBuilder<WorkflowAutoRejectTriggerRole> builder)
    {
        builder.ToTable("WorkflowAutoRejectTriggerRoles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoleId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne(x => x.Trigger)
            .WithMany(x => x.TriggerRoles)
            .HasForeignKey(x => x.WorkflowAutoRejectTriggerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.RoleId);

        builder.HasIndex(x => new { x.WorkflowAutoRejectTriggerId, x.RoleId }).IsUnique();
    }
}
