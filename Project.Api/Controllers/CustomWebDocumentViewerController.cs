using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Reporting.Controllers
{
    /// <summary>
    /// Custom Web Document Viewer Controller for DevExpress Reporting
    /// Required for report viewing and export functionality
    /// This controller handles the /DXXRDV endpoint automatically
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] // Exclude from Swagger
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService)
            : base(controllerService)
        {
        }
    }
}
