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
    internal class RecquestReciverConfiguration : IEntityTypeConfiguration<RequestReciver>
    {
        public void Configure(EntityTypeBuilder<RequestReciver> builder)
        {
            builder.ToTable("RequestReciver");

            builder.Property(x => x.ReciverIdNo)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.ReciverIdNo)
                .IsUnique();


            builder.Property(x => x.ReciverName)
               .IsRequired()
               .HasMaxLength(500);

            builder.HasIndex(x => x.ReciverName)
                .IsUnique();



            

           


        }

    
    }
}
