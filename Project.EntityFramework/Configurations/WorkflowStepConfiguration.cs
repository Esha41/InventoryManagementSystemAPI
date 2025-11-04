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
    internal class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
    {

        public void Configure(EntityTypeBuilder<WorkflowStep> builder)
        {
            builder.ToTable("WorkflowSteps");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StepOrder).IsRequired();
            builder.Property(x => x.MustApprove).HasDefaultValue(true);
            builder.Property(x => x.RequireHigherApproval).HasDefaultValue(false);
            builder.Property(x => x.ReserveQty).HasDefaultValue(false);

            builder.HasOne(x => x.Workflow)
                   .WithMany(x => x.WorkflowSteps)
                   .HasForeignKey(x => x.WorkflowId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ApplicationRole)
                   .WithMany()
                   .HasForeignKey(x => x.ApplicationRoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.HigherApprovalRole)
                   .WithMany()
                   .HasForeignKey(x => x.HigherApprovalRoleId)
                   .IsRequired(false) // <-- make it optional
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
