using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
    {
        public void Configure(EntityTypeBuilder<Weapon> builder)
        {
            builder.ToTable("Weapons");
            // Seed data is handled in ApplicationDbInitializer (runtime seeding)
        }
    }
}

