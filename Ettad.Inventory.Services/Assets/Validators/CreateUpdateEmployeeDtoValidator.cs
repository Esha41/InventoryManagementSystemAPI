using FluentValidation;
using Ettad.Inventory.Service.Assets.Dtos;

namespace Ettad.Inventory.Service.Assets.Validators
{
    public class CreateUpdateEmployeeDtoValidator : AbstractValidator<CreateUpdateEmployeeDto>
    {
        public CreateUpdateEmployeeDtoValidator()
        {
            RuleFor(x => x.NameAr)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.NameAr))
                .WithMessage("Arabic name cannot exceed 500 characters");

            RuleFor(x => x.NameEn)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.NameEn))
                .WithMessage("English name cannot exceed 500 characters");

            RuleFor(x => x.MilitaryId)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.MilitaryId))
                .WithMessage("Military ID cannot exceed 100 characters");

            RuleFor(x => x.Phone)
                .MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Phone cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must be a valid email address")
                .MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email cannot exceed 255 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(5000).When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes cannot exceed 5000 characters");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).When(x => x.DepartmentId.HasValue)
                .WithMessage("Department ID must be valid if provided");
        }
    }
}

