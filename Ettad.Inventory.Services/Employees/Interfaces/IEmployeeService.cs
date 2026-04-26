using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Service.Common.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Inventory.Service.Employees.Interfaces
{
    public interface IEmployeeService
    {
        Task<APIOperationResponse<List<EmployeeDto>>> GetAllAsync();
        Task<APIOperationResponse<EmployeeDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateEmployeeDto inputDto);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateEmployeeDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);

        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en");
        Task<APIOperationResponse<byte[]>> ExportAsync(string language = "en");
        Task<APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>> ImportPreviewAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<ImportResult<EmployeeExcelImportRowDto>>> ImportAsync(IFormFile file, string language = "en");
    }
}

