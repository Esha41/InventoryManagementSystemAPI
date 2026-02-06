using Ettad.Inventory.Service.AllowanceItems;
using Project.Api.Reports.DTO;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;

namespace Project.Api.Reports.DataSources
{
    public class AllowanceItemsRealTimeDataSource
    {
        public AllowanceItemsRealTimeDataSource() { }
        public List<AllowanceItemReportDto> Get()
        {
            if (ReportServiceLocator.ServiceProvider == null)
                return AllowanceItemsDesignTimeDataSource.Get();

            using var scope = ReportServiceScope.Create();

            var service = scope.ServiceProvider
                .GetRequiredService<IAllowanceItemService>();

            var departmentRepository = scope.ServiceProvider
                .GetRequiredService<ICrossCuttingRepository<Department>>();

            var response = service.GetAllAsync()
                                  .GetAwaiter()
                                  .GetResult();

            if (!response.Succeeded)
                return new();

            // Get all departments to map DepartmentId to DepartmentName
            var departments = departmentRepository.GetAllAsync()
                                                 .GetAwaiter()
                                                 .GetResult()
                                                 .Where(d => !d.IsDeleted)
                                                 .ToDictionary(d => d.Id, d => d.NameEn ?? d.NameAr ?? string.Empty);

            return response.Data.Select(a => new AllowanceItemReportDto
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