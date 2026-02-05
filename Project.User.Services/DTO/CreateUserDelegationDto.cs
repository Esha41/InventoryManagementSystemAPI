using System;
using System.Collections.Generic;
using Ettad.Data.Enums;

namespace Ettad.User.Services.DTO
{
    public class CreateUserDelegationDto
    {
        public string DelegateeUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        /// <summary>
        /// List of scopes to delegate. At least one must be selected.
        /// </summary>
        public List<DelegationScope> DelegationScopes { get; set; }
    }
}
