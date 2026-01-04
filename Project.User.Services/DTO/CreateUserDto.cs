namespace Ettad.User.Services.DTO
{
    public class CreateUserDto
    {
        public string? UserName { get; set; }
        //   public string Email { get; set; }
        public string? Password { get; set; }
        public bool IsLdapUser { get; set; }
        public string ExtraEmployeesView { get; set; }
        public long? DepartmentId { get; set; }
        public List<string> RoleIds { get; set; }
        public string? FullNameEN { get; set; }
        public string? FullNameAR { get; set; }
        public long? RankId { get; set; }
        public string? MilitoryId { get; set; }

        public string? Email { get; set; }
        public string LdapUserName { get; set; }
    }

}
