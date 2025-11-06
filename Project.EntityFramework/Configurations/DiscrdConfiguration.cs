using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class DiscrdConfiguration : IEntityTypeConfiguration<Discard>
    {
        public void Configure(EntityTypeBuilder<Discard> builder)
        {
            builder.ToTable("Discards");
        }
    }

}
