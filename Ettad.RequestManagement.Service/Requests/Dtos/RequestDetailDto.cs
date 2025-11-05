using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ettad.Data.Entities;

namespace Ettad.RequestManagement.Service.Requests.Dtos
{
    public class RequestDetailDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long ItemQuantity { get; set; }
        public long RequestId { get; set; }

        #region Navigation Properties
        public BaseItem Item { get; set; }
        #endregion
    }
}

