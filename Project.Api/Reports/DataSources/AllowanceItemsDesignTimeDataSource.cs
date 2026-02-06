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
                    DepartmentName = "Qatar Emiri Land Forces",
                    DepartmentId = 1,
                    Year = 2026,
                    ItemName = ".308 Winchester",
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
}
