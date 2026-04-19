using Ettad.Data.Enums;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Module.lookup.Dtos;
using Ettad.User.Services.DTO;
using Ettad.CrossCutting.Comman.FileUpload;

namespace Ettad.Inventory.Service.AssetSupply.Dtos
{
    /// <summary>
    /// Response DTO for asset supply operations
    /// </summary>
    public class AssetSupplyDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public DateTime? SupplyDate { get; set; }
        public SupplySubmissionStatus SubmissionStatus { get; set; }
        public string SubmissionStatusName => SubmissionStatus.ToString();
        public SupplyFulfillmentStatus FulfillmentStatus { get; set; }
        public string FulfillmentStatusName => FulfillmentStatus.ToString();
        
        public DepartmentDto? Department { get; set; }
        public UserDto? Custodian { get; set; }

        public long? ReceiverEmployeeId { get; set; }
        public EmployeeDto? ReceiverEmployee { get; set; }

        public string? Location { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }

        public List<AssetSupplyDetailDto> SupplyDetails { get; set; } = new();
        public List<FileUploadDto> Files { get; set; } = new();
    }
}
