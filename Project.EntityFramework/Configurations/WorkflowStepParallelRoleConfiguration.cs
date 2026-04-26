using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class WorkflowStepParallelRoleConfiguration : IEntityTypeConfiguration<WorkflowStepParallelRole>
    {
        public void Configure(EntityTypeBuilder<WorkflowStepParallelRole> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("WorkflowStepParallelRoles");

            builder.HasOne(x => x.WorkflowStep)
                .WithMany(x => x.ParallelRoles)
                .HasForeignKey(x => x.WorkflowStepId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.WorkflowStepId);
            builder.HasIndex(x => new { x.WorkflowStepId, x.RoleId }).IsUnique();
        }
    }
}
