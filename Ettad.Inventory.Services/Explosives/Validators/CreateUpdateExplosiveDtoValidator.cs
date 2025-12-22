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

            RuleFor(x => x.ExplosiveType)
                .IsInEnum().WithMessage("Invalid explosive type");

            RuleFor(x => x.NetExplosiveQuantity)
                .GreaterThan(0).When(x => x.NetExplosiveQuantity.HasValue)
                .WithMessage("Net explosive quantity must be greater than 0");

            RuleFor(x => x.NetExplosiveQuantityUnitId)
                .GreaterThan(0).When(x => x.NetExplosiveQuantityUnitId.HasValue)
                .WithMessage("Unit must be valid");

            RuleFor(x => x.TotalWeight)
                .GreaterThan(0).When(x => x.TotalWeight.HasValue)
                .WithMessage("Total weight must be greater than 0");

            RuleFor(x => x.TotalWeightUnitId)
                .GreaterThan(0).When(x => x.TotalWeightUnitId.HasValue)
                .WithMessage("Weight unit must be valid");

            RuleFor(x => x.HazardDivisionId)
                .GreaterThan(0).When(x => x.HazardDivisionId.HasValue)
                .WithMessage("Hazard division must be valid");

            RuleFor(x => x.CompatibilityId)
                .GreaterThan(0).When(x => x.CompatibilityId.HasValue)
                .WithMessage("Compatibility must be valid");
        }
    }
}
