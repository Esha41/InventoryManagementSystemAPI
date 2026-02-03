using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class ExplosiveConfiguration : IEntityTypeConfiguration<Explosive>
    {
        public void Configure(EntityTypeBuilder<Explosive> builder)
        {
            builder.ToTable("Explosives");

            builder.HasOne(x => x.HazardDivision)
                .WithMany()
                .HasForeignKey(x => x.HazardDivisionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // Performance indexes for pagination, filtering, and sorting
            // Note: IsDeleted, TypeId, ClassificationId, Name are in BaseItems table
            // HazardDivisionId is in Explosives table
            // We can only create composite indexes within the same table
            
            // Single-column index on Explosive-specific property (in Explosives table)
            builder.HasIndex(x => x.HazardDivisionId);
        }
    }
}
