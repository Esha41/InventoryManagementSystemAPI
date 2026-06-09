using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class WeaponAccessoryConfiguration : IEntityTypeConfiguration<WeaponAccessory>
    {
        public void Configure(EntityTypeBuilder<WeaponAccessory> builder)
        {
            builder.ToTable("WeaponAccessories");

            builder.HasKey(x => new { x.WeaponId, x.AccessoryId });

            builder.Property(x => x.DefaultQuantity)
                .IsRequired();

            builder.HasOne(x => x.Weapon)
                .WithMany(w => w.WeaponAccessories)
                .HasForeignKey(x => x.WeaponId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Accessory)
                .WithMany(a => a.WeaponAccessories)
                .HasForeignKey(x => x.AccessoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.AccessoryId);
        }
    }
}
