using DevExpress.XtraReports.UI;
using Ettad.Inventory.Service.AllowanceItems;
using Ettad.Reporting.Reports;
using Project.Api.Reports.DataSources;
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
                "AllowanceItemsReport" =>
                    Task.Run(() => CreateAllowanceItemsReport())
                        .GetAwaiter()
                        .GetResult(),

                _ => new BaseReportTemplate()
            };
        }
        private async Task<XtraReport> CreateAllowanceItemsReport()
        {
            var report = new AllowanceItemsReport();

            var allowanceService = _provider.GetRequiredService<IAllowanceItemService>();

            var objectDataSource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource
            {
                Name = "AllowanceItems",
                DataSource = typeof(AllowanceItemsRealTimeDataSource),
                DataMember = "GetAsync"
            };

            report.DataSource = objectDataSource;
            report.DataMember = null;

            return report;
        }
    }
}
