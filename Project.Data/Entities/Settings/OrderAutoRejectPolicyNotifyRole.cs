using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ettad.CrossCutting.Comman.Idenitity;

namespace Ettad.Data.Entities.Settings;

public class OrderAutoRejectPolicyNotifyRole
{
    public int PolicyId { get; set; }

    public virtual OrderAutoRejectPolicy Policy { get; set; }

    [Required]
    [ForeignKey(nameof(Role))]
    public string RoleId { get; set; }

    public virtual ApplicationRole Role { get; set; }
}
