using DevExpress.DataAccess.Sql;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.ReportDesigner.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using Ettad.Reporting.Services;
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
        private readonly IReportService _reportService;

        public CustomReportDesignerController(
            IReportDesignerMvcControllerService controllerService,
            IReportService reportService)
            : base(controllerService)
        {
            _reportService = reportService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetDesignerModel(
            [FromForm] string reportUrl,
            [FromServices] IReportDesignerModelBuilder designerModelBuilder,
            [FromForm] ReportDesignerSettingsBase designerModelSettings,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var ds = new SqlDataSource("DefaultConnection");

                var tables = await _reportService.GetTableNamesAsync(cancellationToken).ConfigureAwait(false);
                foreach (var t in tables)
                {
                    try
                    {
                        var tableRef = string.Equals(t.SchemaName, "dbo", StringComparison.OrdinalIgnoreCase)
                            ? t.TableName
                            : $"{t.SchemaName}.{t.TableName}";
                        var query = SelectQueryFluentBuilder
                            .AddTable(tableRef)
                            .SelectAllColumnsFromTable()
                            .Build(t.TableName);
                        ds.Queries.Add(query);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Skip table {t.SchemaName}.{t.TableName}: {ex.Message}");
                    }
                }

                if (ds.Queries.Count > 0)
                    ds.RebuildResultSchema();

                var designerModel = designerModelBuilder
                    .Report(reportUrl ?? "BaseReportTemplate")
                    .DataSources(dataSources => dataSources.Add("EttadDb", ds))
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
    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
}