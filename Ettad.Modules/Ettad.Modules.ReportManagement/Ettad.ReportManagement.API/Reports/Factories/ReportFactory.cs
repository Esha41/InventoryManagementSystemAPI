using DevExpress.XtraReports.UI;
using Ettad.Reporting.Reports;
using Ettad.Modules.ReportManagement.API.Reports.Templates;

namespace Ettad.Modules.ReportManagement.API.Reports.Factories
{
    public class ReportFactory
    {
        public XtraReport Create(string reportName)
        {
            return reportName switch 
            {
                "AllowanceItemsReportTemplate" => new AllowanceItemsReportTemplate(),
                _ => new BaseReportTemplate()
            };
        }
    }
}
