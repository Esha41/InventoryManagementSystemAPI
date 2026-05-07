namespace Ettad.Data.Constants;

public static class CriticalStockMonitorConstants
{
    public const string SETTINGS_KEY = "CriticalStockNotificationRecipients";
    public const string SETTINGS_GROUP = "CriticalStockNotifications";
    public const string SCHEDULE_SETTINGS_KEY = "CriticalStockMonitorSchedule";
    public const string SCHEDULE_SETTINGS_GROUP = "BackgroundJobs";
    public const string JOB_ID = "CriticalStockMonitor";
    public const string DEFAULT_ROLE_NAME = "Head of Depo Division (Inventory)";
    public const string DEFAULT_CRON_EXPRESSION = "30 9 * * *"; // Default: 9:30 AM local time (distinct default from low stock)
}
