namespace Ettad.Inventory.Service.Monitoring
{
    public static class LowStockMonitorConstants
    {
        public const string SETTINGS_KEY = "LowStockNotificationRecipients";
        public const string SETTINGS_GROUP = "LowStockNotifications";
        public const string SCHEDULE_SETTINGS_KEY = "LowStockMonitorSchedule";
        public const string SCHEDULE_SETTINGS_GROUP = "BackgroundJobs";
        public const string JOB_ID = "LowStockMonitor";
        public const string DEFAULT_ROLE_NAME = "Head of Depo Division (Inventory)";
        public const string DEFAULT_CRON_EXPRESSION = "15 6 * * *"; // Default: 6:15 AM UTC (9:15 AM Qatar time, UTC+3)
    }
}

