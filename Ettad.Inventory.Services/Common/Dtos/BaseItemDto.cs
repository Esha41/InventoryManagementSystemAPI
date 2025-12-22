using System;
using Ettad.Data.Enums;
using Ettad.Module.lookup.Dtos;

namespace Ettad.Inventory.Service.Common.Dtos
{
    public class BaseItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ItemNo { get; set; }
        public ItemType ItemType { get; set; }
        public string PartNo { get; set; }
        public decimal? Price { get; set; }
        public long? MinimumQuantity { get; set; }
        public bool IsDeleted { get; set; }

        #region Navigation Properties

        // HCC removed - no longer used

        #endregion
    }
}
