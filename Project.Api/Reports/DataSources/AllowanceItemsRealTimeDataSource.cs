using DevExpress.XtraPrinting.Native;
using Ettad.Inventory.Service.AllowanceItems;
using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Project.Api.Reports.DTO;

namespace Project.Api.Reports.DataSources
{
    public class AllowanceItemsRealTimeDataSource
    {
        public AllowanceItemsRealTimeDataSource() { }
        public List<AllowanceItemReportDto> Get()
        {
            using var scope = ReportServiceScope.Create();

            var service = scope.ServiceProvider
                .GetRequiredService<IAllowanceItemService>();

            var response = service.GetAllAsync()
                                  .GetAwaiter()
                                  .GetResult();

            if (!response.Succeeded)
                return new();

            return response.Data.Select(a => new AllowanceItemReportDto
            {
                ItemName = a.ItemName,
                TotalQuantity = a.Quantity,
                DepartmentId = a.DepartmentId,
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