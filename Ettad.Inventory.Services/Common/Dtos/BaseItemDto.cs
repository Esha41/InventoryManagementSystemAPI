using System;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Enums;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Common.Dtos
{
    public class BaseItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public string? ItemNo { get; set; }
        public ItemType ItemType { get; set; }
        public string? Nsn { get; set; }
        public string? PartNo { get; set; }
        public decimal? Price { get; set; }
        public long? MinimumQuantity { get; set; }
        public long? CriticalQuantity { get; set; }
        public string? Distribution { get; set; }
        public string? ReferenceNo { get; set; }
        public string? UNNumber { get; set; }
        public string? Notes { get; set; }
        public long? ClassificationId { get; set; }
        public long? TypeId { get; set; }
        public bool IsDeleted { get; set; }

        #region Navigation Properties

        public ClassificationDto Classification { get; set; }
        public ItemTypeLookupDto Type { get; set; }
        public List<FileUploadDto> Images { get; set; }
        public List<PrimaryPurposDto> PrimaryPurposes { get; set; }

        #endregion
    }
}
