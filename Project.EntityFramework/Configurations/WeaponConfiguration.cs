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

            builder.Property(x => x.CaliberCategory)
                .IsRequired()
                .HasDefaultValue(WeaponCaliberCategory.Small);

            // Performance indexes for pagination, filtering, and sorting
            // Note: IsDeleted, TypeId, ClassificationId, Name are in BaseItems table
            // CountryOfManufactureId is in Weapons table
            // We can only create composite indexes within the same table
            
            // Single-column index on Weapon-specific property (in Weapons table)
            builder.HasIndex(x => x.CountryOfManufactureId);

            // Seed data is handled in ApplicationDbInitializer (runtime seeding)
        }
    }
}

