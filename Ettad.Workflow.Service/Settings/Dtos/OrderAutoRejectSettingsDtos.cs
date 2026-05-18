namespace Ettad.Workflows.Service.Settings.Dtos;

public class RoleRefDto
{
    public string Id { get; set; } = string.Empty;

    public string? Name { get; set; }
}

public class OrderAutoRejectSettingsDto
{
    public int ThresholdDays { get; set; }

    public string ScanCron { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public bool NotifyRequester { get; set; } = true;

    public List<int> ReminderLeadDays { get; set; } = new();

    public List<RoleRefDto> NotifyRoles { get; set; } = new();
}

public class UpdateOrderAutoRejectSettingsDto
{
    public int ThresholdDays { get; set; }

    public string ScanCron { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public bool NotifyRequester { get; set; } = true;

    public List<int> ReminderLeadDays { get; set; } = new();

    public List<string> NotifyRoleIds { get; set; } = new();
}
