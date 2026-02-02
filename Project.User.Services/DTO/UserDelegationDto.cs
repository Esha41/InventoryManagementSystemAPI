using System;
using System.Collections.Generic;
using Ettad.Data.Enums;

namespace Ettad.User.Services.DTO
{
    public class UserDelegationDto
    {
        public int Id { get; set; }
        public string DelegatorUserId { get; set; }
        public string DelegatorUserName { get; set; }
        public string DelegatorFullName { get; set; }
        
        public string DelegateeUserId { get; set; }
        public string DelegateeUserName { get; set; }
        public string DelegateeFullName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public int DelegationStatus { get; set; } // 0 = Pending, 1 = Approved, 2 = Rejected
        public bool IsIncoming { get; set; }

        /// <summary>
        /// List of delegated scopes (serialized as string array in JSON)
        /// </summary>
        public List<DelegationScope> DelegationScopes { get; set; } = new List<DelegationScope>();
    }
}
