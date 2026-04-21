using Ettad.Data.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ettad.EntityFramework.Configurations;

internal class OrderAutoRejectPolicyReminderDayConfiguration : IEntityTypeConfiguration<OrderAutoRejectPolicyReminderDay>
{
    public void Configure(EntityTypeBuilder<OrderAutoRejectPolicyReminderDay> builder)
    {
        builder.ToTable("OrderAutoRejectPolicyReminderDays");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LeadDays)
            .IsRequired();

        builder.HasIndex(x => new { x.PolicyId, x.LeadDays })
            .IsUnique();

        builder.HasOne(x => x.Policy)
            .WithMany(x => x.ReminderLeadDays)
            .HasForeignKey(x => x.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
