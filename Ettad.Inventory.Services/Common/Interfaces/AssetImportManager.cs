using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ettad.CrossCutting.Comman.Models;
using Ettad.ResponseHandler.Models;
using Ettad.ResponseHandler.Consts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;

namespace Ettad.Inventory.Service.Common.Interfaces
{
    public interface IAssetImportManager<TDto, TImportDto>
    {
        Task<APIOperationResponse<ImportResult<TDto>>> ImportAsync(
            IFormFile file, 
            string language, 
            Func<List<TImportDto>, Task> loadLookupsAction,
            Func<TImportDto, string, Task<TDto>> mapDtoAction,
            Func<TDto, Task<List<string>>> validateAction,
            Func<TDto, Task<APIOperationResponse<long>>> createAction,
            Dictionary<string, string> columnMappings);

        Task<APIOperationResponse<ImportResult<TImportDto>>> ImportPreviewAsync(
            IFormFile file, 
            string language, 
            Func<List<TImportDto>, Task> loadLookupsAction,
            Func<TImportDto, string, Task<TDto>> mapDtoAction,
            Func<TDto, Task<List<string>>> validateAction,
            Dictionary<string, string> columnMappings);

        Task<APIOperationResponse<byte[]>> GenerateTemplateAsync(
            string language,
            string sheetName,
            string[] headers,
            Action<ExcelWorksheet> addSampleDataAction,
            Action<ExcelPackage> addLookupsAction,
            Action<ExcelWorksheet> addValidationAction);
    }

