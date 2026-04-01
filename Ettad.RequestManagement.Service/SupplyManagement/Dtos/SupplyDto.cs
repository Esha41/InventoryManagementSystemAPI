using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.Inventory.Service.Common.Dtos;
using Ettad.Module.lookup.Dtos;
using Ettad.CrossCutting.Comman.FileUpload;

namespace Ettad.RequestManagement.Service.SupplyManagement.Dtos
{
    public class SupplyDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public DateTime? SupplyDate { get; set; }
        public string? RecieverName { get; set; }
        public long? ReceiverRankId { get; set; }
        public string? RecieverMilitaryId { get; set; }
        public SupplySubmissionStatus SubmissionStatus { get; set; }
        public SupplyFulfillmentStatus FulfillmentStatus { get; set; }
        public string? Notes { get; set; }

        #region Navigation Properties
        public OrderDto Order { get; set; }
        public RankDto? ReceiverRank { get; set; }
        public List<SupplyDetailDto> SupplyDetails { get; set; } = new();
        public List<FileUploadDto> Files { get; set; } = new();
        #endregion
    }

    public class SupplyDetailDto
    {
        public long Id { get; set; }
        public long SupplyId { get; set; }
        public long ItemId { get; set; }
        public string Lot { get; set; } = string.Empty;
        public long Quantity { get; set; }
        public string? Notes { get; set; }

        #region Calculated Properties
        /// <summary>
        /// The quantity requested in the order for this item
        /// </summary>
        public long RequestedQuantity { get; set; }

        /// <summary>
        /// The total quantity supplied for this item across all supply details in this supply
        /// </summary>
        public long TotalSuppliedQuantity { get; set; }

        /// <summary>
        /// Indicates whether the item has been fully fulfilled (TotalSuppliedQuantity >= RequestedQuantity)
        /// </summary>
        public bool IsFullyFulfilled { get; set; }
        #endregion

        #region Navigation Properties
        public BaseItemDto? Item { get; set; }
        #endregion
    }
}

