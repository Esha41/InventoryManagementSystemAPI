using System;

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
    }
}
