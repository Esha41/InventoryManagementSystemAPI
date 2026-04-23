using FluentValidation;
using Ettad.Inventory.Service.Explosives.Dtos;

namespace Ettad.Inventory.Service.Explosives.Validators
{
    public class CreateUpdateExplosiveDtoValidator : AbstractValidator<CreateUpdateExplosiveDto>
    {
        public CreateUpdateExplosiveDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.ItemNo)
                .NotEmpty().WithMessage("Item number is required")
                .MaximumLength(100).WithMessage("Item number cannot exceed 100 characters");

            RuleFor(x => x.UnitId)
                .GreaterThan(0).When(x => x.UnitId.HasValue)
                .WithMessage("Unit must be valid");

            RuleFor(x => x.HazardDivisionId)
                .GreaterThan(0).When(x => x.HazardDivisionId.HasValue)
                .WithMessage("Hazard division must be valid");

            RuleFor(x => x.ArmNumber)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.ArmNumber))
                .WithMessage("Arm number cannot exceed 200 characters");

            RuleFor(x => x.CompatibilityId)
                .GreaterThan(0).When(x => x.CompatibilityId.HasValue)
                .WithMessage("Compatibility must be valid when provided");

            RuleFor(x => x.CriticalQuantity)
                .GreaterThan(0).When(x => x.CriticalQuantity.HasValue)
                .WithMessage("Critical stock must be greater than 0 when provided");
        }
    }
}
