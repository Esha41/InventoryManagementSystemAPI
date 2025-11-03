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
    internal class RequestDetailConfiguration : IEntityTypeConfiguration<RequestDetail>
    {

        public void Configure(EntityTypeBuilder<RequestDetail> builder)
        {
            builder.ToTable("RequestDetails");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Item)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Request)
                .WithMany(x => x.ResquestDetails)
                .IsRequired(true)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
