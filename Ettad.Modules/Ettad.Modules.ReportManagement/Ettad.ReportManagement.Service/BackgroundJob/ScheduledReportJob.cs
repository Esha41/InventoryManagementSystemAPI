using Ettad.ReportManagement.Service.Interfaces;
using Hangfire;

namespace Ettad.ReportManagement.Service.BackgroundJob
{
    /// <summary>
    /// Hangfire job class for scheduled report execution.
    /// This class uses Hangfire's built-in dependency injection.
    /// </summary>
    public class ScheduledReportJob
    {
        private readonly IScheduledReportExecutionService _executionService;

        public ScheduledReportJob(IScheduledReportExecutionService executionService)
        {
            _executionService = executionService;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ExecuteAsync()
        {
            await _executionService.ExecuteScheduledReportsAsync();
        }
    }
}

