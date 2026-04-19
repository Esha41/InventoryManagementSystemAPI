using DevExpress.XtraReports.UI;
using Ettad.Reporting.Reports;
using Ettad.ReportManagement.Service.Dtos;
using Ettad.ReportManagement.Service.Reports.Templates;

namespace Ettad.ReportManagement.Service.Reports.Factories
{
    public class ReportFactory
    {
        public XtraReport Create(string reportName)
        {
            return reportName switch 
            {
                ReportConstants.AllowanceItemsReportTemplate => new AllowanceItemsReportTemplate(),
                ReportConstants.UserReportTemplate => new UsersReportTemplate(),
                _ => new BaseReportTemplate()
            };
        }
    }
}

