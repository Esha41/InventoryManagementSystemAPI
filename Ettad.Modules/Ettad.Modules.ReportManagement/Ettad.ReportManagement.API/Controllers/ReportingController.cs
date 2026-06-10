using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.ReportDesigner.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using Ettad.CrossCutting.Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Modules.ReportManagement.API.Controllers
{
    /// <summary>
    /// DevExpress Web Document Viewer (report preview/export). Requires report dashboard or designer permission.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [CheckAuthorize("ReportDashboard", "ReportDesigner")]
    [Route("DXXRDV")]
    [Route("api/DXXRDV")]
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService)
        {
        }
    }

    /// <summary>
    /// DevExpress Web Report Designer endpoints.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Authorize]
    [CheckAuthorize("ReportDesigner")]
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

    /// <summary>
    /// DevExpress Query Builder (used from the report designer).
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [CheckAuthorize("ReportDesigner")]
    [Route("DXXQB")]
    [Route("api/DXXQB")]
    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
}