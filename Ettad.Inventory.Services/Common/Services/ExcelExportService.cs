using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Ettad.Inventory.Service.Common.Services
{
    public interface IExcelExportService
    {
        byte[] ExportToExcel<T>(
            IEnumerable<T> data,
            string sheetName,
            Dictionary<string, Func<T, object>> columnMappings
        );
        
        byte[] ExportMultipleSheets(
            Dictionary<string, (IEnumerable<object> data, Dictionary<string, Func<object, object>> columns)> sheets
        );
    }

    public class ExcelExportService : IExcelExportService
    {
        public ExcelExportService()
        {
            // Set EPPlus license context (EPPlus 8+ API)
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
        }

        public byte[] ExportToExcel<T>(
            IEnumerable<T> data,
            string sheetName,
            Dictionary<string, Func<T, object>> columnMappings)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // Add headers
            int col = 1;
            foreach (var header in columnMappings.Keys)
            {
                worksheet.Cells[1, col].Value = header;
                worksheet.Cells[1, col].Style.Font.Bold = true;
                worksheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                worksheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                col++;
            }

            // Add data
            int row = 2;
            foreach (var item in data)
            {
                col = 1;
                foreach (var mapping in columnMappings.Values)
                {
                    var value = mapping(item);
                    worksheet.Cells[row, col].Value = value;
                    col++;
                }
                row++;
            }

            // Auto-fit columns
            if (worksheet.Dimension != null)
            {
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Add borders
                using (var range = worksheet.Cells[1, 1, row - 1, columnMappings.Count])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
            }

            return package.GetAsByteArray();
        }

        public byte[] ExportMultipleSheets(
            Dictionary<string, (IEnumerable<object> data, Dictionary<string, Func<object, object>> columns)> sheets)
        {
            using var package = new ExcelPackage();

            foreach (var sheet in sheets)
            {
                var worksheet = package.Workbook.Worksheets.Add(sheet.Key);
                var (data, columns) = sheet.Value;

                // Add headers
                int col = 1;
                foreach (var header in columns.Keys)
                {
                    worksheet.Cells[1, col].Value = header;
                    worksheet.Cells[1, col].Style.Font.Bold = true;
                    worksheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    col++;
                }

                // Add data
                int row = 2;
                foreach (var item in data)
                {
                    col = 1;
                    foreach (var mapping in columns.Values)
                    {
                        var value = mapping(item);
                        worksheet.Cells[row, col].Value = value;
                        col++;
                    }
                    row++;
                }

                // Auto-fit columns
                if (worksheet.Dimension != null)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }
            }

            return package.GetAsByteArray();
        }
    }
}
