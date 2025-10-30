using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class AmmunitionConfiguration : IEntityTypeConfiguration<Ammunition>
    {
        public void Configure(EntityTypeBuilder<Ammunition> builder)
        {
            builder.ToTable("Ammunitions");

            builder.HasOne(x => x.NatureOption)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.NatureOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Nsn)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.NsnId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PrimaryPurpos)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PrimaryPurposId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProjectileColor)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ProjectileColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProjectailMaterial)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ProjectailMaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CaseType)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CaseTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Propellant)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PropellantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Compatibility)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.CompatibilityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.HazardDivision)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.HazardDivisionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CaseLengthUnit)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.CaseLengthUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BulletDiameterUnit)
                .WithMany()
                .IsRequired(true)
                .HasForeignKey(x => x.BulletDiameterUnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
