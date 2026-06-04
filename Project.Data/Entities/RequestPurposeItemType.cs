using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    public class RequestPurposeItemType
    {
        public long RequestPurposeId { get; set; }
        public ItemType ItemType { get; set; }

        #region Navigation Properties
        public RequestPurpose RequestPurpose { get; set; }
        #endregion
    }
}
