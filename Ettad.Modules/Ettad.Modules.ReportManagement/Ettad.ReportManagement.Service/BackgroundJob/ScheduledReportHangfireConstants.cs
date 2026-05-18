namespace Ettad.ReportManagement.Service.BackgroundJob
{
    /// <summary>
    /// Hangfire recurring job identifiers for polling scheduled report configurations.
    /// </summary>
    public static class ScheduledReportHangfireConstants
    {
        public const string RecurringJobId = "ettad-scheduled-reports-poll";

        /// <summary>
        /// Cron for how often Hangfire runs the checker (minute-level cron).
        /// Each schedule&#39;s fire time compares <see cref="Ettad.Data.Entities.ScheduledReport.NextRunDate"/> to the app&#39;s
        /// <c>DateTime.Now</c> (server local; see <c>SystemDateTimeProvider</c>).
        /// </summary>
        public const string DefaultCronExpression = "0 8 * * 4";
    }
}
