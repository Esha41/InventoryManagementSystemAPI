using Ettad.Data.Enums;

namespace Ettad.Workflows.Service.Monitoring;

public static class OrderAutoRejectConstants
{
    public static readonly WorkflowType[] OrderWorkflowTypes =
    {
        WorkflowType.NormalOrder,
        WorkflowType.OrderFromAllowance,
        WorkflowType.NormalOrderForTrainingPurpose,
        WorkflowType.NormalOrder_Weapon,
        WorkflowType.OrderFromAllowance_Weapon,
        WorkflowType.NormalOrderForTrainingPurpose_Weapon,
    };

    public const string Group = "BackgroundJobs";

    public const string TriggerRoleIdKey = "OrderAutoRejectTriggerRoleId";
    public const string ThresholdDaysKey = "OrderAutoRejectThresholdDays";
    public const string ScanCronKey = "OrderAutoRejectScanCron";

    public const string JobId = "order-auto-reject-scan";

    public const int DefaultThresholdDays = 30;

    /// <summary>Default schedule when nothing is stored yet (top of every hour).</summary>
    public const string DefaultCronExpression = "0 * * * *";
}
