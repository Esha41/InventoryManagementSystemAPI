using System.Threading.Tasks;

namespace Ettad.Modules.ReportManagement.API.Services.Interfaces
{
    public interface IScheduledReportExecutionService
    {
        Task ExecuteScheduledReportsAsync();
        Task ExecuteReportAsync(Guid scheduledReportId);
    }
}
