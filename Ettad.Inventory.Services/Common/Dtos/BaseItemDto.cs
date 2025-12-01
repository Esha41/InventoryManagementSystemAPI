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
        public long HccId { get; set; }
        public string PartNo { get; set; }
        public bool IsDeleted { get; set; }

        #region Navigation Properties

        public HccDto Hcc { get; set; }

        #endregion
    }
}