    public class AssetImportManager<TDto, TImportDto> : IAssetImportManager<TDto, TImportDto>
        where TDto : new()
        where TImportDto : new()
    {
        private readonly IExcelImportService _excelImportService;
        private readonly ILogger<AssetImportManager<TDto, TImportDto>> _logger;

        public AssetImportManager(
            IExcelImportService excelImportService,
            ILogger<AssetImportManager<TDto, TImportDto>> logger)
        {
            _excelImportService = excelImportService;
            _logger = logger;
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
        }

        public async Task<APIOperationResponse<ImportResult<TDto>>> ImportAsync(
            IFormFile file,
            string language,
            Func<List<TImportDto>, Task> loadLookupsAction,
            Func<TImportDto, string, Task<TDto>> mapDtoAction,
            Func<TDto, Task<List<string>>> validateAction,
            Func<TDto, Task<APIOperationResponse<long>>> createAction,
            Dictionary<string, string> columnMappings)
        {
            try
            {
                // 1. Parse Excel
                var importResult = await _excelImportService.ImportFromExcelAsync<TImportDto>(file, columnMappings);
                
                var finalResult = new ImportResult<TDto>
                {
                    TotalProcessed = importResult.TotalProcessed,
                    ImportHeaders = columnMappings.Values.Distinct().Select(v => System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(v)).ToList()
                };
                finalResult.Errors.AddRange(importResult.Errors);

                if (importResult.SuccessCount == 0)
                {
                     return APIOperationResponse<ImportResult<TDto>>.Success(finalResult, "Processed with no successful records");
                }

                // 2. Load Lookups (Once)
                await loadLookupsAction(importResult.SuccessfulRecords);

                // 3. Process Rows
                int rowNumber = 2;
                foreach (var importDto in importResult.SuccessfulRecords)
                {
                    try
                    {
                        // Map
                        var dto = await mapDtoAction(importDto, language);

                        // Validate
                        var validationErrors = await validateAction(dto);
                        if (validationErrors != null && validationErrors.Any())
                        {
                            AddError(finalResult, rowNumber, $"Validation failed: {string.Join(", ", validationErrors)}", dto);
                            rowNumber++;
                            continue;
                        }

                        // Create
                        var createResult = await createAction(dto);
                        if (!createResult.Succeeded)
                        {
                            AddError(finalResult, rowNumber, createResult.Message, dto);
                        }
                        else
                        {
                            finalResult.SuccessfulRecords.Add(dto);
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.InnerException != null ? $"{ex.Message} (Inner: {ex.InnerException.Message})" : ex.Message;
                        AddError(finalResult, rowNumber, $"Processing error: {msg}");
                    }
                    rowNumber++;
                }

                return APIOperationResponse<ImportResult<TDto>>.Success(finalResult, "Import processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing file {FileName}", file?.FileName);
                return APIOperationResponse<ImportResult<TDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<ImportResult<TImportDto>>> ImportPreviewAsync(
            IFormFile file,
            string language,
            Func<List<TImportDto>, Task> loadLookupsAction,
            Func<TImportDto, string, Task<TDto>> mapDtoAction,
            Func<TDto, Task<List<string>>> validateAction,
            Dictionary<string, string> columnMappings)
        {
            try
            {
                // 1. Parse Excel
                var importResult = await _excelImportService.ImportFromExcelAsync<TImportDto>(file, columnMappings);
                
                var finalResult = new ImportResult<TImportDto>
                {
                    TotalProcessed = importResult.TotalProcessed,
                    ImportHeaders = columnMappings.Values.Distinct().Select(v => System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(v)).ToList()
                };
                finalResult.Errors.AddRange(importResult.Errors);

                if (importResult.SuccessCount == 0)
                {
                     return APIOperationResponse<ImportResult<TImportDto>>.Success(finalResult, "Preview processed with no records");
                }

                // 2. Load Lookups
                await loadLookupsAction(importResult.SuccessfulRecords);

                // 3. Process Rows (Validate only)
                int rowNumber = 2;
                foreach (var importDto in importResult.SuccessfulRecords)
                {
                    try
                    {
                        var dto = await mapDtoAction(importDto, language);

                        var validationErrors = await validateAction(dto);
                        if (validationErrors != null && validationErrors.Any())
                        {
                            finalResult.Errors.Add(new ImportError
                            {
                                RowNumber = rowNumber,
                                ErrorMessage = $"Row {rowNumber}: {string.Join("; ", validationErrors)}",
                                ColumnName = "Validation",
                                RowData = importDto
                            });
                        }
                        else
                        {
                            finalResult.SuccessfulRecords.Add(importDto);
                        }
                    }
                    catch (Exception ex)
                    {
                         // Use finalResult explicitly typed/constrained logic
                        finalResult.Errors.Add(new ImportError
                        {
                            RowNumber = rowNumber,
                            ErrorMessage = $"Row {rowNumber}: {ex.Message}",
                            ColumnName = "N/A",
                            RowData = importDto
                        });
                    }
                    rowNumber++;
                }

                return APIOperationResponse<ImportResult<TImportDto>>.Success(finalResult, "Preview processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error previewing file {FileName}", file?.FileName);
                return APIOperationResponse<ImportResult<TImportDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<byte[]>> GenerateTemplateAsync(
            string language,
            string sheetName,
            string[] headers,
            Action<ExcelWorksheet> addSampleDataAction,
            Action<ExcelPackage> addLookupsAction,
            Action<ExcelWorksheet> addValidationAction)
        {
            try
            {
                using var package = new ExcelPackage();
                var sheet = package.Workbook.Worksheets.Add(sheetName);

                // Headers
                for (int col = 1; col <= headers.Length; col++)
                {
                    sheet.Cells[1, col].Value = headers[col - 1];
                    sheet.Cells[1, col].Style.Font.Bold = true;
                    sheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    sheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    sheet.Cells[1, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Column(col).Width = 20; 
                }

                // Sample Data
                addSampleDataAction?.Invoke(sheet);

                // Lookups
                addLookupsAction?.Invoke(package);

                // Validation
                addValidationAction?.Invoke(sheet);

                sheet.View.FreezePanes(2, 1);
                
                return APIOperationResponse<byte[]>.Success(package.GetAsByteArray(), "Template generated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating template");
                return APIOperationResponse<byte[]>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private void AddError(ImportResult<TDto> result, int rowNumber, string message, object rowData = null)
        {
            result.Errors.Add(new ImportError
            {
                RowNumber = rowNumber,
                ErrorMessage = $"Row {rowNumber}: {message}",
                ColumnName = "N/A",
                RowData = rowData
            });
        }
    }
}
