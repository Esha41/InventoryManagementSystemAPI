using Ettad.Data.Entities;

namespace Ettad.ReportManagement.Service.Interfaces
{
    public interface IScheduledReportDistributionStrategy
    {
        bool CanHandle(ScheduledReport scheduledReport);

        Task ExecuteAsync(ScheduledReport scheduledReport, ScheduledReportExecution execution, CancellationToken cancellationToken);
    }
}
