using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using Ettad.Reporting.Reports;
using Project.Api.Reports.Templates;

namespace Project.Api.Reports.Factories
{
    public class ReportFactory
    {
        private readonly IServiceProvider _provider;
        private readonly IServiceScopeFactory _scopeFactory;

        public ReportFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        public XtraReport Create(string reportName)
        {
            return reportName switch
            {
                "AllowanceItemsReport" => new AllowanceItemsReport(),
                _ => new BaseReportTemplate()
            };
        }
    }
}
