using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.EntityFramework.Configurations
{
    public class WorkflowTypeConfiguration : IEntityTypeConfiguration<WorkFlowType>
    {
        public void Configure(EntityTypeBuilder<WorkFlowType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("WorkFlowType");

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameAr)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.NameEn)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Seed data
            builder.HasData(
                new WorkFlowType
                {
                    Id = 1,
                    NameAr = "Request",
                    NameEn = "Request",
                    IsDeleted = false
                },
                new WorkFlowType
                {
                    Id = 2,
                    NameAr = "Discard",
                    NameEn = "Discard",
                    IsDeleted = false
                },
                new WorkFlowType
                {
                    Id = 3,
                    NameAr = "Return",
                    NameEn = "Return",
                    IsDeleted = false
                }
            );
        }
    }
}
