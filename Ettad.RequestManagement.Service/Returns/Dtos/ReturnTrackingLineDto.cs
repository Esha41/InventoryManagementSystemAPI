using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Module.lookup.Dtos;

namespace Ettad.RequestManagement.Service.Returns.Dtos
{
    public class ReturnTrackingLineDto
    {
        public long Id { get; set; }

        public long ReturnId { get; set; }

        public long RequestId { get; set; }

        public long DepotId { get; set; }

        public long? RequestItemId { get; set; }

        /// <summary>From RequestItem.Item when included (for workflow summary).</summary>
        public string? ItemName { get; set; }

        public string? ItemNo { get; set; }

        public long? ReturnedQuantity { get; set; }

        public long? ReceivedQuantity { get; set; }

        public string? Lot { get; set; }

        public string? BatchNumber { get; set; }

        public string? SerialNumber { get; set; }

        public string? Notes { get; set; }

        public long? AssetId { get; set; }

        public long? InventoryDetailId { get; set; }

        public DepotDto? Depot { get; set; }

        public List<FileUploadDto> Files { get; set; } = new();
    }
}
