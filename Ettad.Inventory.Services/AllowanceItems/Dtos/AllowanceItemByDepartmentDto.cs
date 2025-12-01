using Ettad.Data.Entities;
using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.AllowanceItems.Dtos
{
    public class AllowanceItemDetailDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
        public ItemType ItemType { get; set; }
        
        public int UsedQuantityFromAllowance { get; set; }
        public int ReservedQuantityByOrdersOnProcessing { get; set; }
        public int RemainingQuantityFromAllowance { get; set; }

        // Item details
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public string? BatchNo { get; set; }
    }

    public class AllowanceItemByDepartmentDto
    {
        public long DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentNameAr { get; set; }
        public string DepartmentNameEn { get; set; }
        public int Year { get; set; }
        public ItemType? ItemType { get; set; }
        public List<AllowanceItemDetailDto> Items { get; set; } = new List<AllowanceItemDetailDto>();
    }
}

