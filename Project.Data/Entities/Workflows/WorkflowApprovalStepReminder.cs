using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities.Workflows;

/// <summary>
/// Tracks which reminder tier (lead-days) was already sent for a pending approval step.
/// </summary>
public class WorkflowApprovalStepReminder : AuditEntity<int>
{
    [Required]
    public int WorkflowApprovalStepId { get; set; }

    public virtual WorkflowApprovalStep WorkflowApprovalStep { get; set; }

    [Required]
    public int LeadDays { get; set; }

    [Required]
    public DateTime SentAt { get; set; }
}
