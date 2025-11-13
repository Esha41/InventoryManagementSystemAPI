using Ettad.Module.lookup.Dtos;
using System.Collections.Generic;

namespace Ettad.User.Services.DTO
{

    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsLdapUser { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string ExtraEmployeesView { get; set; }
        public long? DeparmentId { get; set; }
        public List<UserRoleSummaryDto> Roles { get; set; } = new();
        public string FullNameEN { get; set; }
        public string FullNameAR { get; set; }
        public long? RankId { get; set; }
        public string? MilitoryId { get; set; }
        public DepartmentDto? Department { get; set; }
        public RankDto? Rank { get; set; }
    }

    public class UserRoleSummaryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
