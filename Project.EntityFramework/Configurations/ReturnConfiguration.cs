using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ReturnConfiguration : IEntityTypeConfiguration<Return>
    {
        public void Configure(EntityTypeBuilder<Return> builder)
        {
            builder.ToTable("Returns");
        }
    }
}
