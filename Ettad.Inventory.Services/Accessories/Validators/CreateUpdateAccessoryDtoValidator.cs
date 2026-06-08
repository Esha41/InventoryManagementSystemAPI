using FluentValidation;
using Ettad.Inventory.Service.Accessories.Dtos;

namespace Ettad.Inventory.Service.Accessories.Validators
{
    public class CreateUpdateAccessoryDtoValidator : AbstractValidator<CreateUpdateAccessoryDto>
    {
        public CreateUpdateAccessoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.ItemNo)
                .NotEmpty().WithMessage("Item number is required")
                .MaximumLength(100).WithMessage("Item number cannot exceed 100 characters");

            RuleFor(x => x.NameAr)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.NameAr))
                .WithMessage("Arabic name cannot exceed 200 characters");

            RuleFor(x => x.CriticalQuantity)
                .GreaterThan(0).When(x => x.CriticalQuantity.HasValue)
                .WithMessage("Critical stock must be greater than 0 when provided");
        }
    }
}
