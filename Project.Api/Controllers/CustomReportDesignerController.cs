using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Reporting.Controllers
{
    /// <summary>
    /// Custom Report Designer Controller for DevExpress Reporting
    /// Handles the /DXXRD endpoint automatically
    /// The base ReportDesignerController handles routing automatically
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude from Swagger
    public class CustomReportDesignerController : ReportDesignerController
    {
        public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService)
            : base(controllerService)
        {
        }
    }
}
