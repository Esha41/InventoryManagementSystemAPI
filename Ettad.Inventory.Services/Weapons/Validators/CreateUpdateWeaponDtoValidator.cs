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

            RuleFor(x => x.WeaponType)
                .IsInEnum().WithMessage("Invalid weapon type");

            RuleFor(x => x.ActionType)
                .IsInEnum().WithMessage("Invalid action type");

            RuleFor(x => x.BarrelLength)
                .GreaterThan(0).When(x => x.BarrelLength.HasValue)
                .WithMessage("Barrel length must be greater than 0");

            RuleFor(x => x.BarrelLengthUnitId)
                .GreaterThan(0).When(x => x.BarrelLengthUnitId.HasValue)
                .WithMessage("Barrel length unit must be valid");

            RuleFor(x => x.OverallLength)
                .GreaterThan(0).When(x => x.OverallLength.HasValue)
                .WithMessage("Overall length must be greater than 0");

            RuleFor(x => x.OverallLengthUnitId)
                .GreaterThan(0).When(x => x.OverallLengthUnitId.HasValue)
                .WithMessage("Overall length unit must be valid");

            RuleFor(x => x.Weight)
                .GreaterThan(0).When(x => x.Weight.HasValue)
                .WithMessage("Weight must be greater than 0");

            RuleFor(x => x.WeightUnitId)
                .GreaterThan(0).When(x => x.WeightUnitId.HasValue)
                .WithMessage("Weight unit must be valid");
                
            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0).When(x => x.Capacity.HasValue)
                .WithMessage("Capacity must be greater than or equal to 0");
        }
    }
}
