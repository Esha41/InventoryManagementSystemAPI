using Ettad.Inventory.Service.AllowanceItems;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Modules.ReportManagement.API.Services.Dtos;

namespace Ettad.Modules.ReportManagement.API.Reports.DataSources
{
    public class AllowanceItemsDataSource
    {
        public List<AllowanceItemReportDto> Get()
        {
            using var scope = ReportServiceScope.Create();

            var allowanceService = scope.ServiceProvider.GetRequiredService<IAllowanceItemService>();

            var departmentRepository = scope.ServiceProvider.GetRequiredService<ICrossCuttingRepository<Department>>();

            var allowanceResult = allowanceService.GetAllAsync().GetAwaiter().GetResult();

            if (!allowanceResult.Succeeded)
                return new();

            // Get all departments to map DepartmentId to DepartmentName
            var departments = departmentRepository.GetAllAsync().GetAwaiter().GetResult()
                                                 .Where(d => !d.IsDeleted)
                                                 .ToDictionary(d => d.Id, d => d.NameEn ?? d.NameAr ?? string.Empty);

            return allowanceResult.Data.Select(a => new AllowanceItemReportDto
            {
                ItemName = a.ItemName,
                TotalQuantity = a.Quantity,
                DepartmentId = a.DepartmentId,
                DepartmentName = departments.TryGetValue(a.DepartmentId, out var deptName) ? deptName : string.Empty,
                Year = a.Year,
                UsedQuantity = a.ReservedQuantityByOrdersOnProcessing,
                ReservedQuantity = a.UsedQuantityFromAllowance,
                RemainingQuantity = a.RemainingQuantityFromAllowance
            }).ToList();
        }

        public List<AllowanceItemReportDto> GetDesignTimeData()
        {
            return new List<AllowanceItemReportDto>
            {
                new AllowanceItemReportDto
                {
                    DepartmentName = "Qatar Emiri Land Forces",
                    DepartmentId = 1,
                    Year = 2026,
                    ItemName = "TESTTT",
                    TotalQuantity = 100,
                    ReservedQuantity = 80,
                    UsedQuantity = 40,
                    RemainingQuantity = 60
                },
                new AllowanceItemReportDto
                {
                    DepartmentName = "Qatar Emiri Air Force",
                    DepartmentId = 2,
                    Year = 2026,
                    ItemName = ".45 ACP",
                    TotalQuantity = 200,
                    ReservedQuantity = 150,
                    UsedQuantity = 90,
                    RemainingQuantity = 60
                },
                  new AllowanceItemReportDto
                {
                    DepartmentName = "Qatar Emiri Navy",
                    DepartmentId = 3,
                    Year = 2026,
                    ItemName = ".50 BMG",
                    TotalQuantity = 100,
                    ReservedQuantity = 80,
                    UsedQuantity = 40,
                    RemainingQuantity = 60
                },
                new AllowanceItemReportDto
                {
                    DepartmentName = "Emiri Guard Directorate",
                    DepartmentId = 4,
                    Year = 2025,
                    ItemName = "5.56x45mm NATO",
                    TotalQuantity = 200,
                    ReservedQuantity = 150,
                    UsedQuantity = 90,
                    RemainingQuantity = 60
                }
            };
        }
    }
   
    public static class ReportServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; set; } = default!;
    }

    public static class ReportServiceScope
    {
        public static IServiceScope Create()
        {
            return ReportServiceLocator.ServiceProvider.CreateScope();
        }
    }
}