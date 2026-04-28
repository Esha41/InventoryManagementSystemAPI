using FluentValidation;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Weapons.Dtos;

namespace Ettad.Inventory.Service.Weapons.Validators
{
    public class CreateUpdateWeaponDtoValidator : AbstractValidator<CreateUpdateWeaponDto>
    {
        public CreateUpdateWeaponDtoValidator()
        {
            RuleFor(x => x.CaliberCategory)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Caliber category is required")
                .Must(v => Enum.IsDefined(typeof(WeaponCaliberCategory), v!.Value))
                .WithMessage("Caliber category must be Small, Medium, or Large");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.ItemNo)
                .NotEmpty().WithMessage("Item number is required")
                .MaximumLength(100).WithMessage("Item number cannot exceed 100 characters");

            RuleFor(x => x.CaliberId)
                .GreaterThan(0).When(x => x.CaliberId.HasValue)
                .WithMessage("Caliber must be valid when provided");

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

            RuleFor(x => x.CriticalQuantity)
                .GreaterThan(0).When(x => x.CriticalQuantity.HasValue)
                .WithMessage("Critical stock must be greater than 0 when provided");
        }
    }
}
