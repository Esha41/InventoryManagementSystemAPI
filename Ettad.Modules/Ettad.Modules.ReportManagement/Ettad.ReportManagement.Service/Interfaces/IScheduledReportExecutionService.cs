using System.Threading.Tasks;

namespace Ettad.ReportManagement.Service.Interfaces
{
    public interface IScheduledReportExecutionService
    {
        Task ExecuteScheduledReportsAsync();
        Task ExecuteReportAsync(Guid scheduledReportId);
    }
}

