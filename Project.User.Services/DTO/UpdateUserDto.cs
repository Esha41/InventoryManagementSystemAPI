namespace Ettad.User.Services.DTO
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
       // public string Email { get; set; }
        public bool IsLdapUser { get; set; }
        public string ExtraEmployeesView { get; set; }
        public int? EmployeeId { get; set; }
        public int? OrganizationId { get; set; }

    }

}
