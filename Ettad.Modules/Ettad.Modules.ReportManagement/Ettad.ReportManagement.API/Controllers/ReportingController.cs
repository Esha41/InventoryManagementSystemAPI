using DevExpress.DataAccess.Sql;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.ReportDesigner.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Modules.ReportManagement.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("DXXRDV")]
    [Route("api/DXXRDV")]
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService)
        {
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("DXXRD")]
    [Route("api/DXXRD")]
    public class CustomReportDesignerController : ReportDesignerController
    {
        public CustomReportDesignerController(
            IReportDesignerMvcControllerService controllerService)
            : base(controllerService)
        {
        }

        [HttpPost("GetDesignerModel")]
        public async Task<IActionResult> GetDesignerModel(
            [FromForm] string reportUrl,
            [FromServices] IReportDesignerModelBuilder designerModelBuilder,
            [FromForm] ReportDesignerSettingsBase designerModelSettings,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var ds = new SqlDataSource("DefaultConnection");

                var designerModel = designerModelBuilder
                    .Report(reportUrl ?? "BaseReportTemplate")
                    .BuildModel();

                designerModel.Assign(designerModelSettings);
                return DesignerModel(designerModel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting report designer model: {ex.Message}");
                return StatusCode(500, new { error = "Failed to build designer model.", detail = ex.Message });
            }
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("DXXQB")]
    [Route("api/DXXQB")]
    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
}