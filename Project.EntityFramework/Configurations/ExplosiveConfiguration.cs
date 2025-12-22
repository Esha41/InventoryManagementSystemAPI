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

            builder.Property(x => x.Unit)
                .IsRequired();

            builder.HasOne(x => x.HazardDivision)
                .WithMany()
                .HasForeignKey(x => x.HazardDivisionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
