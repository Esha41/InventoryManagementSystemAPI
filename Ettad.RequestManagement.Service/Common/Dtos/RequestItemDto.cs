using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.Common.Dtos
{
    public class RequestItemDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public long RequestId { get; set; }
        public string? Notes { get; set; }

        public List<RequestItemWeaponAssociationDto> WeaponAssociations { get; set; } = new();

        #region Navigation Properties (Simplified)
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public string Nsn { get; set; }
        public ItemType ItemType { get; set; }
        #endregion
    }
}
