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
    internal class WorkflowApprovalHistoryConfiguration : IEntityTypeConfiguration<WorkflowStepApprovalLog>
    {
        public void Configure(EntityTypeBuilder<WorkflowStepApprovalLog> builder)
        {
            builder.ToTable("WorkflowStepApprovalLog");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OldRequestStatus).IsRequired();
            builder.Property(x => x.NewRequestStatus).IsRequired();
            builder.Property(x => x.ChangedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");
          

            builder.Property(x => x.Comments).HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.ChangedBy).HasMaxLength(200).IsRequired(false);
        }
    }
}
