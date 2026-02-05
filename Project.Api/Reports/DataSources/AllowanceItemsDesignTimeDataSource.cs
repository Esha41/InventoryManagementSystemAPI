using Project.Api.Reports.DTO;

namespace Project.Api.Reports.DataSources
{
    public static class AllowanceItemsDesignTimeDataSource
    {
        // Used ONLY by DevExpress designer
        public static List<AllowanceItemReportDto> Get()
        {
            return new List<AllowanceItemReportDto>
            {
                new AllowanceItemReportDto
                {
                    ItemName = ".308 Winchester",
                    TotalQuantity = 100,
                    ReservedQuantity = 80,
                    UsedQuantity = 40,
                    RemainingQuantity = 60
                },
                new AllowanceItemReportDto
                {
                    ItemName = ".45 ACP",
                    TotalQuantity = 200,
                    ReservedQuantity = 150,
                    UsedQuantity = 90,
                    RemainingQuantity = 60
                },
                  new AllowanceItemReportDto
                {
                    ItemName = ".50 BMG",
                    TotalQuantity = 100,
                    ReservedQuantity = 80,
                    UsedQuantity = 40,
                    RemainingQuantity = 60
                },
                new AllowanceItemReportDto
                {
                    ItemName = "5.56x45mm NATO",
                    TotalQuantity = 200,
                    ReservedQuantity = 150,
                    UsedQuantity = 90,
                    RemainingQuantity = 60
                }
            };
        }
    }
}
