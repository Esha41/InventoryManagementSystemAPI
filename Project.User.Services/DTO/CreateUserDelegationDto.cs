using System;

namespace Ettad.User.Services.DTO
{
    public class CreateUserDelegationDto
    {
        public string DelegateeUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}
