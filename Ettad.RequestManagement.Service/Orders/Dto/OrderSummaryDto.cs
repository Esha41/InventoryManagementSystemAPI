using Ettad.Data.Enums;
using System;
using System.Collections.Generic;

namespace Ettad.RequestManagement.Service.Orders.Dto
{
	public class OrderSummaryDto
	{
		public long Id { get; set; } // Order ID
		public RequestStatus Status { get; set; } // RequestStatus
		public DateTime UsageDateFrom { get; set; } // Request Date (UsageDateFrom)
        public DateTime UsageDateTo { get; set; } // Request Date (UsageDateTo)

		// Department
		public long DepartmentId { get; set; }
		public string DepartmentNameAr { get; set; }
		public string DepartmentNameEn { get; set; }

		// Requester
		public string RequesterId { get; set; }
		public string RequesterName { get; set; }

		// Items
		public ICollection<OrderRequestItemDto> Items { get; set; }
	}
}
