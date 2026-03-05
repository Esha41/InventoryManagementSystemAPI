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
                .NotEmpty()
                .WithMessage("Military ID is required")
                .MaximumLength(100)
                .WithMessage("Military ID cannot exceed 100 characters");

            RuleFor(x => x.DepartmentId)
                .NotNull()
                .WithMessage("Department is required")
                .GreaterThan(0)
                .WithMessage("Department ID must be valid");

            RuleFor(x => x.RankId)
                .NotNull()
                .WithMessage("Rank is required")
                .GreaterThan(0)
                .WithMessage("Rank ID must be valid");

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
        }
    }
}

