using FluentValidation;
using Ettad.LdapSettings.Services.Dtos;

namespace Ettad.LdapSettings.Services.Validators
{
    public class LdapOptionsValidator : AbstractValidator<LdapOptions>
    {
        public LdapOptionsValidator()
        {
            // When IsActive is true, server and domain are required
            RuleFor(x => x.LdapServer)
                .NotEmpty()
                .When(x => x.IsActive)
                .WithMessage("LDAP server is required when LDAP is active")
                .MaximumLength(500)
                .WithMessage("LDAP server cannot exceed 500 characters");

            RuleFor(x => x.LdapDomain)
                .NotEmpty()
                .When(x => x.IsActive)
                .WithMessage("LDAP domain is required when LDAP is active")
                .MaximumLength(200)
                .WithMessage("LDAP domain cannot exceed 200 characters");

            // Optional fields - validate only if provided
            RuleFor(x => x.LdapUsername)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.LdapUsername))
                .WithMessage("LDAP username cannot exceed 200 characters");

            RuleFor(x => x.LdapPassword)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.LdapPassword))
                .WithMessage("LDAP password cannot exceed 500 characters");

            RuleFor(x => x.LdapEmpAttr)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.LdapEmpAttr))
                .WithMessage("LDAP employee attribute cannot exceed 100 characters");
        }
    }
}

