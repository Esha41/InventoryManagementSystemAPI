using DevExpress.XtraPrinting.Native;
using Ettad.Inventory.Service.AllowanceItems;
using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Project.Api.Reports.DTO;

namespace Project.Api.Reports.DataSources
{
    public class AllowanceItemsRealTimeDataSource
    {
        private readonly IAllowanceItemService _allowanceItemService;

        public AllowanceItemsRealTimeDataSource(IAllowanceItemService service)
        {
            _allowanceItemService = service;
        }

        public List<AllowanceItemReportDto> GetAsync()
        {
            var response = _allowanceItemService.GetAllAsync().GetAwaiter().GetResult();

            if (!response.Succeeded)
                return new List<AllowanceItemReportDto>();

            return response.Data.Select(a => new AllowanceItemReportDto
            {
                ItemName = a.ItemName,
                TotalQuantity = a.Quantity,
                DepartmentId=a.DepartmentId,
                Year=a.Year,
                UsedQuantity = a.ReservedQuantityByOrdersOnProcessing,
                ReservedQuantity = a.UsedQuantityFromAllowance,
                RemainingQuantity = a.RemainingQuantityFromAllowance
            }).ToList();
        }
    }
}