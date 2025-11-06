using Ettad.Data.Entities;
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
    internal class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
    {
        public void Configure(EntityTypeBuilder<Workflow> builder)
        {
            builder.ToTable("Workflows");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkflowName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.WorkflowType)
                   .IsRequired();

            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.IsSpecialOrReserved).HasDefaultValue(false);
            builder.HasMany(x => x.WorkflowSteps)
                       .WithOne(x => x.Workflow)
                       .HasForeignKey(x => x.WorkflowId)
                       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
