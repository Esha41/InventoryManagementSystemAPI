using DevExpress.XtraReports.UI;
using Ettad.Reporting.Reports;
using Project.Api.Reports.Templates;

namespace Project.Api.Reports.Factories
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
