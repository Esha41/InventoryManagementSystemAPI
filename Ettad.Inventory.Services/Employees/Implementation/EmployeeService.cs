using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Ettad.CrossCutting.Comman.Time;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.Common.Interfaces;
using Ettad.Inventory.Service.Employees.Interfaces;

namespace Ettad.Inventory.Service.Employees.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICrossCuttingRepository<Employee> _employeeRepository;
        private readonly ICrossCuttingRepository<Department> _departmentRepository;
        private readonly ICrossCuttingRepository<Rank> _rankRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateEmployeeDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITransactionManager _transactionManager;
        private readonly IExcelImportService _excelImportService;

        public EmployeeService(
            ICrossCuttingRepository<Employee> employeeRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            ICrossCuttingRepository<Rank> rankRepository,
            IMapper mapper,
            IValidator<CreateUpdateEmployeeDto> validator,
            ICurrentUserService currentUserService,
            ILogger<EmployeeService> logger,
            IDateTimeProvider dateTimeProvider,
            ITransactionManager transactionManager,
            IExcelImportService excelImportService)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _rankRepository = rankRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _transactionManager = transactionManager;
            _excelImportService = excelImportService;
        }

        public async Task<APIOperationResponse<EmployeeDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting employee by ID. EmployeeId: {EmployeeId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var employee = await _employeeRepository.FindOneAsync(
                    e => e.Id == id && !e.IsDeleted,
                    false,
                    nameof(Employee.Department),
                    nameof(Employee.Rank)
                );

                if (employee == null)
                    return APIOperationResponse<EmployeeDto>.Fail(ResponseType.NotFound, "Employee not found");

                var dto = _mapper.Map<EmployeeDto>(employee);
                
                _logger.LogInformation("Employee retrieved successfully. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                
                return APIOperationResponse<EmployeeDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee by ID. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<EmployeeDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<EmployeeDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all employees. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var employees = await _employeeRepository.FindAsync(
                    e => !e.IsDeleted,
                    false,
                    nameof(Employee.Department),
                    nameof(Employee.Rank)
                );

                var dtos = _mapper.Map<List<EmployeeDto>>(employees);
                
                _logger.LogInformation("All employees retrieved successfully. Count: {Count}, User: {UserId}", 
                    dtos.Count, _currentUserService.UserId);
                
                return APIOperationResponse<List<EmployeeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all employees. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<EmployeeDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateEmployeeDto inputDto)
        {
            _logger.LogInformation("Creating new employee. NameAr: {NameAr}, NameEn: {NameEn}, User: {UserId}", 
                inputDto?.NameAr, inputDto?.NameEn, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Employee validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var employee = _mapper.Map<Employee>(inputDto);
                employee.CreationDate = _dateTimeProvider.Now;
                employee.CreatedBy = _currentUserService.UserId;

                // Add to repository
                var createdEmployee = await _employeeRepository.AddAsync(employee);
                _logger.LogInformation("Employee created successfully. EmployeeId: {EmployeeId}, User: {UserId}",
                                createdEmployee.Id, _currentUserService.UserId);

                return APIOperationResponse<long>.Success(createdEmployee.Id, "Employee created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee. User: {UserId}", _currentUserService.UserId);
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateEmployeeDto inputDto)
        {
            _logger.LogInformation("Updating employee. EmployeeId: {EmployeeId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if employee exists
                var existingEmployee = await _employeeRepository.FindOneAsync(e => e.Id == id && !e.IsDeleted);
                if (existingEmployee == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Employee not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingEmployee);
                existingEmployee.ModificationDate = _dateTimeProvider.Now;
                existingEmployee.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _employeeRepository.UpdateAsync(existingEmployee);
                
                _logger.LogInformation("Employee updated successfully. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Employee updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting employee. EmployeeId: {EmployeeId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var employee = await _employeeRepository.FindOneAsync(e => e.Id == id && !e.IsDeleted);
                if (employee == null)
                {
                    _logger.LogWarning("Employee not found for deletion. EmployeeId: {EmployeeId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Employee not found");
                }

                await _employeeRepository.DeleteAsync(employee);

                _logger.LogInformation("Employee deleted successfully. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Employee deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        #region Import / Export / Template

        public async Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en")
        {
            try
            {
                ExcelPackage.License.SetNonCommercialPersonal("Ettad");
                var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
                var headers = EmployeeExcelColumnMappings.GetTemplateHeaders(language);

                var departments = (await _departmentRepository.FindAsync(d => !d.IsDeleted))
                    .OrderBy(d => d.Id)
                    .ToList();
                var departmentLabels = departments
                    .Select(d => isAr
                        ? !string.IsNullOrWhiteSpace(d.NameAr) ? d.NameAr.Trim() : (d.NameEn ?? "").Trim()
                        : !string.IsNullOrWhiteSpace(d.NameEn) ? d.NameEn.Trim() : (d.NameAr ?? "").Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .GroupBy(s => s, StringComparer.OrdinalIgnoreCase).Select(g => g.First())
                    .OrderBy(s => s).ToList();

                var ranks = (await _rankRepository.FindAsync(r => !r.IsDeleted))
                    .OrderBy(r => r.Id)
                    .ToList();
                var rankLabels = ranks
                    .Select(r => isAr
                        ? !string.IsNullOrWhiteSpace(r.NameAr) ? r.NameAr.Trim() : (r.NameEn ?? "").Trim()
                        : !string.IsNullOrWhiteSpace(r.NameEn) ? r.NameEn.Trim() : (r.NameAr ?? "").Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .GroupBy(s => s, StringComparer.OrdinalIgnoreCase).Select(g => g.First())
                    .OrderBy(s => s).ToList();

                using var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add(EmployeeExcelColumnMappings.WorksheetName);

                for (int col = 0; col < headers.Count; col++)
                {
                    ws.Cells[1, col + 1].Value = headers[col];
                    ws.Cells[1, col + 1].Style.Font.Bold = true;
                    ws.Cells[1, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    ws.Cells[1, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                static void FillLookupSheet(ExcelWorksheet sheet, IReadOnlyList<string> values)
                {
                    for (int i = 0; i < values.Count; i++)
                        sheet.Cells[i + 1, 1].Value = values[i];
                    if (values.Count == 0)
                        sheet.Cells[1, 1].Value = "";
                }

                var deptSheet = package.Workbook.Worksheets.Add("Departments");
                deptSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(deptSheet, departmentLabels);

                var rankSheet = package.Workbook.Worksheets.Add("Ranks");
                rankSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(rankSheet, rankLabels);

                var headerIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < headers.Count; i++)
                    headerIndex[headers[i]] = i + 1;

                void AddListValidation(int colIdx, ExcelWorksheet lookup, string sheetName)
                {
                    var colLetter = GetColumnLetter(colIdx);
                    var range = $"{colLetter}2:{colLetter}10000";
                    var v = ws.DataValidations.AddListValidation(range);
                    var lr = lookup.Dimension?.End.Row ?? 1;
                    v.Formula.ExcelFormula = $"'{sheetName}'!$A$1:$A${lr}";
                    v.ShowErrorMessage = true;
                    v.ErrorTitle = "Invalid Value";
                    v.Error = "Please select a value from the dropdown list";
                    v.ShowInputMessage = true;
                    v.PromptTitle = "Select";
                    v.Prompt = "Choose from the list";
                }

                var hdrDept = isAr ? "القسم *" : "Department *";
                var hdrRank = isAr ? "الرتبة *" : "Rank *";

                AddListValidation(headerIndex[hdrDept], deptSheet, "Departments");
                AddListValidation(headerIndex[hdrRank], rankSheet, "Ranks");

                for (int c = EmployeeExcelColumnMappings.HiddenColumnCount + 1; c <= headers.Count; c++)
                    ws.Column(c).AutoFit();

                ws.Column(1).Hidden = true;
                ws.Column(1).Width = 0;
                ws.View.FreezePanes(2, 1);

                return APIOperationResponse<byte[]>.Success(package.GetAsByteArray(), "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating employee import template");
                return APIOperationResponse<byte[]>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<byte[]>> ExportAsync(string language = "en")
        {
            try
            {
                ExcelPackage.License.SetNonCommercialPersonal("Ettad");
                var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
                var headers = EmployeeExcelColumnMappings.GetExportHeaders(language);

                var employees = await _employeeRepository
                    .Find(e => !e.IsDeleted, false, nameof(Employee.Department), nameof(Employee.Rank))
                    .OrderBy(e => e.Id)
                    .ToListAsync();

                using var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add(EmployeeExcelColumnMappings.WorksheetName);

                for (int col = 0; col < headers.Count; col++)
                {
                    ws.Cells[1, col + 1].Value = headers[col];
                    ws.Cells[1, col + 1].Style.Font.Bold = true;
                    ws.Cells[1, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    ws.Cells[1, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int excelRow = 2;
                foreach (var emp in employees)
                {
                    for (int col = 0; col < headers.Count; col++)
                        ws.Cells[excelRow, col + 1].Value = ResolveExportCellValue(emp, headers[col], isAr);
                    excelRow++;
                }

                if (ws.Dimension != null)
                {
                    for (int c = EmployeeExcelColumnMappings.HiddenColumnCount + 1; c <= ws.Dimension.End.Column; c++)
                        ws.Column(c).AutoFit();
                }

                ws.Column(1).Hidden = true;
                ws.Column(1).Width = 0;
                ws.View.FreezePanes(2, 1);

                var fileName = $"Employees_{DateTime.UtcNow:yyyyMMdd}.xlsx";
                _logger.LogInformation("Employee export completed. Count: {Count}, User: {UserId}", employees.Count, _currentUserService.UserId);
                return APIOperationResponse<byte[]>.Success(package.GetAsByteArray(), "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting employees");
                return APIOperationResponse<byte[]>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>> ImportPreviewAsync(IFormFile file, string language = "en")
        {
            try
            {
                var mappings = EmployeeExcelColumnMappings.Get(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<EmployeeExcelImportRowDto>(
                    file, mappings, EmployeeExcelColumnMappings.WorksheetName);

                await EnrichAndValidateImportAsync(importResult, language);

                importResult.TotalProcessed = importResult.SuccessfulRecords.Count + importResult.Errors.Count;
                return APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>.Success(importResult, "Preview processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error previewing employee import");
                return APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>> ImportAsync(IFormFile file, string language = "en")
        {
            try
            {
                var mappings = EmployeeExcelColumnMappings.Get(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<EmployeeExcelImportRowDto>(
                    file, mappings, EmployeeExcelColumnMappings.WorksheetName);

                await EnrichAndValidateImportAsync(importResult, language);

                if (importResult.SuccessfulRecords.Count == 0)
                {
                    return APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>.Fail(
                        ResponseType.BadRequest,
                        importResult.Errors.Count > 0 ? "No valid rows to import" : "No data rows found");
                }

                await using var transaction = await _transactionManager.BeginAsync();
                try
                {
                    int created = 0, updated = 0;
                    foreach (var row in importResult.SuccessfulRecords)
                    {
                        var dto = new CreateUpdateEmployeeDto
                        {
                            NameAr = row.NameAr,
                            NameEn = row.NameEn,
                            MilitaryId = row.MilitaryId,
                            DepartmentId = row.DepartmentId,
                            RankId = row.RankId,
                            Phone = row.Phone,
                            Email = row.Email,
                            Notes = row.Notes
                        };

                        if (row.EmployeeId > 0)
                        {
                            var existing = await _employeeRepository.FindOneAsync(e => e.Id == row.EmployeeId && !e.IsDeleted);
                            if (existing == null) continue;
                            _mapper.Map(dto, existing);
                            existing.ModificationDate = _dateTimeProvider.Now;
                            existing.ModifiedBy = _currentUserService.UserId;
                            await _employeeRepository.UpdateAsync(existing);
                            updated++;
                        }
                        else
                        {
                            var employee = _mapper.Map<Employee>(dto);
                            employee.CreationDate = _dateTimeProvider.Now;
                            employee.CreatedBy = _currentUserService.UserId;
                            await _employeeRepository.AddAsync(employee);
                            created++;
                        }
                    }

                    await _transactionManager.CommitAsync();

                    _logger.LogInformation("Employee import completed. Created: {Created}, Updated: {Updated}, User: {UserId}",
                        created, updated, _currentUserService.UserId);

                    importResult.TotalProcessed = created + updated;
                    return APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>.Success(
                        importResult, $"Import completed: {created} created, {updated} updated.");
                }
                catch (Exception exInner)
                {
                    await _transactionManager.RollbackAsync();
                    _logger.LogError(exInner, "Employee import transaction failed");
                    return APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>.Fail(
                        ResponseType.InternalServerError, $"An error occurred: {exInner.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing employees");
                return APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private async Task EnrichAndValidateImportAsync(ImportResult<EmployeeExcelImportRowDto> importResult, string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            var departments = (await _departmentRepository.FindAsync(d => !d.IsDeleted)).ToList();
            var ranks = (await _rankRepository.FindAsync(r => !r.IsDeleted)).ToList();

            // Pass 1: resolve lookups + per-row validation
            var invalidRows = new List<(EmployeeExcelImportRowDto row, string error)>();

            foreach (var row in importResult.SuccessfulRecords.ToList())
            {
                var rowErrors = new List<string>();

                // Resolve department
                if (!string.IsNullOrWhiteSpace(row.DepartmentName))
                {
                    var t = row.DepartmentName.Trim();
                    var dept = departments.FirstOrDefault(d =>
                        !string.IsNullOrWhiteSpace(d.NameEn) && string.Equals(d.NameEn.Trim(), t, StringComparison.OrdinalIgnoreCase) ||
                        !string.IsNullOrWhiteSpace(d.NameAr) && string.Equals(d.NameAr.Trim(), t, StringComparison.OrdinalIgnoreCase));
                    if (dept != null)
                        row.DepartmentId = dept.Id;
                    else
                        rowErrors.Add($"Department not found: {t}");
                }

                // Resolve rank
                if (!string.IsNullOrWhiteSpace(row.RankName))
                {
                    var t = row.RankName.Trim();
                    var rank = ranks.FirstOrDefault(r =>
                        !string.IsNullOrWhiteSpace(r.NameEn) && string.Equals(r.NameEn.Trim(), t, StringComparison.OrdinalIgnoreCase) ||
                        !string.IsNullOrWhiteSpace(r.NameAr) && string.Equals(r.NameAr.Trim(), t, StringComparison.OrdinalIgnoreCase));
                    if (rank != null)
                        row.RankId = rank.Id;
                    else
                        rowErrors.Add($"Rank not found: {t}");
                }

                // Validate via existing FluentValidation rules
                if (rowErrors.Count == 0)
                {
                    var dto = new CreateUpdateEmployeeDto
                    {
                        NameAr = row.NameAr,
                        NameEn = row.NameEn,
                        MilitaryId = row.MilitaryId,
                        DepartmentId = row.DepartmentId,
                        RankId = row.RankId,
                        Phone = row.Phone,
                        Email = row.Email,
                        Notes = row.Notes
                    };
                    var vr = await _validator.ValidateAsync(dto);
                    if (!vr.IsValid)
                        rowErrors.Add(string.Join("; ", vr.Errors.Select(e => e.ErrorMessage)));
                }

                if (rowErrors.Count > 0)
                    invalidRows.Add((row, string.Join("; ", rowErrors)));
            }

            FlushInvalidRows(importResult, invalidRows);

            // Pass 2: in-file MilitaryId duplicates
            var dupMilIds = importResult.SuccessfulRecords
                .Where(r => !string.IsNullOrWhiteSpace(r.MilitaryId))
                .GroupBy(r => r.MilitaryId!.Trim(), StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            invalidRows.Clear();
            foreach (var row in importResult.SuccessfulRecords)
            {
                if (!string.IsNullOrWhiteSpace(row.MilitaryId) && dupMilIds.Contains(row.MilitaryId.Trim()))
                    invalidRows.Add((row, $"Duplicate Military ID in file: {row.MilitaryId}"));
            }
            FlushInvalidRows(importResult, invalidRows);

            // Pass 3: DB MilitaryId uniqueness (exclude round-trip IDs)
            var milIdsToCheck = importResult.SuccessfulRecords
                .Where(r => !string.IsNullOrWhiteSpace(r.MilitaryId))
                .ToList();

            if (milIdsToCheck.Count > 0)
            {
                var milValues = milIdsToCheck.Select(r => r.MilitaryId!.Trim()).ToList();
                var roundTripIds = milIdsToCheck.Where(r => r.EmployeeId > 0).Select(r => r.EmployeeId).Distinct().ToList();

                var existingDups = (await _employeeRepository
                    .Find(e => !e.IsDeleted && e.MilitaryId != null && milValues.Contains(e.MilitaryId) && !roundTripIds.Contains(e.Id))
                    .Select(e => e.MilitaryId)
                    .ToListAsync())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (existingDups.Count > 0)
                {
                    var invalid = importResult.SuccessfulRecords
                        .Where(r => !string.IsNullOrWhiteSpace(r.MilitaryId) && existingDups.Contains(r.MilitaryId!.Trim()))
                        .Select(r => (r, $"Military ID already exists: {r.MilitaryId}"))
                        .ToList();
                    FlushInvalidRows(importResult, invalid);
                }
            }
        }

        private static object? ResolveExportCellValue(Employee emp, string header, bool isAr)
        {
            return header switch
            {
                "_EmployeeId" => emp.Id,
                "Name (Arabic)" or "الاسم بالعربي" => emp.NameAr ?? "",
                "Name (English)" or "الاسم بالإنجليزي" => emp.NameEn ?? "",
                "Military ID" or "الرقم العسكري" => emp.MilitaryId ?? "",
                "Department" or "القسم" => emp.Department == null ? "" :
                    isAr
                        ? (!string.IsNullOrWhiteSpace(emp.Department.NameAr) ? emp.Department.NameAr : emp.Department.NameEn) ?? ""
                        : (!string.IsNullOrWhiteSpace(emp.Department.NameEn) ? emp.Department.NameEn : emp.Department.NameAr) ?? "",
                "Rank" or "الرتبة" => emp.Rank == null ? "" :
                    isAr
                        ? (!string.IsNullOrWhiteSpace(emp.Rank.NameAr) ? emp.Rank.NameAr : emp.Rank.NameEn) ?? ""
                        : (!string.IsNullOrWhiteSpace(emp.Rank.NameEn) ? emp.Rank.NameEn : emp.Rank.NameAr) ?? "",
                "Phone" or "الهاتف" => emp.Phone ?? "",
                "Email" or "البريد الإلكتروني" => emp.Email ?? "",
                "Notes" or "ملاحظات" => emp.Notes ?? "",
                _ => ""
            };
        }

        private static string GetColumnLetter(int columnNumber)
        {
            string columnLetter = "";
            while (columnNumber > 0)
            {
                columnNumber--;
                columnLetter = (char)('A' + columnNumber % 26) + columnLetter;
                columnNumber /= 26;
            }
            return columnLetter;
        }

        private static void FlushInvalidRows(
            ImportResult<EmployeeExcelImportRowDto> importResult,
            List<(EmployeeExcelImportRowDto row, string error)> invalidRows)
        {
            if (invalidRows.Count == 0) return;
            var toRemove = new HashSet<EmployeeExcelImportRowDto>(invalidRows.Select(x => x.row));
            importResult.SuccessfulRecords.RemoveAll(r => toRemove.Contains(r));
            foreach (var (row, error) in invalidRows)
            {
                importResult.Errors.Add(new ImportError
                {
                    RowNumber = row.RowNumber,
                    ErrorMessage = error,
                    RowData = row
                });
            }
        }

        #endregion
    }
}

