using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ExplosiveConfiguration : IEntityTypeConfiguration<Explosive>
    {
        public void Configure(EntityTypeBuilder<Explosive> builder)
        {
            builder.ToTable("Explosives");
            // Seed data is handled in ApplicationDbInitializer (runtime seeding)
        }
    }
}

