namespace Ettad.Data.Entities
{
    public class Return : BaseRequest
    {
        public long? ReturnToDepotId { get; set; }
        public DateTime? DeliveryDate { get; set; }

        #region Navigation Properties

        public Depot ReturnToDepot { get; set; }

        #endregion
    }
}
