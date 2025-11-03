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
    internal class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Requests");

            builder.Property(x => x.RequestNo)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.RequestNo)
                .IsUnique();

            builder.HasOne(x => x.Depo)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.DepotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RequestReciver)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.RequestReciverId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
