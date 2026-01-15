using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Ettad.Inventory.Services.Common
{
    public class ImportResult<T>
    {
        public List<T> SuccessfulRecords { get; set; } = new List<T>();
        public List<ImportError> Errors { get; set; } = new List<ImportError>();
        public int TotalProcessed { get; set; }
        public int SuccessCount => SuccessfulRecords.Count;
        public int FailureCount => Errors.Count;
    }

    public class ImportError
    {
        public int RowNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string ColumnName { get; set; }
        public object RowData { get; set; } // Store the actual row data for preview
    }

    public interface IExcelImportService
    {
        Task<ImportResult<T>> ImportFromExcelAsync<T>(
            IFormFile file,
            Dictionary<string, string> columnMappings) where T : new();
    }

    public class ExcelImportService : IExcelImportService
    {
        public ExcelImportService()
        {
            // Set EPPlus license context (EPPlus 8+ API)
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
        }

        public async Task<ImportResult<T>> ImportFromExcelAsync<T>(
            IFormFile file,
            Dictionary<string, string> columnMappings) where T : new()
        {
            var result = new ImportResult<T>();

            if (file == null || file.Length == 0)
            {
                result.Errors.Add(new ImportError { ErrorMessage = "File is empty or null" });
                return result;
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        result.Errors.Add(new ImportError { ErrorMessage = "No worksheets found in the Excel file" });
                        return result;
                    }

                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    int colCount = worksheet.Dimension?.Columns ?? 0;

                    if (rowCount < 2) 
                    {
                        result.Errors.Add(new ImportError { ErrorMessage = "Excel file contains no data (or only headers)" });
                        return result;
                    }

                    // Map headers to column indices
                    var headerMap = new Dictionary<string, int>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        var headerVal = worksheet.Cells[1, col].Text?.Trim();
                        if (!string.IsNullOrEmpty(headerVal) && columnMappings.ContainsKey(headerVal))
                        {
                            headerMap[headerVal] = col;
                        }
                    }

                    // Process Data Rows
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var item = new T();
                            bool rowHasData = false;
                            
                            foreach (var map in columnMappings)
                            {
                                var excelHeader = map.Key;
                                var propertyName = map.Value;

                            if (headerMap.TryGetValue(excelHeader, out int colIndex))
                            {
                                // Use .Value instead of .Text to properly read numeric cells
                                var cellValue = worksheet.Cells[row, colIndex].Value?.ToString() ?? string.Empty;
                                if (!string.IsNullOrWhiteSpace(cellValue))
                                {
                                    rowHasData = true;
                                    SetProperty(item, propertyName, cellValue.Trim());
                                }
                            }
                            }

                            if (rowHasData)
                            {
                                // Set RowNumber if property exists
                                var rowNumProp = typeof(T).GetProperty("RowNumber", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                                if (rowNumProp != null && rowNumProp.CanWrite && rowNumProp.PropertyType == typeof(int))
                                {
                                    rowNumProp.SetValue(item, row);
                                }

                                // Optional: Add Data Annotation Validation here if needed
                                var ctx = new ValidationContext(item);
                                var validationResults = new List<ValidationResult>();
                                if (!Validator.TryValidateObject(item, ctx, validationResults, true))
                                {
                                    foreach (var validationError in validationResults)
                                    {
                                         result.Errors.Add(new ImportError 
                                         { 
                                             RowNumber = row, 
                                             ErrorMessage = validationError.ErrorMessage,
                                             ColumnName = string.Join(", ", validationError.MemberNames),
                                             RowData = item
                                         });
                                    }
                                }
                                else
                                {
                                    result.SuccessfulRecords.Add(item);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var msg = ex.Message;
                            if (ex.InnerException != null)
                            {
                                msg += $" (Inner: {ex.InnerException.Message})";
                            }
                            
                            result.Errors.Add(new ImportError 
                            { 
                                RowNumber = row, 
                                ErrorMessage = $"Error processing row: {msg}",
                                RowData = null // Can't reliably provide RowData here
                            });
                        }
                    }
                    
                    result.TotalProcessed = rowCount - 1; // Excluding header
                }
            }

            return result;
        }

        private void SetProperty<T>(T obj, string propertyName, string value)
        {
            var property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property != null && property.CanWrite)
            {
                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                object convertedValue = null;

                try 
                {
                    if (targetType == typeof(string))
                        convertedValue = value;
                    else if (targetType == typeof(int))
                        convertedValue = int.Parse(value);
                    else if (targetType == typeof(long))
                        convertedValue = long.Parse(value);
                    else if (targetType == typeof(double))
                        convertedValue = double.Parse(value);
                    else if (targetType == typeof(decimal))
                        convertedValue = decimal.Parse(value);
                    else if (targetType == typeof(DateTime))
                        convertedValue = DateTime.Parse(value); // Adjust format as needed
                    else if (targetType == typeof(bool))
                    {
                        // Handle common Excel boolean formats
                        var normalizedValue = value.Trim().ToLowerInvariant();
                        if (normalizedValue == "yes" || normalizedValue == "y" || normalizedValue == "1" || normalizedValue == "true")
                            convertedValue = true;
                        else if (normalizedValue == "no" || normalizedValue == "n" || normalizedValue == "0" || normalizedValue == "false")
                            convertedValue = false;
                        else
                            convertedValue = bool.Parse(value); // Fallback to standard parsing
                    }
                    else if (targetType.IsEnum)
                    {
                        try
                        {
                            convertedValue = Enum.Parse(targetType, value, true);
                        }
                        catch
                        {
                            // Try scrubbing spaces/hyphens
                            var scrubbed = value.Replace(" ", "").Replace("-", "").Replace("_", "");
                            convertedValue = Enum.Parse(targetType, scrubbed, true);
                        }
                    }
                    
                    if (convertedValue != null)
                        property.SetValue(obj, convertedValue);
                }
                catch
                {
                    // If conversion fails, you might want to throw or log specifics.
                    // For now, let it bubble up to be caught as a row error.
                    throw new Exception($"Failed to convert value '{value}' for property '{propertyName}' ({targetType.Name})");
                }
            }
        }
    }
}
