using Ettad.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations
{
    internal class UserDepotConfiguration : IEntityTypeConfiguration<UserDepot>
    {
        public void Configure(EntityTypeBuilder<UserDepot> builder)
        {
            builder.HasKey(ud => new { ud.UserId, ud.DepotId });

            builder.ToTable("UserDepots");

            builder.HasOne(ud => ud.User)
                .WithMany()
                .HasForeignKey(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ud => ud.Depot)
                .WithMany()
                .HasForeignKey(ud => ud.DepotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ud => ud.DepotId);
            builder.HasIndex(ud => ud.UserId);
        }
    }
}
