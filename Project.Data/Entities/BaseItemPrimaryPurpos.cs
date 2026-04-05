namespace Ettad.Data.Entities
{
    public class BaseItemPrimaryPurpos
    {
        public long BaseItemId { get; set; }

        public long PrimaryPurposId { get; set; }

        #region Navigation Properties

        public BaseItem BaseItem { get; set; }
        public PrimaryPurpos PrimaryPurpos { get; set; }

        #endregion
    }
}
