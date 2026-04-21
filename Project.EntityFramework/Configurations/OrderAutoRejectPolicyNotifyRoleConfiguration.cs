using Ettad.Data.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations;

internal class OrderAutoRejectPolicyNotifyRoleConfiguration : IEntityTypeConfiguration<OrderAutoRejectPolicyNotifyRole>
{
    public void Configure(EntityTypeBuilder<OrderAutoRejectPolicyNotifyRole> builder)
    {
        builder.ToTable("OrderAutoRejectPolicyNotifyRoles");

        builder.HasKey(x => new { x.PolicyId, x.RoleId });

        builder.Property(x => x.RoleId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasOne(x => x.Policy)
            .WithMany(x => x.NotifyRoles)
            .HasForeignKey(x => x.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
