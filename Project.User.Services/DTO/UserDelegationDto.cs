using System;

namespace Ettad.User.Services.DTO
{
    public class UserDelegationDto
    {
        public int Id { get; set; }
        public string DelegatorUserId { get; set; }
        public string DelegatorUserName { get; set; }
        public string DelegatorFullName { get; set; }
        public string DelegatorFullNameEn { get; set; }
        public string DelegatorFullNameAr { get; set; }
        public string DelegatorMilitaryId { get; set; }
        
        public string DelegateeUserId { get; set; }
        public string DelegateeUserName { get; set; }
        public string DelegateeFullName { get; set; }
        public string DelegateeFullNameEn { get; set; }
        public string DelegateeFullNameAr { get; set; }
        public string DelegateeMilitaryId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public int DelegationStatus { get; set; } // 0 = Pending, 1 = Approved, 2 = Rejected
        public bool IsIncoming { get; set; }
    }
}
