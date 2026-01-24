using DevExpress.DataAccess.Sql;
using System.Collections.Generic;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.ReportDesigner.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using Microsoft.AspNetCore.Mvc;

namespace ServerApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService)
        {
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomReportDesignerController : ReportDesignerController
    {
        public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService) : base(controllerService)
        {
        }

        [HttpPost("[action]")]
        public IActionResult GetDesignerModel(
            [FromForm] string reportUrl,
            [FromServices] IReportDesignerModelBuilder designerModelBuilder,
            [FromForm] ReportDesignerSettingsBase designerModelSettings)
        {
            try
            {
                var ds = new SqlDataSource("DefaultConnection");

                var designerModel = designerModelBuilder
                                    .Report(reportUrl)
                                    .DataSources(dataSources =>
                                    {
                                        dataSources.Add("EttadDb", ds);
                                    })
                                    .BuildModel();

                designerModel.Assign(designerModelSettings);
                return DesignerModel(designerModel);
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error getting report data: {ex.Message}");
                return null;
            }
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
}