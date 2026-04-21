using Ettad.Data.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations;

internal class OrderAutoRejectPolicyConfiguration : IEntityTypeConfiguration<OrderAutoRejectPolicy>
{
    public void Configure(EntityTypeBuilder<OrderAutoRejectPolicy> builder)
    {
        builder.ToTable("OrderAutoRejectPolicies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TriggerRoleId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(x => x.ThresholdDays)
            .IsRequired();

        builder.Property(x => x.ScanCron)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.IsEnabled)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.NotifyRequester)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(x => x.TriggerRole)
            .WithMany()
            .HasForeignKey(x => x.TriggerRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.NotifyRoles)
            .WithOne(x => x.Policy)
            .HasForeignKey(x => x.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ReminderLeadDays)
            .WithOne(x => x.Policy)
            .HasForeignKey(x => x.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
