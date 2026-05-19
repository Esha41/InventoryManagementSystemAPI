namespace Ettad.ReportManagement.Service.BackgroundJob
{
    /// <summary>
    /// Hangfire recurring job identifiers for polling scheduled report configurations.
    /// </summary>
    public static class ScheduledReportHangfireConstants
    {
        public const string RecurringJobId = "ettad-scheduled-reports-poll";
    }
}
