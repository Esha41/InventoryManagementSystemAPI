namespace Ettad.LdapSettings.Services.Dtos
{
    /// <summary>
    /// LDAP configuration options stored in settings and used for LDAP authentication.
    /// </summary>
    public class LdapOptions
    {
        public bool IsActive { get; set; }
        public string? LdapServer { get; set; }
        public string? LdapDomain { get; set; }
        public string? LdapUsername { get; set; }
        public string? LdapPassword { get; set; }
        public string? LdapEmpAttr { get; set; }
    }
}


