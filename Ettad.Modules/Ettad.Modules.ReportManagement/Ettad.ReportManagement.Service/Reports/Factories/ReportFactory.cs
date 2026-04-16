using DevExpress.XtraReports.UI;
using Ettad.Reporting.Reports;
using Ettad.ReportManagement.Service.Reports.Templates;

namespace Ettad.ReportManagement.Service.Reports.Factories
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

