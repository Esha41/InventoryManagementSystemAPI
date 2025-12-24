using FluentValidation;
using Ettad.Inventory.Service.Weapons.Dtos;

namespace Ettad.Inventory.Service.Weapons.Validators
{
    public class CreateUpdateWeaponDtoValidator : AbstractValidator<CreateUpdateWeaponDto>
    {
        public CreateUpdateWeaponDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.ItemNo)
                .NotEmpty().WithMessage("Item number is required")
                .MaximumLength(100).WithMessage("Item number cannot exceed 100 characters");

            RuleFor(x => x.Caliber)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Caliber))
                .WithMessage("Caliber cannot exceed 100 characters");

            RuleFor(x => x.CaliberUnitId)
                .GreaterThan(0).When(x => x.CaliberUnitId.HasValue)
                .WithMessage("Caliber unit must be valid");

            RuleFor(x => x.YearOfManufacture)
                .InclusiveBetween(1800, 2100).When(x => x.YearOfManufacture.HasValue)
                .WithMessage("Year of manufacture must be between 1800 and 2100");

            RuleFor(x => x.CountryOfManufactureId)
                .GreaterThan(0).When(x => x.CountryOfManufactureId.HasValue)
                .WithMessage("Country of manufacture must be valid");

            RuleFor(x => x.Model)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Model))
                .WithMessage("Model cannot exceed 200 characters");
        }
    }
}
