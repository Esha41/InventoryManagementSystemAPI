using Ettad.Data.Entities.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class WorkflowStepNotifierConfiguration : IEntityTypeConfiguration<WorkflowStepNotifier>
    {
        public void Configure(EntityTypeBuilder<WorkflowStepNotifier> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("WorkflowStepNotifiers");

            builder.HasOne(x => x.WorkflowStep)
                .WithMany(x => x.Notifiers)
                .HasForeignKey(x => x.WorkflowStepId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            // Index for faster lookups
            builder.HasIndex(x => x.WorkflowStepId);
        }
    }
}
