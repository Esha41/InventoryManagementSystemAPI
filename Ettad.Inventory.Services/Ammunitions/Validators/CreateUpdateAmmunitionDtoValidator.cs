using FluentValidation;
using Ettad.Inventory.Service.Ammunitions.Dtos;

namespace Ettad.Inventory.Service.Ammunitions.Validators
{
    public class CreateUpdateAmmunitionDtoValidator : AbstractValidator<CreateUpdateAmmunitionDto>
    {
        public CreateUpdateAmmunitionDtoValidator()
        {
            // Only Name and ItemNo are required
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.ItemNo)
                .NotEmpty().WithMessage("Item number is required")
                .MaximumLength(100).WithMessage("Item number cannot exceed 100 characters");

            // All other fields are optional - only validate if provided
            RuleFor(x => x.PartNo)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.PartNo))
                .WithMessage("Part number cannot exceed 100 characters");

            RuleFor(x => x.HccId)
                .GreaterThan(0).When(x => x.HccId.HasValue)
                .WithMessage("HCC must be greater than 0 when provided");

            RuleFor(x => x.BulletDiameter)
                .GreaterThan(0).When(x => x.BulletDiameter.HasValue)
                .WithMessage("Bullet diameter must be greater than 0 when provided");

            RuleFor(x => x.BulletDiameterUnitId)
                .GreaterThan(0).When(x => x.BulletDiameterUnitId.HasValue)
                .WithMessage("Unit must be greater than 0 when provided");

            RuleFor(x => x.ArmNumber)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.ArmNumber))
                .WithMessage("Arm number cannot exceed 200 characters");

            RuleFor(x => x.Primer)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Primer))
                .WithMessage("Primer cannot exceed 100 characters");

            RuleFor(x => x.TotalWeight)
                .GreaterThan(0).When(x => x.TotalWeight.HasValue)
                .WithMessage("Total weight must be greater than 0 when provided");

            RuleFor(x => x.CaseTypeId)
                .GreaterThan(0).When(x => x.CaseTypeId.HasValue)
                .WithMessage("Case type must be greater than 0 when provided");

            RuleFor(x => x.PropellantId)
                .GreaterThan(0).When(x => x.PropellantId.HasValue)
                .WithMessage("Propellant must be greater than 0 when provided");

            RuleFor(x => x.CompatibilityId)
                .GreaterThan(0).When(x => x.CompatibilityId.HasValue)
                .WithMessage("Compatibility must be greater than 0 when provided");

            RuleFor(x => x.HazardDivisionId)
                .GreaterThan(0).When(x => x.HazardDivisionId.HasValue)
                .WithMessage("Hazard division must be greater than 0 when provided");

            // Optional field validations
            RuleFor(x => x.NatureOptionId)
                .GreaterThan(0).When(x => x.NatureOptionId.HasValue)
                .WithMessage("Nature option ID must be greater than 0 when provided");

            RuleFor(x => x.PrimaryPurposId)
                .GreaterThan(0).When(x => x.PrimaryPurposId.HasValue)
                .WithMessage("Primary purpose ID must be greater than 0 when provided");

            RuleFor(x => x.ProjectileColorId)
                .GreaterThan(0).When(x => x.ProjectileColorId.HasValue)
                .WithMessage("Projectile color ID must be greater than 0 when provided");

            RuleFor(x => x.ProjectailMaterialId)
                .GreaterThan(0).When(x => x.ProjectailMaterialId.HasValue)
                .WithMessage("Projectile material ID must be greater than 0 when provided");

            // Optional field validations for Price and MinimumQuantity
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).When(x => x.Price.HasValue)
                .WithMessage("Price must be greater than or equal to 0 when provided");

            RuleFor(x => x.MinimumQuantity)
                .GreaterThan(0).When(x => x.MinimumQuantity.HasValue)
                .WithMessage("Minimum quantity must be greater than 0 when provided");
        }
    }
}

