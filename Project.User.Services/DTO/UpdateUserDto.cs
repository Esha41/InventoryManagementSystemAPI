namespace Ettad.User.Services.DTO
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
       // public string Email { get; set; }
        public bool IsLdapUser { get; set; }
        public string ExtraEmployeesView { get; set; }
        public int? OrganizationId { get; set; }
        public int? DepartmentId { get; set; }
        public List<string> RoleIds { get; set; }
        public string? FullNameEN { get; set; }
        public string? FullNameAR { get; set; }
        public long? RankId { get; set; }
        public string? MilitoryId { get; set; }
        public string? Email { get; set; }
    }

}
