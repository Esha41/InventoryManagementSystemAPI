using DevExpress.XtraReports.UI;
using Ettad.Data.Constants;
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
                ReportConstants.UserReportTemplate => new UsersReportTemplate(),
                ReportConstants.AssetsReportTemplate => new AssetsReportTemplate(),
                ReportConstants.LoginAuditReportTemplate => new LoginAuditReportTemplate(),
                ReportConstants.AuditorPendingApprovalsReportTemplate => new AuditorPendingApprovalsReportTemplate(),
                ReportConstants.AutoRejectedOrdersReportTemplate => new AutoRejectedOrdersReportTemplate(),
                _ => new BaseReportTemplate()
            };
        }
    }
}

