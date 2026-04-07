using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;
using Ettad.User.Services.DTO;

namespace Ettad.Inventory.Service.Assets.Dtos
{
    public class AssetDto
    {
        public long Id { get; set; }

        public long ItemId { get; set; }

        public string? SerialNumber { get; set; }

        public string? RFID { get; set; }

        public long DepotId { get; set; }

        public long BatchId { get; set; }

        public string BatchNumber { get; set; } = string.Empty;

        public long? DepartmentId { get; set; }

        public long? CustodianId { get; set; }

        public string? Location { get; set; }

        public AssetStatus? Status { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? WarrantyExpiryDate { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string? DeliveryReceipt { get; set; }

        public string? Notes { get; set; }

        public bool IsDeleted { get; set; }

        #region Navigation Properties

        public BaseItemDto Item { get; set; }

        public DepotDto Depot { get; set; }

        public DepartmentDto Department { get; set; }

        public EmployeeDto? Custodian { get; set; }

        public List<FileUploadDto> Images { get; set; }

        #endregion
    }
}

