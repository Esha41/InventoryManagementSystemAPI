using Ettad.Data.Entities;

namespace Ettad.RequestManagement.Service.RequestRecivers.Dtos
{
    public class RequestReciverDto
    {
        public long Id { get; set; }
        public string ReciverIdNo { get; set; }
        public string ReciverName { get; set; }
        public long RankId { get; set; }

        #region Navigation Properties
        public Rank Rank { get; set; }
        #endregion
    }
}

