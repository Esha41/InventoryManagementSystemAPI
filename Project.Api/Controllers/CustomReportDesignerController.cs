using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Reporting.Controllers
{
    /// <summary>
    /// Custom Report Designer Controller for DevExpress Reporting
    /// Required for the report designer functionality
    /// This controller handles the /DXXRD endpoint automatically
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude from Swagger
    public class CustomReportDesignerController : ReportDesignerController
    {
        //public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService)
        //    : base(controllerService)
        //{
        //}

    }
}
